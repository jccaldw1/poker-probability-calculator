using PokerProbabilityCalculator.Model;

namespace PokerProbabilityCalculator.Service;

public class ProbabilityCalculatorService
{
    private MadeHandService madeHandService;

    private List<PlayerHand> playerHands;

    private List<KeyValuePair<PlayerHand, int>> handWinningFrequencies;

    public ProbabilityCalculatorService(List<PlayerHand> playerHands)
    {
        this.playerHands = playerHands;

        // Set up winning hand frequencies.
        List<KeyValuePair<PlayerHand, int>> handWinningFrequencies = new();

        foreach(PlayerHand hand in this.playerHands)
        {
            handWinningFrequencies.Add(new KeyValuePair<PlayerHand, int>(hand, 0));
        }

        this.handWinningFrequencies = handWinningFrequencies;

        madeHandService = new MadeHandService();
    }

    public List<KeyValuePair<PlayerHand, int>> CalculateWinningProbabilities(List<PlayerHand> liveHands, Board currentBoard)
    {
        // For each remaining card in the deck, examine each live hand to see which hand that card helps. Keep a running count for each live hand of numbers of cards that favor them.

        // Get all the cards that remain in the deck given the cards that have been played.
        Deck deck = constructDeckGivenPlayedCards(liveHands, currentBoard);

        // Run out the board if possible. Examine each five card board and determine which live hand wins.
        // Base step
        if(currentBoard.GetCards().Count == 5)
        {
            List<MadeHand> madeHands = new();

            // Keep track of the made hands that the player's hands make.
            List<KeyValuePair<PlayerHand, MadeHand>> playerHandMadeHandAssociation = new();

            foreach(PlayerHand hand in liveHands)
            {
                MadeHand madeHandForThisHand = madeHandService.DetermineMadeHand(hand, currentBoard);

                madeHands.Add(madeHandForThisHand);

                playerHandMadeHandAssociation.Add(new KeyValuePair<PlayerHand, MadeHand>(hand, madeHandForThisHand));
            }

            MadeHand bestMadeHand = madeHandService.DetermineBestMadeHand(madeHands);

            // The player hands that result in the winning hand. Note that if a tie occurs all player hands will be winning.
            List<PlayerHand> winningHands = playerHandMadeHandAssociation.Where(kvp => kvp.Value == bestMadeHand).Select(kvp => kvp.Key).ToList();

            // Add 1 to each of the hand winning frequencies for the winning hand.

            // Get all the hand frequency rows for the winning hand. We need to update these values by 1, so we have to keep track of them.
            var handWinningFrequenciesToUpdate = handWinningFrequencies.Where(kvp => winningHands.Contains(kvp.Key)).ToList();

            // Remove the hand frequency rows so we can update them.
            handWinningFrequencies.RemoveAll(kvp => winningHands.Contains(kvp.Key));

            // Add back the hand frequency row with the extra frequency.
            foreach(var handWinningFrequency in handWinningFrequenciesToUpdate)
            {
                handWinningFrequencies.Add(new KeyValuePair<PlayerHand, int>(handWinningFrequency.Key, handWinningFrequency.Value + 1));
            }
        }
        // Recursive step
        else
        {
            foreach(Card card in deck.cards)
            {
                Board newBoard = currentBoard.PlayCardOnBoard(card);

                CalculateWinningProbabilities(liveHands, newBoard);
            }
        }

        return handWinningFrequencies;
    }

    private Deck constructDeckGivenPlayedCards(List<PlayerHand> liveHands, Board currentBoard)
    {
        Deck deck = new();

        deck.cards.RemoveAll(card => currentBoard.GetCards().Contains(card));

        foreach (PlayerHand hand in liveHands)
        {
            deck.cards.Remove(hand.Card1);
            deck.cards.Remove(hand.Card2);
        }

        return deck;
    }
}
