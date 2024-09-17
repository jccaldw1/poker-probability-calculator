using PokerProbabilityCalculator.Model;

namespace PokerProbabilityCalculator.Service;

/// <summary>
/// Given a player hand and a board, determines whether a straight flush or straight can be made.
/// </summary>
/// <remarks>
/// This service does not take into account the whole deck - it only determines whether there is a straight possible given the cards in play and the number of cards left to be played.
/// </remarks>
public static class StraightPossibilityService
{
    private static readonly Value[] orderedStraightyValues = {
        Value.A, Value.Num2, Value.Num3, Value.Num4, Value.Num5, Value.Num6, Value.Num7, Value.Num8, Value.Num9, Value.Num10, Value.J, Value.Q, Value.K, Value.A
    };

    public static List<Card>? CanMakeStraight(PlayerHand hand, Board board, Deck deck)
    {
        int numberOfCardsPlayed = board.NumberOfCardsPlayed();

        List<Card> cardsInPlay = new()
        {
            hand.Card1,
            hand.Card2
        };

        board.GetCards().ForEach(cardsInPlay.Add);

        return FindPossibleStraights(cardsInPlay, numberOfCardsPlayed, deck);
    }

    public static List<Card>? CanMakeStraightFlush(PlayerHand hand, Board board, Suit suit, Deck deck)
    {
        List<Card> cardsInPlay = new()
        {
            hand.Card1,
            hand.Card2
        };

        board.GetCards().ForEach(cardsInPlay.Add);

        List<Card> cardsOfSameSuitInPlay = cardsInPlay.Where(card => card.Suit == suit).ToList();

        List<Card> cardsThatCanMakeStraightFlush = new();

        // In short, if a flush is possible, look for potential straight flushes.
        if (cardsOfSameSuitInPlay.Count >= board.NumberOfCardsPlayed())
        {
            for (int i = 0; i < orderedStraightyValues.Length - 4; i++)
            {
                // We need a certain number of cards within the straight range.
                List<Value> possibleStraightyValues = new()
                {
                    orderedStraightyValues[i],
                    orderedStraightyValues[i + 1],
                    orderedStraightyValues[i + 2],
                    orderedStraightyValues[i + 3],
                    orderedStraightyValues[i + 4]
                };

                bool thisStraightIsPossible = true;
                
                foreach (Card card in cardsOfSameSuitInPlay)
                {
                    if (!possibleStraightyValues.Contains(card.Value))
                    {
                        thisStraightIsPossible = false;
                        break;
                    }
                }

                if (!thisStraightIsPossible) continue;
                
                // Find what values are not in play and look for them in the deck.
                foreach (Card card in cardsOfSameSuitInPlay)
                {
                    possibleStraightyValues.Remove(card.Value);
                }

                List<Card> cardsToLookFor = new();
                
                foreach (Value value in possibleStraightyValues)
                {
                    cardsToLookFor.Add(new Card(suit, value));
                }

                bool cardsToLookForAreInDeck = true;

                foreach (Card card in cardsToLookFor)
                {
                    if(!deck.cards.Contains(card))
                        cardsToLookForAreInDeck = false;
                }

                if (cardsToLookForAreInDeck)
                {
                    cardsThatCanMakeStraightFlush.AddRange(cardsToLookFor);
                }
            }
        }
        else
        {
            return null;
        }

        if (cardsThatCanMakeStraightFlush.Count > 0) return cardsThatCanMakeStraightFlush;

        return null;
    }

    private static List<Card>? FindPossibleStraights(List<Card> cardsInPlay, int numberOfCardsThatNeedToContributeToStraight, Deck deck)
    {
        List<Card>? cardsThatCouldMakeStraight = null;

        for(int i = 0; i < orderedStraightyValues.Length - 4; i++)
        {
            // We need a certain number of cards within the straight range.
            List<Value> possibleStraightyValues = new()
            {
                orderedStraightyValues[i],
                orderedStraightyValues[i + 1],
                orderedStraightyValues[i + 2],
                orderedStraightyValues[i + 3],
                orderedStraightyValues[i + 4]
            };

            if(cardsInPlay.Count(card => possibleStraightyValues.Contains(card.Value)) >= numberOfCardsThatNeedToContributeToStraight)
            {
                bool thisStraightExistsInDeck = true;

                foreach(Value value in possibleStraightyValues)
                    if(!deck.cards.Any(card => card.Value == value))
                        thisStraightExistsInDeck = false;

                if (thisStraightExistsInDeck)
                {
                    cardsThatCouldMakeStraight ??= new();

                    // Just pick a card in the deck that has the value. We know the deck contains these values.
                    foreach(Value value in possibleStraightyValues)
                    {
                        if(cardsThatCouldMakeStraight.Any(card => card.Value == value))
                            continue;

                        cardsThatCouldMakeStraight.Add(deck.cards.First(card => card.Value == value));
                    }
                }
            }
        }

        return cardsThatCouldMakeStraight;
    }
}
