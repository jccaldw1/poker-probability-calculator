using PokerProbabilityCalculator.Model;

namespace PokerProbabilityCalculator.Service;

/// <summary>
/// Given a list of hands and a board, determines whether each hand can be made. If a hand cannot be made, returns null. If it can be made, returns the list of cards in the deck that can be added to the board to make that hand.
/// </summary>
public class MadeHandPossibilityService
{
    private Board _board;
    private List<PlayerHand> _playerHands;
    private Deck _deck;

    public MadeHandPossibilityService(List<PlayerHand> hands, Board board)
    {
        _board = board;
        _playerHands = hands;
        _deck = new();

        _deck.RemoveRange(board.GetCards());

        hands.ForEach(hand =>
        {
            _deck.RemoveCard(hand.Card1);
            _deck.RemoveCard(hand.Card2);
        });
    }

    /// <returns>
    /// The cards that need to be dealt in order to make a royal flush with the passed hand. If a royal flush
    /// is not possible, returns null.
    /// </returns>
    public List<Card>? NecessaryRoyalFlushCards(PlayerHand hand)
    {
        List<Value> BroadwayCards = new()
        {
            Value.A,
            Value.K,
            Value.Q,
            Value.J,
            Value.Num10
        };

        int cardsThatNeedToContributeToRoyalFlush = _board.GetCards().Count;

        List<Card> totalCardsInPlay = new List<Card>
        {
            hand.Card1,
            hand.Card2
        };

        totalCardsInPlay.AddRange(_board.GetCards());

        var broadwayCardsInPlay = totalCardsInPlay.Where(card => BroadwayCards.Contains(card.Value));

        List<Suit> suitsThatCouldHaveRoyalFlush = new();

        // See if a royal flush is possible for each suit given the cards in play.
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            var suitedBroadwayCardsInPlay = broadwayCardsInPlay.Where(card => card.Suit == suit);

            if (suitedBroadwayCardsInPlay.Count() >= cardsThatNeedToContributeToRoyalFlush)
                suitsThatCouldHaveRoyalFlush.Add(suit);
        }

        List<Card>? cardsThatCouldMakeRoyalFlush = null;

        // Additionally, we need to examine the deck to determine if the cards needed are there.
        foreach (Suit suit in suitsThatCouldHaveRoyalFlush)
        {
            List<Card> royalFlushCardsForThisSuit = new();

            foreach (Value value in BroadwayCards)
                royalFlushCardsForThisSuit.Add(new Card(suit, value));

            var suitedBroadwayCards = broadwayCardsInPlay.Where(card => card.Suit == suit).ToList();

            var missingRoyalFlushCards = royalFlushCardsForThisSuit.Except(suitedBroadwayCards).ToList();

            if (_deck.Contains(missingRoyalFlushCards))
            {
                cardsThatCouldMakeRoyalFlush ??= new();
                cardsThatCouldMakeRoyalFlush.AddRange(missingRoyalFlushCards);
            }
        }

        return cardsThatCouldMakeRoyalFlush;
    }

    public List<Card>? NecessaryStraightFlushCards(PlayerHand hand)
    {
        List<Card> cardsThatCanMakeStraightFlush = new();
        List<Card> cardsInPlay = new();
        
        cardsInPlay.Add(hand.Card1);
        cardsInPlay.Add(hand.Card2);
        
        cardsInPlay.AddRange(_board.GetCards());
        
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            // what about blocking hands???
            List<Card>? cardsOfSuitThatCanMakeStraightFlush = StraightPossibilityService.CanMakeStraightFlush(hand, _board, suit, _deck);
            if (cardsOfSuitThatCanMakeStraightFlush != null)
            {
                cardsThatCanMakeStraightFlush.AddRange(cardsOfSuitThatCanMakeStraightFlush);
            }
        }

        if (cardsThatCanMakeStraightFlush.Count == 0)
            return null;
        
        return cardsThatCanMakeStraightFlush;
    }

    public List<Card>? NecessaryCardsToMakeFourOfAKind(PlayerHand hand)
    {
        // Four of a kind requires only four cards.
        int numberOfCardsThatNeedToContributeToFourOfAKind = Math.Max(4 - _board.GetCards().Count, 0);

        List<Card> cardsInPlay = new()
        {
            hand.Card1,
            hand.Card2
        };

        _board.GetCards().ForEach(cardsInPlay.Add);

        List<KeyValuePair<Value, int>> frequenciesOfValues = new();

        cardsInPlay.ForEach(card =>
        {
            frequenciesOfValues.Add(new(card.Value, cardsInPlay.Where(cardInPlay => cardInPlay.Value == card.Value).Count()));
        });

        frequenciesOfValues = frequenciesOfValues.Distinct().ToList();

        List<Value> valuesToLookFor = new();

        frequenciesOfValues.ForEach(frequency =>
        {
            if(frequency.Value > numberOfCardsThatNeedToContributeToFourOfAKind)
                valuesToLookFor.Add(frequency.Key);
        });

        List<Card>? cardsThatMakeFourOfAKind = null;

        foreach(Value value in valuesToLookFor)
        {
            IEnumerable<Card> cardsInDeckWithValue = _deck.cards.Where(card => card.Value == value);

            if (cardsInDeckWithValue.Count() + frequenciesOfValues.First(card => card.Key == value).Value == 4)
            {
                cardsThatMakeFourOfAKind ??= new();
                cardsThatMakeFourOfAKind.AddRange(cardsInDeckWithValue);
            }
        }

        return cardsThatMakeFourOfAKind;
    }

    public List<Card>? NecessaryCardsToMakeFullHouse(PlayerHand hand)
    {
        // We need to check that both a three-of-a-kind and a pair of a different value are possible.
        List<Value> possibleThreeOfAKindValues = new();
        List<Value> possiblePairValues = new();

        List<Card> cardsInPlay = new()
        {
            hand.Card1,
            hand.Card2
        };

        _board.GetCards().ForEach(cardsInPlay.Add);

        List<KeyValuePair<Value, int>> valueFrequencies = new();

        // Get the number of times each value appears in the cards in play.
        cardsInPlay
            .Select(card => card.Value)
            .Distinct()
            .ToList()
            .ForEach(value =>
        {
            valueFrequencies.Add(new KeyValuePair<Value, int>(value, cardsInPlay.Where(card => card.Value == value).Count()));
        });

        List<KeyValuePair<Value, int>> valueFrequenciesIncludingCardsInDeck = new();

        foreach(KeyValuePair<Value, int> valueFrequency in valueFrequencies)
        {
            if(valueFrequency.Value + _deck.cards.Where(card => card.Value == valueFrequency.Key).Count() >= 3)
                possibleThreeOfAKindValues.Add(valueFrequency.Key);

            if (valueFrequency.Value + _deck.cards.Where(card => card.Value == valueFrequency.Key).Count() >= 2)
                possiblePairValues.Add(valueFrequency.Key);
        }

        List<Card>? cardsThatCouldHelpMakeFullHouse = null;

        foreach(var possibleThreeOfAKindValue in possibleThreeOfAKindValues)
        {
            IEnumerable<Value> possiblePairValuesExcludingThreeOfAKindValue = possiblePairValues.Where(possiblePair => possiblePair != possibleThreeOfAKindValue);

            if (possiblePairValuesExcludingThreeOfAKindValue.Count() > 0)
            {
                cardsThatCouldHelpMakeFullHouse ??= new();
                cardsThatCouldHelpMakeFullHouse.AddRange(_deck.cards.Where(card => card.Value == possibleThreeOfAKindValue));
                cardsThatCouldHelpMakeFullHouse.AddRange(_deck.cards.Where(card => possiblePairValuesExcludingThreeOfAKindValue.Contains(card.Value)));
            }
        }

        return cardsThatCouldHelpMakeFullHouse;
    }

    public List<Card>? NecessaryCardsToMakeFlush(PlayerHand hand)
    {
        List<Suit> possibleFlushSuits = new();

        int cardsThatNeedToContributeToFlush = _board.NumberOfCardsPlayed();

        List<Card>? cardsThatCouldMakeFlush = null;

        foreach(Suit suit in Enum.GetValues(typeof(Suit)))
        {
            List<Card> cardsInPlayOfThisSuit = new();

            if (hand.Card1.Suit == suit)
                cardsInPlayOfThisSuit.Add(hand.Card1);

            if (hand.Card2.Suit == suit)
                cardsInPlayOfThisSuit.Add(hand.Card2);

            cardsInPlayOfThisSuit.AddRange(_board.GetCards().Where(card => card.Suit == suit));

            if(cardsInPlayOfThisSuit.Count() >= cardsThatNeedToContributeToFlush)
            {
                var cardsInDeckOfThisSuit = _deck.cards.Where(card => card.Suit == suit);

                // Check that the deck has enough cards to make a flush.
                if(cardsInDeckOfThisSuit.Count() >= 5 - cardsInPlayOfThisSuit.Count())
                {
                    cardsThatCouldMakeFlush ??= new();
                    cardsThatCouldMakeFlush.AddRange(cardsInDeckOfThisSuit);
                }
            }
        }

        return cardsThatCouldMakeFlush;
    }

    public List<Card>? NecessaryCardsToMakeStraight(PlayerHand hand) => StraightPossibilityService.CanMakeStraight(hand, _board, _deck);

    public List<Card>? NecessaryCardsToMakeThreeOfAKind(PlayerHand hand)
    {
        List<Card> cardsInPlay = new()
        {
            hand.Card1,
            hand.Card2
        };

        _board.GetCards().ForEach(cardsInPlay.Add);

        int cardsLeftToBePlayed = 5 - _board.NumberOfCardsPlayed();

        List<KeyValuePair<Value,int>> valueFrequencies = new();

        cardsInPlay.ForEach(card =>
        {
            var valueFrequencyForThisValue = valueFrequencies.Where(value => card.Value == value.Key);

            // TODO: Stop this nonsense. There should only be zero or one, so stop considering it in the code.
            if(valueFrequencyForThisValue.Count() == 0)
            {
                valueFrequencies.Add(new(card.Value, 1));
            }
            else if (valueFrequencyForThisValue.Count() == 1)
            {
                var frequencyForThisValue = valueFrequencyForThisValue.First().Value;
                valueFrequencies.Remove(valueFrequencyForThisValue.First());

                valueFrequencies.Add(new(card.Value, frequencyForThisValue++));
            }
            else
            {
                throw new Exception("multiple value frequencies???");
            }
        });

        List<Card>? cardsThatCouldMakeThreeOfAKind = null;

        foreach(var frequency in valueFrequencies)
        {
            int cardsNeededForThereToBeThreeOfThisValue = Math.Max(3 - cardsLeftToBePlayed, 0);

            // TODO: Figure out what to do in this case: the hand being looked for is already on the board.
            // There already exists a three-of-a-kind dummy!!!!
            //if (frequency.Value >= 3)
            //    return true;

            // If this card is frequent enough to potentially have 3 by the river:
            if(frequency.Value >= cardsNeededForThereToBeThreeOfThisValue)
            {
                // Then it is possible to make three of a kind, as long as the deck contains enough of that card.
                if(_deck.cards.Where(card => card.Value == frequency.Key).Count() >= 3 - frequency.Value)
                {
                    cardsThatCouldMakeThreeOfAKind ??= new();
                    cardsThatCouldMakeThreeOfAKind.AddRange(_deck.cards.Where(card => card.Value == frequency.Key));
                }
            }
        }

        return cardsThatCouldMakeThreeOfAKind;
    }

    public List<Card>? isTwoPairPossible(PlayerHand hand)
    {
        List<Card> cardsInPlay = new()
        {
            hand.Card1,
            hand.Card2
        };

        _board.GetCards().ForEach(cardsInPlay.Add);

        List<Value> valuesWithPairs = new();

        foreach (var card in cardsInPlay)

            if (cardsInPlay.Where(cardToCheck => cardToCheck.Value == card.Value).Count() >= 2)
                valuesWithPairs.Add(card.Value);

        //if (valuesWithPairs.Count() >= 2)
        //    return true;

        List<Card>? cardsThatCouldHelpMakeTwoPair = null;

        // If we have no pairs and there are four or more cards played, we cannot possibly make two pair.
        if (valuesWithPairs.Count() == 0 && _board.NumberOfCardsPlayed() >= 4)
        {
            return cardsThatCouldHelpMakeTwoPair;
        }

        List<Value> valuesInPlayWithoutPairs = cardsInPlay.Select(card => card.Value).Distinct().Except(valuesWithPairs).ToList();

        int numberOfPairs = valuesWithPairs.Count();

        // We now have at least two cards to work with.
        for (int i = 0; i < 2 - numberOfPairs; i++)
        {
            valuesInPlayWithoutPairs.ForEach(value =>
            {
                // If we can find a card to pair this value, add it.
                if (_deck.cards.Any(card => card.Value == value))
                {
                    cardsThatCouldHelpMakeTwoPair ??= new();
                    cardsThatCouldHelpMakeTwoPair.AddRange(_deck.cards.Where(card => card.Value == value));
                }
            });
        }

        // Now we have found some more potential pairs. So we can simply check if valuesWithPairs has at least two pairs.
        if (valuesWithPairs.Count() < 2)
            return null;

        return cardsThatCouldHelpMakeTwoPair;
    }

    public bool isPairPossible(PlayerHand hand)
    {
        List<Card> cardsInPlay = new()
        {
            hand.Card1,
            hand.Card2,
        };

        cardsInPlay.AddRange(_board.GetCards());

        List<Value> valuesInPlay = new();

        foreach(Card card in cardsInPlay)
        {
            // If the value already is in the list of values, then we have a pair.
            if (valuesInPlay.Contains(card.Value))
            {
                return true;
            }
            else
            {
                valuesInPlay.Add(card.Value);
            }

        }

        // At this point all the values in play are unique. If we found a duplicate we have already returned true.

        if(_board.NumberOfCardsPlayed() < 5)
        {
            foreach(Value value in valuesInPlay)
            {
                // If we can find another card with this value, return true;
                if (_deck.cards.Any(card => card.Value == value))
                {
                    return false;
                }
            }
        }
        else
        {
            // If we have played all cards and don't have a pair, a pair isn't possible. Amazing!!!!
            return false;
        }

        return false;
    }
}
