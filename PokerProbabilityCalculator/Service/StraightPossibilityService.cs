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

    public static bool CanMakeStraight(PlayerHand hand, Board board, Deck deck)
    {
        int numberOfCardsPlayed = board.NumberOfCardsPlayed();

        List<Card> cardsInPlay = new()
        {
            hand.Card1,
            hand.Card2
        };

        board.GetCards().ForEach(cardsInPlay.Add);

        for(int i = 0; i < orderedStraightyValues.Length - 4; i++)
            if (cardsInPlay.Any(card => card.Value == orderedStraightyValues[i]))
                if(CheckIfThereIsAStraightPossibleStartingFromThisCard(i, cardsInPlay, numberOfCardsPlayed, deck))
                    return true;

        return false;
    }

    public static bool CanMakeStraightFlush(PlayerHand hand, Board board, Suit suit, Deck deck)
    {
        int numberOfCardsPlayed = board.NumberOfCardsPlayed();

        List<Card> cardsInPlay = new()
        {
            hand.Card1,
            hand.Card2
        };

        board.GetCards().ForEach(cardsInPlay.Add);

        cardsInPlay = cardsInPlay.Where(card => card.Suit == suit).ToList();

        for(int i = 0; i < orderedStraightyValues.Length - 4; i++)
            if(CheckIfThereIsAStraightPossibleStartingFromThisCard(i, cardsInPlay, numberOfCardsPlayed, deck))
                return true;

        return false;
    }

    private static bool CheckIfThereIsAStraightPossibleStartingFromThisCard(int indexOfValue, List<Card> cardsInPlay, int numberOfCardsThatNeedToContributeToStraight, Deck deck)
    {
        // We need a certain number of cards within the straight range.
        List<Value> possibleStraightyValues = new()
        {
            orderedStraightyValues[indexOfValue],
            orderedStraightyValues[indexOfValue + 1],
            orderedStraightyValues[indexOfValue + 2],
            orderedStraightyValues[indexOfValue + 3],
            orderedStraightyValues[indexOfValue + 4]
        };

        if(cardsInPlay.Where(card => possibleStraightyValues.Contains(card.Value)).Count() >= numberOfCardsThatNeedToContributeToStraight)
        {
            bool thisStraightExistsInDeck = true;

            foreach(Value value in possibleStraightyValues)

                if(!deck.cards.Any(card => card.Value == value))
                    thisStraightExistsInDeck = false;

            if(thisStraightExistsInDeck)
                return true;
        }

        return false;
    }
}
