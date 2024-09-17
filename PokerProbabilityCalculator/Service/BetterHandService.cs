using PokerProbabilityCalculator.Model;

namespace PokerProbabilityCalculator.Service;

/// <summary>
/// Given a list of hands, determines which hand is currently best and the criteria for the other hands to become better.
/// </summary>
public class BetterHandService
{
    private List<PlayerHand> _playerHands;

    private Board _board;

    private MadeHandService _madeHandService;

    private MadeHand _winningMadeHand;

    private Deck _deck;

    private List<KeyValuePair<PlayerHand, MadeHand>> _madeHandsFromPlayerHands;

    private int _numberOfCardsLeftToBePlayed;

    private static readonly List<Value> BroadwayCards = new() { Value.A, Value.K, Value.Q, Value.J, Value.Num10 };

    public BetterHandService(List<PlayerHand> playerHands, Board board)
    {
        _playerHands = playerHands;
        _madeHandService = new();
        _board = board;
        _numberOfCardsLeftToBePlayed = 5 - board.GetCards().Count;

        _deck = new();
        _deck.RemoveRange(board.GetCards());

        _madeHandsFromPlayerHands = new();

        _playerHands.ForEach(hand =>
        {
            _madeHandsFromPlayerHands.Add(
                new KeyValuePair<PlayerHand, MadeHand>(hand, _madeHandService.DetermineMadeHand(hand, _board))
            );

            _deck.RemoveCard(hand.Card1);
            _deck.RemoveCard(hand.Card2);
        });

        _winningMadeHand = _madeHandService.DetermineBestMadeHand(
            _madeHandsFromPlayerHands.Select(hand => hand.Value).ToList()
        );
    }

    private List<Card> GetCardsThatWouldImproveLowerHand(PlayerHand handToCompare)
    {
        int numberOfCardsThatCouldHelp = 0;

        int numberOfCardsLeftToBePlayed = 5 - _board.GetCards().Count;

        List<Hand> handsThatArePossible = new List<Hand>();

        // Loop through all hands better than or equal to the winning made hand.
        for(int i = (int)Hand.RoyalFlush; i >= (int)_winningMadeHand.Hand; i--)
        {
            if (isHandPossible(handToCompare, (Hand)i, numberOfCardsLeftToBePlayed))
            {
                handsThatArePossible.Add((Hand)i);
            }
        }

        return new();
    }

    /// <summary>
    /// Determines whether it is possible to create the <paramref name="hand"/> from the <paramref name="handToCompare"/> given the <paramref name="numberOfCardsLeftToBePlayed"/>.
    /// </summary>
    private bool isHandPossible(PlayerHand handToCompare, Hand hand, int numberOfCardsLeftToBePlayed)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Given a <paramref name="hand"/>, determines if it is possible to make a royal flush given that <paramref name="hand"/>, the current deck,
    /// and the number of cards left to be played.
    /// </summary>
    public bool isRoyalFlushPossible(PlayerHand hand)
    {
        // If there are n cards left to be played, we need 5-n cards that can contribute to a royal flush.
        int cardsThatNeedToContributeToRoyalFlush = 5 - _numberOfCardsLeftToBePlayed;

        List<Card> totalCardsInPlay = new List<Card>
        {
            hand.Card2,
            hand.Card2
        };
        totalCardsInPlay.AddRange(_board.GetCards());

        var broadwayCardsInPlay = totalCardsInPlay.Where(card => BroadwayCards.Contains(card.Value));

        List<Suit> suitsThatCouldHaveRoyalFlush = new();
        
        // See if a royal flush is possible for each suit given the cards in play.
        foreach(Suit suit in Enum.GetValues(typeof(Suit)))
        {
            var suitedBroadwayCardsInPlay = broadwayCardsInPlay.Where(card => card.Suit == suit);

            if(suitedBroadwayCardsInPlay.Count() >= cardsThatNeedToContributeToRoyalFlush) 
            {
                suitsThatCouldHaveRoyalFlush.Add(suit);
            }
        }

        // Additionally, we need to examine the deck to determine if the cards needed are there.
        foreach(Suit suit in suitsThatCouldHaveRoyalFlush)
        {
            List<Card> royalFlushCardsForThisSuit = new();

            foreach(Value value in BroadwayCards)
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
    
    private int calculateNumberOfCardsThatCouldMadeGivenHand(Hand i, PlayerHand handToCompare)
    {
        throw new NotImplementedException();
    }

}
