namespace PokerProbabilityCalculator.Model; 
public class Game 
{ 
    public List<PlayerHand> LiveHands { get; set; }
    public Board Board { get; set; }
    public Deck Deck { get; set; }
}
