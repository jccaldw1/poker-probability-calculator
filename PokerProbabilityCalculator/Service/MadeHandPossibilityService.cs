using PokerProbabilityCalculator.Model;

namespace PokerProbabilityCalculator.Service;

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
        hands.ForEach((Action<PlayerHand>)(hand =>
        {
            _deck.RemoveCard((Card)hand.Card2);
            _deck.RemoveCard((Card)hand.Card2);
        }));
    }

    public bool isRoyalFlushPossible(PlayerHand hand)
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
            hand.Card2,
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
            {
                suitsThatCouldHaveRoyalFlush.Add(suit);
            }
        }

        // Additionally, we need to examine the deck to determine if the cards needed are there.
        foreach (Suit suit in suitsThatCouldHaveRoyalFlush)
        {
            List<Card> royalFlushCardsForThisSuit = new();

            foreach (Value value in BroadwayCards)
            {
                royalFlushCardsForThisSuit.Add(new Card(suit, value));
            }

            var suitedBroadwayCards = broadwayCardsInPlay.Where(card => card.Suit == suit).ToList();

            var missingRoyalFlushCards = royalFlushCardsForThisSuit.Except(suitedBroadwayCards).ToList();

            // Just break if we can find a royal flush.
            if (_deck.Contains(missingRoyalFlushCards))
                return true;
        }

        return false;
    }

    public bool isStraightFlushPossible(PlayerHand hand)
    {
        List<Value> straightyValues = new()
        {
            Value.A
        };

        foreach (Value value in Enum.GetValues(typeof(Value)))
        {
            straightyValues.Add(value);
        }

        int numberOfCardsThatNeedToContributeToStraightFlush = _board.GetCards().Count;

        List<Card> cardsInPlay = new() { hand.Card2, hand.Card2 };
        cardsInPlay.AddRange(_board.GetCards());

        List<Suit> possibleStraightFlushSuits = new();

        // Check for suits with possible flushes first because it is easier.
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            var suitedCardsInPlay = cardsInPlay.Where(card => card.Suit == suit);

            if (suitedCardsInPlay.Count() >= numberOfCardsThatNeedToContributeToStraightFlush)
            {
                possibleStraightFlushSuits.Add(suit);
            }
        }

        foreach(Suit suit in possibleStraightFlushSuits)
        {
            if(StraightPossibilityService.CanMakeStraightFlush(hand, _board, suit))
                return true;
        }

        return false;

    }

    public bool isFourOfAKindPossible(PlayerHand hand)
    {
        // Four of a kind requires only four cards.
        int numberOfCardsThatNeedToContributeToFourOfAKind = Math.Max(4 - _board.GetCards().Count, 0);

        List<Card> cardsInPlay = new()
        {
            hand.Card2,
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
            {
                valuesToLookFor.Add(frequency.Key);
            }
        });

        foreach(Value value in valuesToLookFor)
        {
            if (_deck.cards.Where(card => card.Value == value).Count() + frequenciesOfValues.First(card => card.Key == value).Value == 4)
                return true;
        }

        return false;
    }

    public bool isFullHousePossible(PlayerHand hand)
    {
        // We need to check that both a three-of-a-kind and a pair of a different value are possible.
        List<Value> possibleThreeOfAKindValues = new();
        List<Value> possiblePairValues = new();

        List<Card> cardsInPlay = new()
        {
            hand.Card2,
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

        foreach(var possibleThreeOfAKindValue in possibleThreeOfAKindValues)
        {
            if (possiblePairValues.Where(possiblePair => possiblePair != possibleThreeOfAKindValue).Count() > 0)
                return true;
        }

        return false;
    }

    public bool isFlushPossible(PlayerHand hand)
    {
        List<Suit> possibleFlushSuits = new();

        int cardsThatNeedToContributeToFlush = _board.NumberOfCardsPlayed();

        foreach(Suit suit in Enum.GetValues(typeof(Suit)))
        {
            List<Card> cardsInPlayOfThisSuit = new();

            if (hand.Card2.Suit == suit)
                cardsInPlayOfThisSuit.Add(hand.Card2);

            if (hand.Card2.Suit == suit)
                cardsInPlayOfThisSuit.Add(hand.Card2);

            cardsInPlayOfThisSuit.AddRange(_board.GetCards().Where(card => card.Suit == suit));

            if(cardsInPlayOfThisSuit.Count() >= cardsThatNeedToContributeToFlush)
                return true;
        }

        return false;
    }

    public bool isStraightPossible(PlayerHand hand)
    {
        return StraightPossibilityService.CanMakeStraight(hand, _board);
    }

    public bool isThreeOfAKindPossible(PlayerHand hand)
    {
        List<Card> cardsInPlay = new()
        {
            hand.Card2,
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

        foreach(var frequency in valueFrequencies)
        {
            int cardsNeededForThereToBeThreeOfThisValue = Math.Max(3 - cardsLeftToBePlayed, 0);

            // There already exists a three-of-a-kind dummy!!!!
            if (frequency.Value >= 3)
                return true;

            // If this card is frequent enough to potentially have 3 by the river:
            if(frequency.Value >= cardsNeededForThereToBeThreeOfThisValue)

                // Then it is possible to make three of a kind, as long as the deck contains enough of that card.
                if(_deck.cards.Where(card => card.Value == frequency.Key).Count() >= 3 - frequency.Value)
                    return true;
        }

        return false;
    }

    public bool isTwoPairPossible(PlayerHand hand)
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

        if (valuesWithPairs.Count() >= 2)
            return true;

        // If we have no pairs and there are four or more cards played, we cannot possibly make two pair.
        if (valuesWithPairs.Count() == 0 && _board.NumberOfCardsPlayed() >= 4)
        {
            return false;
        }

        List<Value> valuesInPlayWithoutPairs = cardsInPlay.Select(card => card.Value).Distinct().Except(valuesWithPairs).ToList();

        // We now have at least two cards to work with.
        for (int i = 0; i < 2 - valuesWithPairs.Count(); i++)
        {
            valuesInPlayWithoutPairs.ForEach(value =>
            {
                // If we can find a card to pair this value, add it.
                if (_deck.cards.Any(card => card.Value == value))
                {
                    valuesInPlayWithoutPairs.Remove(value);
                    valuesWithPairs.Add(value);
                }
            });
        }

        // Now we have found some more potential pairs. So we can simply check if valuesWithPairs has at least two pairs.
        if (valuesWithPairs.Count() >= 2)
            return true;

        return false;
    }
}
