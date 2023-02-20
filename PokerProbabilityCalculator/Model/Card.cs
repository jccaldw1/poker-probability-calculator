using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProbabilityCalculator.Model;

public class Card
{
    public Suit Suit;
    public Value Value;

    public Card(Suit suit, Value value)
    {
        Suit = suit;
        Value = value;
    }

}
