namespace PokerProbabilityCalculator.Model;

public record class Card(Suit Suit, Value Value)
{
    public override string ToString()
    {
        return Value.ToString().Replace("Num", "") + " of " + Suit + "s";
    }
}
