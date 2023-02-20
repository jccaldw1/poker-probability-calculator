using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProbabilityCalculator.Model;

public class Board
{
    public Flop? Flop { get; set; } = null;
    public Card? Turn { get; set; } = null;
    public Card? River { get; set; } = null; 
}
