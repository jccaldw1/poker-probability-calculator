namespace PokerProbabilityCalculator.Model;

public class Board
{
    public Card? Flop1 { get; set; } = null;
    public Card? Flop2 { get; set; } = null;
    public Card? Flop3 { get; set; } = null;
    public Card? Turn { get; set; } = null;
    public Card? River { get; set; } = null;

    public List<Card> GetCards()
    {
        List<Card> cardsOnBoard = new List<Card>();

        if(Flop1 != null)
        {
            cardsOnBoard.Add(Flop1);
        }

        if(Flop2 != null)
        {
            cardsOnBoard.Add(Flop2);
        }

        if(Flop3 != null)
        {
            cardsOnBoard.Add(Flop3);
        }

        if(Turn != null)
        {
            cardsOnBoard.Add(Turn);
        }

        if(River != null) 
        {
            cardsOnBoard.Add(River);
        }

        return cardsOnBoard;
    }

    public int NumberOfCardsPlayed()
    {
        int numberOfCards = 0;

        if(Flop1 != null)
        {
            numberOfCards++;
        }

        if(Flop2 != null)
        {
            numberOfCards++;
        }

        if(Flop3 != null)
        {
            numberOfCards++;
        }

        if(Turn != null)
        {
            numberOfCards++;
        }

        if(River != null) 
        {
            numberOfCards++;
        }

        return numberOfCards;
    }

    // Add a card to the board, wherever it is possible for it to be played.
    // TODO: If a card cannot be played, this method silently fails.
    public Board PlayCardOnBoard(Card cardToPlay)
    {
        if(Flop1 == null)
        {
            Flop1 = cardToPlay;
        }
        else if(Flop2 == null)
        {
            Flop2 = cardToPlay;
        }
        else if(Flop3 == null)
        {
            Flop3 = cardToPlay;
        }
        else if (Turn == null)
        {
            Turn = cardToPlay;
        }
        else if (River == null)
        {
            River = cardToPlay;
        }
        else
        {
            throw new Exception("Cannot play cards on a full board");
        }

        return this;
    }
}
