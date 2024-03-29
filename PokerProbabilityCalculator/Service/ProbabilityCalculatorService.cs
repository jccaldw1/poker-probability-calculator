﻿using PokerProbabilityCalculator.Model;

namespace PokerProbabilityCalculator.Service;

/// <summary>
/// Given a list of hands and an optional (partial) board, determines the frequency with which each hand will win given the board's runout.
/// </summary>
public class ProbabilityCalculatorService
{
    private MadeHandService madeHandService;

    private List<PlayerHand> playerHands;

    private int[] handWinningQuantities;
    private int ties;
    private List<Board> tiedBoards = new();

    public ProbabilityCalculatorService(List<PlayerHand> playerHands)
    {
        this.playerHands = playerHands;

        // Set up winning hand frequencies.
        List<KeyValuePair<PlayerHand, int>> handWinningFrequencies = new();

        foreach(PlayerHand hand in this.playerHands)
        {
            handWinningFrequencies.Add(new KeyValuePair<PlayerHand, int>(hand, 0));
        }

        handWinningQuantities = new int[playerHands.Count];

        ties = 0;

        madeHandService = new MadeHandService();
    }

    public List<KeyValuePair<PlayerHand, int>> CalculateWinningProbabilities(List<PlayerHand> liveHands, Board currentBoard)
    {
        // For each remaining card in the deck, examine each live hand to see which hand that card helps. Keep a running count for each live hand of numbers of cards that favor them.


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
            List<int> indicesOfWinningHands = new();

            foreach(PlayerHand hand in liveHands)
            {
                if (winningHands.Contains(hand))
                {
                    indicesOfWinningHands.Add(liveHands.IndexOf(hand));
                }
            }

            if(indicesOfWinningHands.Count > 1)
            {
                ties += 1;
                tiedBoards.Add(currentBoard);
            }

            foreach (int index in indicesOfWinningHands)
            {
                handWinningQuantities[index] += 1;
            }

            foreach(Card card in currentBoard.GetCards())
            {
                Console.Write(card.ToString() + "; ");
            }

            Console.Write("Winner: " + winningHands[0].Card1.ToString() + "; " + winningHands[0].Card2.ToString() + "ties: " + ties + "\n");
        }
        // Recursive step
        else
        {
            // Get all the cards that remain in the deck given the cards that have been played.
            Deck deck = constructDeckGivenPlayedCards(liveHands, currentBoard);

            foreach(Card card in deck.cards)
            {
                //Console.WriteLine("Card to add: " + card);
                Board newBoard = new Board();

                newBoard.Flop1 = currentBoard.Flop1;
                newBoard.Flop2 = currentBoard.Flop2;
                newBoard.Flop3 = currentBoard.Flop3;
                newBoard.Turn = currentBoard.Turn;
                newBoard.River = currentBoard.River;

                List<Card> liveCards = currentBoard.GetCards();
                foreach(PlayerHand hand in liveHands)
                {
                    liveCards.Add(hand.Card1);
                    liveCards.Add(hand.Card2);
                }

                if (liveCards.Contains(card))
                {
                    throw new Exception("Hey, you're adding a card that's already in play.");
                }

                newBoard.PlayCardOnBoard(card);


                CalculateWinningProbabilities(liveHands, newBoard);
            }
        }

        List<KeyValuePair<PlayerHand, int>> handWinningFrequencies = new();

        for(int i = 0; i < handWinningQuantities.Length; i++)
        {
            handWinningFrequencies.Add(new KeyValuePair<PlayerHand, int>(liveHands.ElementAt(i), handWinningQuantities[i]));
        }

        return handWinningFrequencies;
    }

    private Deck constructDeckGivenPlayedCards(List<PlayerHand> liveHands, Board currentBoard)
    {
        Deck deck = new();
        List<Card> liveCards = new();

        foreach(Card card in currentBoard.GetCards())
        {
            liveCards.Add(card);
        }

        foreach (PlayerHand hand in liveHands)
        {
            liveCards.Add(hand.Card1);
            liveCards.Add(hand.Card2);
        }

        foreach(Card card in liveCards)
        {
            deck.RemoveCard(card);
        }

        return deck;
    }
}
