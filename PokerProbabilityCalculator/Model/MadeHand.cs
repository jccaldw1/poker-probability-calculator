namespace PokerProbabilityCalculator.Model;

public record class MadeHand(Hand Hand, Value Value, Value? Value2, Value? Value3, Value? Value4, Value? Value5)
{
    /// <summary>
    /// The lowest possible made hand, 7-high.
    /// </summary>
    public static MadeHand LowestPossibleMadeHand { get; } = new MadeHand(Hand.HighCard, Value.Num7, Value.Num5, Value.Num4, Value.Num3, Value.Num2);
};
