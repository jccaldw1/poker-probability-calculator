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
        _cardsInPlay = cardsInPlay;
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

        foreach (MadeHand hand in madeHands)
        {
            // The hand enum goes from least to greatest (High Card = 0, Royal Flush = 9), so this comparison is good.
            if (hand.Hand > bestMadeHand.Hand)
            {
                bestMadeHand = hand;
            }
            else if (hand.Hand == bestMadeHand.Hand)
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

        if (cards.Count == 2)
        {
            bestPossibleHand = Hand.OnePair;
        }
        else
        {
            bestPossibleHand = Hand.RoyalFlush;
        }

        _cardsInPlay = cards;

        return RecursivelyDetermineHand(bestPossibleHand);
    }

    /// <summary>
    /// It takes years to learn.
    /// </summary>
    private MadeHand RecursivelyDetermineHand(Hand hand)
    {
        MadeHand? handResult = DetermineMadeHand(hand);
        if (handResult == null)
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
        switch (supposedHand)
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
        if (possibleFlush == null || possibleFlush.Value != Value.A)
        {
            return possibleRoyalFlush;
        }

        // The ace high straight is a necessary condition for a royal flush.
        if (possibleStraight == null || possibleStraight.Value != Value.A)
        {
            return possibleRoyalFlush;
        }

        Suit flushSuit = getSuitQuantities().Where(suitQuantity => suitQuantity.Value >= 5).First().Key;

        // All the cards in the ace high straight.
        List<Value> broadwayValues = new List<Value>() { Value.Num10, Value.J, Value.Q, Value.K, Value.A };

        if (
            _cardsInPlay
                // We know the player has the ace high straight, so this first filter will give at least five results.
                .Where(card => broadwayValues.Contains(card.Value))
                // If this filter results in five items in the collection, then we have a royal flush.
                .Where(card => card.Suit == flushSuit)
                .Count() == 5
            )
        {
            possibleRoyalFlush = new(Hand.RoyalFlush, Value.A, null, null, null, null);
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
        if (possibleStraight == null || possibleFlush == null)
        {
            return possibleStraightFlush;
        }

        Suit flushSuit = getSuitQuantities().Where(suitQuantity => suitQuantity.Value >= 5).First().Key;

        // Iterate through the straight values. If any of them are not the flush suit, then the player does not have a straight flush.
        for (int i = (int)possibleStraight.Value; i > (int)possibleStraight.Value - 4; i--)
        {
            if (!_cardsInPlay.Where(card => card.Value == (Value)i).Any(card => card.Suit == flushSuit))
            {
                return possibleStraightFlush;
            }
        }

        possibleStraightFlush = new(Hand.StraightFlush, possibleStraight.Value, null, null, null, null);

        return possibleStraightFlush;
    }

    private MadeHand? PlayerHasFourOfAKind()
    {
        MadeHand? possibleFourOfAKind = null;

        var fourOfAKind = getValueQuantities().Where(valueQuantity => valueQuantity.Value == 4);

        bool playerHasFourOfAKind = fourOfAKind.Any();

        if (!playerHasFourOfAKind)
        {
            return possibleFourOfAKind;
        }

        Value fourOfAKindValue = fourOfAKind.First().Key;

        // On occasion, two players have four of a kind, in which case the hand with the better "kicker", that is, the card not part of the four of a kind, determines the winner.
        Value highestCardValueNotInvolvedInFourOfAKind = _cardsInPlay.Where(card => card.Value != fourOfAKindValue).OrderByDescending(card => card.Value).First().Value;

        possibleFourOfAKind = new(Hand.FourOfAKind, fourOfAKind.First().Key, highestCardValueNotInvolvedInFourOfAKind, null, null, null);

        return possibleFourOfAKind;
    }

    private MadeHand? PlayerHasFullHouse()
    {
        MadeHand? possibleFullHouse = null;

        Dictionary<Value, int> valueQuantities = getValueQuantities();

        List<Value> valuesWithThreeOccurrences = new List<Value>();
        List<Value> valuesWithTwoOccurrences = new List<Value>();

        foreach (KeyValuePair<Value, int> valueQuantity in valueQuantities)
        {
            if (valueQuantity.Value == 3)
                valuesWithThreeOccurrences.Add(valueQuantity.Key);
            else if (valueQuantity.Value == 2)
                valuesWithTwoOccurrences.Add(valueQuantity.Key);
        }

        if (valuesWithThreeOccurrences.Count() == 1 && valuesWithTwoOccurrences.Count() >= 1)
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
        else if (valuesWithThreeOccurrences.Count() == 2)
        {
            IOrderedEnumerable<Value> orderedValues = valuesWithThreeOccurrences.OrderByDescending(value => value);

            possibleFullHouse = new MadeHand(
                Hand.FullHouse,
                // There can be no more than two threes of a kind in play, so first and last are safe ways to find them.
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
        if (flushSuits.Any())
        {
            Suit flushSuit = flushSuits.First();

            List<Card> orderedCardsInFlushSuit = _cardsInPlay.Where(card => card.Suit == flushSuit).OrderBy(card => card.Value).ToList();

            Value highestFlushCardValue = orderedCardsInFlushSuit.First().Value;

            Stack<Card> stackedCardsInFlushSuit = convertListToStack(orderedCardsInFlushSuit.ToList());

            // Two flushes can be identical except for the smallest card. The higher flush wins, so all Values must be issued.
            possibleFlush = new(
                Hand.Flush,
                // We pop sequentially to get the values of the flush.
                stackedCardsInFlushSuit.Pop().Value,
                stackedCardsInFlushSuit.Pop().Value,
                stackedCardsInFlushSuit.Pop().Value,
                stackedCardsInFlushSuit.Pop().Value,
                stackedCardsInFlushSuit.Pop().Value
            );

            return possibleFlush;

        }

        return possibleFlush;
    }

    private MadeHand? PlayerHasStraight()
    {
        MadeHand? possibleStraight = null;

        List<Card> cardsInAscendingOrder = _cardsInPlay.OrderBy(card => card.Value).ToList();

        List<Value> cardValues = getCardValues();

        // Run once if there are five cards in play, twice if there are six, and three times if there are seven.
        // This is the number of possible straights given the number of cards in play.
        // This loop starts at 2, 1, or 0 depending on if there are 7, 6, or 5 cards in play. This is so it finds the highest possible straight first.
        for (int i = _cardsInPlay.Count - 5; i >= 0; i--)
        {
            Value bottomCardValue = cardsInAscendingOrder.ElementAt(i).Value;

            if (cardValues.Contains(bottomCardValue)
                                && cardValues.Contains((Value)((int)bottomCardValue + 1))
                                && cardValues.Contains((Value)((int)bottomCardValue + 2))
                                && cardValues.Contains((Value)((int)bottomCardValue + 3))
                                && cardValues.Contains((Value)((int)bottomCardValue + 4)))
            {
                possibleStraight = new MadeHand(
                    Hand.Straight,
                    // The value of the straight is the highest card in the straight.
                    (Value)((int)bottomCardValue + 4),
                    null,
                    null,
                    null,
                    null);

                // Once the highest possible straight is found, break out.
                break;
            }
        }

        return possibleStraight;
    }

    private MadeHand? PlayerHasThreeOfAKind()
    {
        MadeHand? possibleThreeOfAKind = null;

        // By this point, since we have checked for a full house, we can be certain there is only one three of a kind in play, if any.
        var threeOfAKindQuantities = getValueQuantities().Where(valueQuantity => valueQuantity.Value == 3);

        if (threeOfAKindQuantities.Count() != 1)
        {
            return possibleThreeOfAKind;
        }

        Value threeOfAKindValue = threeOfAKindQuantities.First().Key;

        // A three of a kind has two kickers.
        Stack<Card> cardsOutsideOfThreeOfAKind = convertListToStack(_cardsInPlay.Where(card => card.Value != threeOfAKindValue).OrderBy(card => card.Value).ToList());

        possibleThreeOfAKind = new(Hand.ThreeOfAKind, threeOfAKindValue, cardsOutsideOfThreeOfAKind.Pop().Value, cardsOutsideOfThreeOfAKind.Pop().Value, null, null);

        return possibleThreeOfAKind;
    }

    private MadeHand? PlayerHasTwoPair()
    {
        MadeHand? possibleTwoPair = null;

        // Get the values that have pairs. There can be up to three on a board of seven cards.
        Stack<Value> pairs = convertListToStack(getValueQuantities().Where(valueQuantity => valueQuantity.Value == 2).Select(valueQuantity => valueQuantity.Key).OrderBy(value => value).ToList());

        if (pairs.Count() < 2)
        {
            return possibleTwoPair;
        }

        // The higher pair.
        var twoPairValue1 = pairs.Pop();
        // The lower pair.
        var twoPairValue2 = pairs.Pop();

        // The kicker that plays with this two pair.
        Value highestCardNotInTwoPair = _cardsInPlay
            // Cards not in the two pair.
            .Where(card => card.Value != twoPairValue1 && card.Value != twoPairValue2)
            .OrderByDescending(card => card.Value)
            .First().Value;

        possibleTwoPair = new(Hand.TwoPair, twoPairValue1, twoPairValue2, highestCardNotInTwoPair, null, null);

        return possibleTwoPair;
    }

    private MadeHand? PlayerHasOnePair()
    {
        MadeHand? possibleOnePair = null;

        // By this point we have determined that the player can either have one pair or high card, so there should only be one pair.
        var pair = getValueQuantities().Where(valueQuantity => valueQuantity.Value == 2).Select(valueQuantity => valueQuantity.Key);

        if (pair.Count() < 1)
            return possibleOnePair;

        var onePairValue = pair.First();

        // One pair hand has three "kickers", the three remaining cards with the highest value.
        Stack<Value> remainingCards = convertListToStack(_cardsInPlay.Where(card => card.Value != onePairValue).OrderBy(card => card.Value).Select(card => card.Value).ToList());

        possibleOnePair = new(Hand.OnePair, onePairValue, remainingCards.Pop(), remainingCards.Pop(), remainingCards.Pop(), null);

        return possibleOnePair;
    }

    private MadeHand PlayerHasHighCard()
    {
        // By this point, we are certain that the player only has a high card. So we can simply sort the cards in play to get the player's hand.

        Stack<Card> fiveHighestCards = convertListToStack(_cardsInPlay.OrderBy(card => card.Value).ToList());

        MadeHand highCard = new(Hand.HighCard, fiveHighestCards.Pop().Value, fiveHighestCards.Pop().Value, fiveHighestCards.Pop().Value, fiveHighestCards.Pop().Value, fiveHighestCards.Pop().Value);

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

        foreach (Card card in _cardsInPlay)
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

        foreach (Card card in _cardsInPlay)
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

    private Stack<T> convertListToStack<T>(List<T> list)
    {
        Stack<T> stack = new();

        foreach (var item in list)
        {
            stack.Push(item);
        }

        return stack;
    }
}
