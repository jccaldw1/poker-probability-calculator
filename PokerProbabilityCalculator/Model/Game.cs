namespace PokerProbabilityCalculator.Model; 
public class Game 
{ 
    public List<PlayerHand> LiveHands { get; set; }
    public Board Board { get; set; }
    public Deck Deck { get; set; }

    public Game(List<PlayerHand> liveHands)
    {
        LiveHands = liveHands;
    }

    public Game(List<PlayerHand> liveHands, Board board)
    {
        LiveHands = liveHands;
        Board = board;

        Deck deck = new();
        List<Card> liveCards = new();

        liveCards.AddRange(board.GetCards());

        foreach(PlayerHand hand in LiveHands)
        {
            liveCards.Add(hand.Card1);
            liveCards.Add(hand.Card2);
        }

        bool cardsCouldBeRemoved = true;
        foreach(Card card in liveCards)
        {
            cardsCouldBeRemoved &= deck.RemoveCard(card);
        }

        if (!cardsCouldBeRemoved)
        {
            throw new Exception("One or more cards is used twice.");
        }
    }
}
