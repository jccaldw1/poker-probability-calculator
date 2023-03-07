namespace PokerProbabilityCalculator.Model;

public class Board
{
    public Flop? Flop { get; set; } = null;
    public Card? Turn { get; set; } = null;
    public Card? River { get; set; } = null;

    public List<Card> GetCards()
    {
        List<Card> cardsOnBoard = new List<Card>();

        if(Flop != null)
        {
            cardsOnBoard.Add(Flop.Card1);
            cardsOnBoard.Add(Flop.Card2);
            cardsOnBoard.Add(Flop.Card3);
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

    // Add a card to the board, wherever it is possible for it to be played.
    // TODO: If a card cannot be played, this method silently fails.
    public Board PlayCardOnBoard(Card cardToPlay)
    {
        if (Flop.Card1 == null)
        {
            Flop.Card1 = cardToPlay;
        }
        else if(Flop.Card2 == null)
        {
            Flop.Card2 = cardToPlay;
        }
        else if (Flop.Card3 == null)
        {
            Flop.Card3 = cardToPlay;
        }
        else if (Turn == null)
        {
            Turn = cardToPlay;
        }
        else if (River == null)
        {
            River = cardToPlay;
        }

        return this;
    }
}
