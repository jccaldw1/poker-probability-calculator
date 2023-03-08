using PokerProbabilityCalculator.Model;
using PokerProbabilityCalculator.Service;

namespace PokerProbabilityCalculatorTest.Tests;

/// <summary>
/// Tests for poker hands consisting of two players (that is, poker hands "heads up").
/// </summary>
public class HeadsUpTests
{
    private ProbabilityCalculatorService _probabilityCalculatorService;

    public void RunAllTests()
    {

    }

    public void TestKingsVersusQueens()
    {
        PlayerHand kings = new(
            new Card(Suit.Diamond, Value.K),
            new Card(Suit.Club, Value.K)
            );

        PlayerHand queens = new(
            new Card(Suit.Diamond, Value.Q),
            new Card(Suit.Club, Value.Q)
            );

        List<PlayerHand> liveHands = new List<PlayerHand> { kings, queens };

        _probabilityCalculatorService = new ProbabilityCalculatorService(liveHands);

        var handWinningFrequencies = _probabilityCalculatorService.CalculateWinningProbabilities(liveHands, new Board());
        foreach(var frequency in handWinningFrequencies)
        {
            Console.WriteLine(frequency);
        }
    }
}
