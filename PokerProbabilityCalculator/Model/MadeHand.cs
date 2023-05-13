using System.Numerics;
using System.Reflection.Metadata;

namespace PokerProbabilityCalculator.Model;

public record class MadeHand(Hand Hand, Value Value, Value? Value2, Value? Value3, Value? Value4, Value? Value5)
{
    /// <summary>
    /// The lowest possible made hand, 7-high.
    /// </summary>
    public static MadeHand LowestPossibleMadeHand { get; } = new MadeHand(Hand.HighCard, Value.Num7, Value.Num5, Value.Num4, Value.Num3, Value.Num2);

    public static int?[] operator -(MadeHand hand1, MadeHand hand2)
    {
        int handDifference = hand1.Hand - hand2.Hand;
        int valueDifference = hand1.Value - hand2.Value;
        int? value2Difference = (hand1.Value2 - hand2.Value2) ?? null;
        int? value3Difference = (hand1.Value3 - hand2.Value3) ?? null;
        int? value4Difference = (hand1.Value4 - hand2.Value4) ?? null;
        int? value5Difference = (hand1.Value5 - hand2.Value5) ?? null;

        return new int?[]
        {
            handDifference, valueDifference, value2Difference, value3Difference, value4Difference, value5Difference
        };

    }

    public static bool operator <(MadeHand left, MadeHand right)
    {
        int?[] handDifferences = left - right;

        for(int i = 0; i < handDifferences.Length; i++)
        {
            // If we discover a negative, left hand is worse.
            if (handDifferences[i] < 0)
                return true;

            // If we discover a positive before seeing a negative, we know left hand is better.
            else if (handDifferences[i] > 0)
                return false;

            // If the difference is exactly zero, we continue looking for differences.
        }

        // If we see all zeros, then the hands are identical.
        return false;
    }

    public static bool operator >(MadeHand left, MadeHand right)
    {
        int?[] handDifferences = left - right;

        for(int i = 0; i < handDifferences.Length; i++)
        {
            // If we discover a negative, we know left hand is worse.
            if (handDifferences[i] < 0)
                return false;

            // If we discover a positive before seeing a negative, we know left hand is better.
            else if (handDifferences[i] > 0)
                return true;

            // If the difference is exactly zero, we continue looking for differences.
        }

        // If we see all zeros, then the hands are identical.
        return false;

    }
};
