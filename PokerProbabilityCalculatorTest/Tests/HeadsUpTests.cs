using PokerProbabilityCalculator.Model;
using PokerProbabilityCalculator.Service;

namespace PokerProbabilityCalculatorTest.Tests;

/// <summary>
/// Tests for poker hands consisting of two players (that is, poker hands "heads up").
/// </summary>
public class HeadsUpTests : Test
{
    private ProbabilityCalculatorService _probabilityCalculatorService;

    private MadeHandService _madeHandService;

    public bool RunAllTests()
    {
        return true;
    }

    private void TestAnyTwoHands(PlayerHand hand1, PlayerHand hand2, Board board)
    {
        List<PlayerHand> liveHands = new List<PlayerHand> { hand1, hand2 };

        _probabilityCalculatorService = new ProbabilityCalculatorService(liveHands);

        var handWinningFrequencies = _probabilityCalculatorService.CalculateWinningProbabilities(liveHands, board);

        foreach(var frequency in handWinningFrequencies)
        {
            Console.WriteLine(frequency);
        }
    }

    private void TestAnyTwoHands(PlayerHand hand1, PlayerHand hand2)
    {
        TestAnyTwoHands(hand1, hand2, new Board());
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

        TestAnyTwoHands(kings, queens);
    }

    public void TestAcesVsAK()
    {
        PlayerHand aces = new(
            new Card(Suit.Diamond, Value.A),
            new Card(Suit.Club, Value.A)
            );

        PlayerHand ak = new(
            new Card(Suit.Spade, Value.A),
            new Card(Suit.Club, Value.K)
            );

        TestAnyTwoHands(aces, ak);
    }

    public void TestAcesVsGarbage()
    {
        PlayerHand aces = new(
            new Card(Suit.Diamond, Value.A),
            new Card(Suit.Club, Value.A)
            );

        PlayerHand garbage = new(
            new Card(Suit.Spade, Value.Num7),
            new Card(Suit.Club, Value.Num2)
            );

        TestAnyTwoHands(aces, garbage);
    }
}
