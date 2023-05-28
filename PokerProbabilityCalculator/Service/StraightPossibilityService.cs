using PokerProbabilityCalculator.Model;

namespace PokerProbabilityCalculator.Service;

public static class StraightPossibilityService
{
    private static readonly Value[] orderedStraightyValues = {
        Value.A, Value.Num2, Value.Num3, Value.Num4, Value.Num5, Value.Num6, Value.Num7, Value.Num8, Value.Num9, Value.Num10, Value.J, Value.Q, Value.K, Value.A
    };

    public static bool CanMakeStraight(PlayerHand hand, Board board)
    {
        int numberOfCardsPlayed = board.NumberOfCardsPlayed();

        List<Card> cardsInPlay = new()
        {
            hand.Card2,
            hand.Card2
        };

        board.GetCards().ForEach(cardsInPlay.Add);

        for(int i = 0; i < orderedStraightyValues.Length; i++)
            if (cardsInPlay.Any(card => card.Value == orderedStraightyValues[i]))
                if(CheckIfThereIsAStraightPossibleStartingFromThisCard(i, cardsInPlay, numberOfCardsPlayed))
                    return true;

        return false;
    }

    public static bool CanMakeStraightFlush(PlayerHand hand, Board board, Suit suit)
    {
        int numberOfCardsPlayed = board.NumberOfCardsPlayed();

        List<Card> cardsInPlay = new()
        {
            hand.Card2,
            hand.Card2
        };

        board.GetCards().ForEach(cardsInPlay.Add);

        cardsInPlay = cardsInPlay.Where(card => card.Suit == suit).ToList();

        for(int i = 0; i < orderedStraightyValues.Length - 5; i++)
            if(CheckIfThereIsAStraightPossibleStartingFromThisCard(i, cardsInPlay, numberOfCardsPlayed))
                return true;

        return false;
    }

    private static bool CheckIfThereIsAStraightPossibleStartingFromThisCard(int indexOfValue, List<Card> cardsInPlay, int numberOfCardsThatNeedToContributeToStraight)
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
            return true;
        }

        return false;
    }
}
