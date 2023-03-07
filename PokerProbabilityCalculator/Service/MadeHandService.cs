using PokerProbabilityCalculator.Model;

namespace PokerProbabilityCalculator.Service;

/// <summary>
/// Given a list of player hands and a board, determines which made hand each player currently has.
/// </summary>
public class MadeHandService
{
    private List<Card> _cardsInPlay;

    public MadeHandService()
    {

    }

    public MadeHandService(List<Card> cardsInPlay)
    {
        this._cardsInPlay = cardsInPlay;
    }

    public MadeHandService(PlayerHand playerHand, Board board)
    {
        _cardsInPlay = new List<Card>
        {
            playerHand.Card1,
            playerHand.Card2
        };

        _cardsInPlay.AddRange(board.GetCards());
    }

    /// <summary>
    /// Given a list of made hands, determine which one wins the pot.
    /// </summary>
    /// <returns>The winning made hand.</returns>
    public MadeHand DetermineBestMadeHand(List<MadeHand> madeHands)
    {
        // The lowest possible hand in poker. A hand of 6-high, that is, a High Card hand where 6 is the highest card, does not exist.
        // We start with the lowest possible hand - if we see any hands better, replace it.
        MadeHand bestMadeHand = new(Hand.HighCard, Value.Num7, Value.Num6, Value.Num5, Value.Num4, Value.Num2);

        foreach(MadeHand hand in madeHands)
        {
            // The hand enum goes from least to greatest (High Card = 0, Royal Flush = 9), so this comparison is good.
            if(hand.Hand > bestMadeHand.Hand)
            {
                bestMadeHand = hand;
            }
            else if(hand.Hand == bestMadeHand.Hand)
            {
                // Compare the values of the two hands.
                if (hand.Value > bestMadeHand.Value)
                    bestMadeHand = hand;
                else if (hand.Value2 > bestMadeHand.Value2)
                    bestMadeHand = hand;
                else if (hand.Value3 > bestMadeHand.Value3)
                    bestMadeHand = hand;
                else if (hand.Value4 > bestMadeHand.Value4)
                    bestMadeHand = hand;
                else if (hand.Value5 > bestMadeHand.Value5)
                    bestMadeHand = hand;
            }
        }

        return bestMadeHand;
    }

    /// <summary>
    /// Determine's the player's made hand from their cards and the board.
    /// </summary>
    public MadeHand DetermineMadeHand()
    {
        Hand bestPossibleHand;

        // There can be 2, 5, 6, or 7 cards in play. If there are only two cards in play, the best hand the player can have is one pair.
        if (_cardsInPlay.Count == 2)
        {
            bestPossibleHand = Hand.OnePair;
        }
        else
        {
            bestPossibleHand = Hand.RoyalFlush;
        }

        // Iterate through the hands from best to worst. We return early when we find the best hand the player has. This means each function determining whether
        // the player has a certain hand assumes the player does not have hands better than the one being looked for.
        return RecursivelyDetermineHand(bestPossibleHand);
    }

    public MadeHand DetermineMadeHand(PlayerHand playerHand, Board board)
    {
        Hand bestPossibleHand;

        List<Card> cards = new List<Card>
        {
            playerHand.Card1,
            playerHand.Card2
        };
        cards.AddRange(board.GetCards());

        if(cards.Count == 2)
        {
            bestPossibleHand = Hand.OnePair;
        }
        else
        {
            bestPossibleHand = Hand.RoyalFlush;
        }

        return RecursivelyDetermineHand(bestPossibleHand);
    }

    /// <summary>
    /// It takes years to learn.
    /// </summary>
    private MadeHand RecursivelyDetermineHand(Hand hand)
    {
        MadeHand? handResult = DetermineMadeHand(hand);
        if(handResult == null)
        {
            return RecursivelyDetermineHand((Hand)((int)hand - 1));
        }
        else
        {
            return handResult;
        }
    }

    private MadeHand? DetermineMadeHand(Hand supposedHand)
    {
        switch(supposedHand)
        {
            case Hand.RoyalFlush:
                return PlayerHasRoyalFlush();
            case Hand.StraightFlush:
                return PlayerHasStraightFlush();
            case Hand.FourOfAKind:
                return PlayerHasFourOfAKind();
            case Hand.FullHouse:
                return PlayerHasFullHouse();
            case Hand.Flush:
                return PlayerHasFlush();
            case Hand.Straight:
                return PlayerHasStraight();
            case Hand.ThreeOfAKind:
                return PlayerHasThreeOfAKind();
            case Hand.TwoPair:
                return PlayerHasTwoPair();
            case Hand.OnePair:
                return PlayerHasOnePair();
            case Hand.HighCard:
                return PlayerHasHighCard();
            default:
                return null;
        }
    }

    private MadeHand? PlayerHasRoyalFlush()
    {
        MadeHand? possibleRoyalFlush = null;

        MadeHand? possibleFlush = PlayerHasFlush();
        MadeHand? possibleStraight = PlayerHasStraight();

        // The ace high flush is a necessary condition for a royal flush.
        if(possibleFlush == null || possibleFlush.Value != Value.A)
        {
            return possibleRoyalFlush;
        }

        // The ace high straight is a necessary condition for a royal flush.
        if(possibleStraight == null || possibleStraight.Value != Value.A)
        {
            return possibleRoyalFlush;
        }
        
        Suit flushSuit = getSuitQuantities().Where(suitQuantity => suitQuantity.Value >= 5).First().Key;

        // All the cards in the ace high straight.
        List<Value> broadwayValues = new List<Value>() { Value.Num10, Value.J, Value.Q, Value.K, Value.A };

        if(
            _cardsInPlay
                // We know the player has the ace high straight, so this first filter will give at least five results.
                .Where(card => broadwayValues.Contains(card.Value))
                // If this filter results in five items in the collection, then we have a royal flush.
                .Where(card => card.Suit == flushSuit)
                .Count() == 5
            )
        {
            possibleRoyalFlush.Hand = Hand.RoyalFlush;
            return possibleRoyalFlush;
        }

        return possibleRoyalFlush;
    }

    private MadeHand? PlayerHasStraightFlush()
    {
        MadeHand? possibleStraightFlush = null;

        MadeHand? possibleStraight = PlayerHasStraight();
        MadeHand? possibleFlush = PlayerHasFlush();

        // A straight is a necessary condition for having a straight flush, as is a flush.
        if(possibleStraight == null || possibleFlush == null)
        {
            return possibleStraightFlush;
        }

        Suit flushSuit = getSuitQuantities().Where(suitQuantity => suitQuantity.Value >= 5).First().Key;

        // Iterate through the straight values. If any of them are not the flush suit, then the player does not have a straight flush.
        for(int i = (int)possibleStraight.Value; i > (int)possibleStraight.Value - 4; i--)
        {
            if(!_cardsInPlay.Where(card => card.Value == (Value)i).Any(card => card.Suit == flushSuit))
            {
                return possibleStraightFlush;
            }
        }

        possibleStraightFlush = new MadeHand();

        possibleStraightFlush.Hand = Hand.StraightFlush;
        possibleStraightFlush.Value = possibleStraight.Value;

        return possibleStraightFlush;
    }

    private MadeHand? PlayerHasFourOfAKind()
    {
        MadeHand? possibleFourOfAKind = null;

        var fourOfAKind = getValueQuantities().Where(valueQuantity => valueQuantity.Value == 4);

        bool playerHasFourOfAKind = fourOfAKind.Any();

        if(!playerHasFourOfAKind)
        {
            return possibleFourOfAKind;
        }

        possibleFourOfAKind = new MadeHand();

        possibleFourOfAKind.Hand = Hand.FourOfAKind;
        possibleFourOfAKind.Value = fourOfAKind.First().Key;

        // On occasion, two players have four of a kind, in which case the hand with the better "kicker", that is, the card not part of the four of a kind, determines the winner.
        Value highestCardValueNotInvolvedInFourOfAKind = _cardsInPlay.Where(card => card.Value != possibleFourOfAKind.Value).OrderByDescending(card => card.Value).First().Value;

        possibleFourOfAKind.Value2 = highestCardValueNotInvolvedInFourOfAKind;

        return possibleFourOfAKind;
    }

    private MadeHand? PlayerHasFullHouse()
    {
        MadeHand? possibleFullHouse = null;

        Dictionary<Value, int> valueQuantities = getValueQuantities();

        List<Value> valuesWithThreeOccurrences = new List<Value>();
        List<Value> valuesWithTwoOccurrences = new List<Value>();

        foreach(KeyValuePair<Value, int> valueQuantity in valueQuantities)
        {
            if(valueQuantity.Value == 3)
                valuesWithThreeOccurrences.Add(valueQuantity.Key);
            else if(valueQuantity.Value == 2)
                valuesWithTwoOccurrences.Add(valueQuantity.Key);
        }

        if(valuesWithThreeOccurrences.Count() == 1 && valuesWithTwoOccurrences.Count() >= 1)
        {
            possibleFullHouse = new MadeHand(
                Hand.FullHouse,
                // A full house has two values - first the three of a kind, then the pair.
                valuesWithThreeOccurrences.First(),
                valuesWithTwoOccurrences.OrderByDescending(value => value).First(),
                null,
                null,
                null
                );
        } 
        // Full houses can happen when there are two threes of a kind in play.
        else if(valuesWithThreeOccurrences.Count() == 2)
        {
            IOrderedEnumerable<Value> orderedValues = valuesWithThreeOccurrences.OrderByDescending(value => value);

            possibleFullHouse = new MadeHand(
                Hand.FullHouse,
                orderedValues.First(),
                orderedValues.Last(),
                null,
                null,
                null);
        }

        return possibleFullHouse;
    }

    private MadeHand? PlayerHasFlush()
    {
        MadeHand? possibleFlush = null;

        // This list can contain one or zero elements.
        IEnumerable<Suit> flushSuits = getSuitQuantities().Where(suitQuantity => suitQuantity.Value >= 5).Select(suitQuantity => suitQuantity.Key).ToList();

        // If any of the suits occur five or more times:
        if(flushSuits.Any())
        {
            Suit flushSuit = flushSuits.First();

            possibleFlush = new MadeHand();

            possibleFlush.Hand = Hand.Flush;

            IOrderedEnumerable<Card> orderedCardsInFlushSuit = _cardsInPlay.Where(card => card.Suit == flushSuit).OrderByDescending(card => card.Value);
            Value highestFlushCardValue = orderedCardsInFlushSuit.First().Value;

            Stack<Card> stackedCardsInFlushSuit = (Stack<Card>)orderedCardsInFlushSuit;

            // Two flushes can be identical except for the smallest card. The higher flush wins, so all Values must be issued.
            possibleFlush.Value = stackedCardsInFlushSuit.Pop().Value;
            possibleFlush.Value2 = stackedCardsInFlushSuit.Pop().Value;
            possibleFlush.Value3 = stackedCardsInFlushSuit.Pop().Value;
            possibleFlush.Value4 = stackedCardsInFlushSuit.Pop().Value;
            possibleFlush.Value5 = stackedCardsInFlushSuit.Pop().Value;

            return possibleFlush;

        }

        return possibleFlush;
    }

    private MadeHand? PlayerHasStraight()
    {
        MadeHand? possibleStraight = null;

        IOrderedEnumerable<Card> cardsInAscendingOrder = _cardsInPlay.OrderBy(card => card.Value);

        List<Value> cardValues = getCardValues();

        // Run once if there are five cards in play, twice if there are six, and three times if there are seven.
        // This is the number of possible straights given the number of cards in play.
        // This loop starts at 2, 1, or 0 depending on if there are 7, 6, or 5 cards in play. This is so it finds the highest possible straight first.
        for (int i = _cardsInPlay.Count - 5; i <= 0; i--)
        {
            Value bottomCardValue = cardsInAscendingOrder.ElementAt(i).Value;

            if(cardValues.Contains(bottomCardValue)
                                && cardValues.Contains((Value)((int)bottomCardValue + 1))
                                && cardValues.Contains((Value)((int)bottomCardValue + 2))
                                && cardValues.Contains((Value)((int)bottomCardValue + 3))
                                && cardValues.Contains((Value)((int)bottomCardValue + 4)))
            {
                possibleStraight = new MadeHand();

                possibleStraight.Hand = Hand.Straight;

                // The value of the straight is the highest card in the straight.
                possibleStraight.Value = (Value)((int)bottomCardValue + 4);
            }
        }

        return possibleStraight;
    }

    private MadeHand? PlayerHasThreeOfAKind()
    {
        MadeHand? possibleThreeOfAKind = null;

        // By this point, since we have checked for a full house, we can be certain there is only one three of a kind in play, if any.
        var threeOfAKindQuantities = getValueQuantities().Where(valueQuantity => valueQuantity.Value == 3);

        if(threeOfAKindQuantities.Count() != 1)
        {
            return possibleThreeOfAKind;
        }

        possibleThreeOfAKind = new();

        possibleThreeOfAKind.Hand = Hand.ThreeOfAKind;
        possibleThreeOfAKind.Value = threeOfAKindQuantities.First().Key;

        // A three of a kind has two kickers.
        Stack<Card> cardsOutsideOfThreeOfAKind = (Stack<Card>)_cardsInPlay.Where(card => card.Value != possibleThreeOfAKind.Value).OrderByDescending(card => card.Value);

        possibleThreeOfAKind.Value2 = cardsOutsideOfThreeOfAKind.Pop().Value;
        possibleThreeOfAKind.Value3 = cardsOutsideOfThreeOfAKind.Pop().Value;

        return possibleThreeOfAKind;
    }

    private MadeHand? PlayerHasTwoPair()
    {
        MadeHand? possibleTwoPair = null;

        // Get the values that have pairs. There can be up to three on a board of seven cards.
        Stack<Value> pairs = (Stack<Value>)getValueQuantities().Where(valueQuantity => valueQuantity.Value == 2).Select(valueQuantity => valueQuantity.Key).OrderByDescending(value => value);

        if(pairs.Count() < 2)
        {
            return possibleTwoPair;
        }

        possibleTwoPair = new MadeHand();
        possibleTwoPair.Hand = Hand.TwoPair;

        // The higher pair.
        possibleTwoPair.Value = pairs.Pop();
        // The lower pair.
        possibleTwoPair.Value2 = pairs.Pop();

        // The kicker that plays with this two pair.
        Value highestCardNotInTwoPair = _cardsInPlay
            // Cards not in the two pair.
            .Where(card => card.Value != possibleTwoPair.Value && card.Value != possibleTwoPair.Value2)
            .OrderByDescending(card => card.Value)
            .First().Value;

        possibleTwoPair.Value3 = highestCardNotInTwoPair;

        return possibleTwoPair;
   }

    private MadeHand? PlayerHasOnePair()
    {
        MadeHand? possibleOnePair = null;

        // By this point we have determined that the player can either have one pair or high card, so there should only be one pair.
        var pair = getValueQuantities().Where(valueQuantity => valueQuantity.Value == 2).Select(valueQuantity => valueQuantity.Key);

        if (pair.Count() < 1)
            return possibleOnePair;

        possibleOnePair = new MadeHand();
        possibleOnePair.Hand = Hand.OnePair;
        possibleOnePair.Value = pair.First();

        // One pair hand has three "kickers" the three remaining cards with the highest value.
        Stack<Value> remainingCards = (Stack<Value>)_cardsInPlay.Where(card => card.Value != possibleOnePair.Value).OrderByDescending(card => card.Value).Select(card => card.Value);

        possibleOnePair.Value2 = remainingCards.Pop();
        possibleOnePair.Value3 = remainingCards.Pop();
        possibleOnePair.Value4 = remainingCards.Pop();

        return possibleOnePair;
    }

    private MadeHand PlayerHasHighCard()
    {
        // By this point, we are certain that the player only has a high card. So we can simply sort the cards in play to get the player's hand.

        Stack<Card> fiveHighestCards = (Stack<Card>)_cardsInPlay.OrderByDescending(card => card.Value).Take(5);

        MadeHand highCard = new MadeHand();
        highCard.Hand = Hand.HighCard;
        highCard.Value = fiveHighestCards.Pop().Value;
        highCard.Value2 = fiveHighestCards.Pop().Value;
        highCard.Value3 = fiveHighestCards.Pop().Value;
        highCard.Value4 = fiveHighestCards.Pop().Value;
        highCard.Value5 = fiveHighestCards.Pop().Value;

        return highCard;
    }


    /// <summary>
    /// Gets the values of all the cards in play. Useful for finding out if the cards are consecutive or if there are cards with the same value.
    /// </summary>
    private List<Value> getCardValues()
    {
        return _cardsInPlay.Select(card => card.Value).ToList();
    }

    /// <summary>
    /// Gets the frequency of each card value of the cards in play.
    /// </summary>
    private Dictionary<Value, int> getValueQuantities()
    {
        Dictionary<Value, int> valueFrequency = new();

        foreach(Card card in _cardsInPlay)
        {
            if (valueFrequency.ContainsKey(card.Value))
            {
                valueFrequency[card.Value]++;
            }
            else
            {
                valueFrequency.Add(card.Value, 1);
            }
        }

        return valueFrequency;

    }

    /// <summary>
    /// Gets the frequency of each suit of the cards in play.
    /// </summary>
    private Dictionary<Suit, int> getSuitQuantities()
    {
        Dictionary<Suit, int> suitFrequency = new();

        foreach(Card card in _cardsInPlay)
        {
            if (suitFrequency.ContainsKey(card.Suit))
            {
                suitFrequency[card.Suit]++;
            }
            else
            {
                suitFrequency.Add(card.Suit, 1);
            }
        }

        return suitFrequency;
    }
}
