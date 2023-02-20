using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProbabilityCalculator.Model;
public class Hand
{
    private Card _card1;
    private Card _card2;

    public Hand(Card card1, Card card2)
    {
        _card1 = card1;
        _card2 = card2;
    }
}
