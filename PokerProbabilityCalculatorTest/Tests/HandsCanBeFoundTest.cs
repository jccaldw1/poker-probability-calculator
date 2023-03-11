using PokerProbabilityCalculator.Model;
using PokerProbabilityCalculator.Service;

namespace PokerProbabilityCalculatorTest.Tests
{
    public class HandsCanBeFoundTest : Test
    {
        private MadeHandService _madeHandService;

        public bool RunAllTests()
        {
            return
                TestRoyalFlushCanBeFound() &&
                TestStraightFlushCanBeFound() &&
                TestFourOfAKindCanBeFound() &&
                TestFullHouseCanBeFound() &&
                TestFullHouseCanBeFoundWithTwoThreesOfAKind() &&
                TestFlushCanBeFound() &&
                TestBoardFlush() &&
                TestBoardFlushWithBetterFlushInHand() &&
                TestStraightCanBeFound() &&
                TestSixCardStraightCanBeFound() &&
                TestSevenCardStraightCanBeFound() &&
                TestThreeOfAKindCanBeFound() &&
                TestTwoPairCanBeFound() &&
                TestOnePairCanBeFound() &&
                TestHighCardCanBeFound();
        }

        public bool TestRoyalFlushCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.A), new Card(Suit.Club, Value.Num2));

            Board board = new();
            board.Flop1 = new Card(Suit.Diamond, Value.K);
            board.Flop2 = new Card(Suit.Diamond, Value.Q);
            board.Flop3 = new Card(Suit.Diamond, Value.Num10);
            board.Turn = new Card(Suit.Diamond, Value.J);
            board.River = new Card(Suit.Diamond, Value.Num9);

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.RoyalFlush;
        }
        public bool TestStraightFlushCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.Num9), new Card(Suit.Club, Value.Num2));

            Board board = new();
            board.Flop1 = new Card(Suit.Diamond, Value.K);
            board.Flop2 = new Card(Suit.Diamond, Value.Q);
            board.Flop3 = new Card(Suit.Diamond, Value.Num10);
            board.Turn = new Card(Suit.Diamond, Value.J);
            board.River = new Card(Suit.Spade, Value.Num8);

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.StraightFlush && madeHand.Value == Value.K;
        }

        public bool TestFourOfAKindCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.Num4), new Card(Suit.Heart, Value.Num4));

            Board board = new()
            {
                Flop1 = new(Suit.Club, Value.Num4),
                Flop2 = new(Suit.Spade, Value.Num4),
                Flop3 = new(Suit.Diamond, Value.J),
                Turn = new(Suit.Heart, Value.Num2),
                River = new(Suit.Spade, Value.Num7)
            };

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.FourOfAKind && madeHand.Value == Value.Num4;
        }

        public bool TestFullHouseCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.Num4), new Card(Suit.Heart, Value.Num4));

            Board board = new()
            {
                Flop1 = new(Suit.Diamond, Value.Num7),
                Flop2 = new(Suit.Club, Value.Num7),
                Flop3 = new(Suit.Diamond, Value.J),
                Turn = new(Suit.Heart, Value.Num2),
                River = new(Suit.Spade, Value.Num7)
            };

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.FullHouse && madeHand.Value == Value.Num7 && madeHand.Value2 == Value.Num4;
        }

        public bool TestFullHouseCanBeFoundWithTwoThreesOfAKind()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.Num4), new Card(Suit.Heart, Value.Num4));

            Board board = new()
            {
                Flop1 = new(Suit.Club, Value.Num4),
                Flop2 = new(Suit.Spade, Value.Num7),
                Flop3 = new(Suit.Diamond, Value.J),
                Turn = new(Suit.Diamond, Value.Num7),
                River = new(Suit.Spade, Value.Num7)
            };

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.FullHouse && madeHand.Value == Value.Num7 && madeHand.Value2 == Value.Num4;
        }

        public bool TestFlushCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.Num4), new Card(Suit.Diamond, Value.Num5));

            Board board = new()
            {
                Flop1 = new(Suit.Diamond, Value.J),
                Flop2 = new(Suit.Spade, Value.Num7),
                Flop3 = new(Suit.Diamond, Value.Q),
                Turn = new(Suit.Diamond, Value.Num7),
                River = new(Suit.Spade, Value.Num7)
            };

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.Flush && madeHand.Value == Value.Q && madeHand.Value2 == Value.J && madeHand.Value3 == Value.Num7 && madeHand.Value4 == Value.Num5 && madeHand.Value5 == Value.Num4;
        }

        public bool TestBoardFlush()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.Num4), new Card(Suit.Diamond, Value.Num5));

            Board board = new()
            {
                Flop1 = new(Suit.Diamond, Value.J),
                Flop2 = new(Suit.Diamond, Value.Num7),
                Flop3 = new(Suit.Diamond, Value.Q),
                Turn = new(Suit.Diamond, Value.Num10),
                River = new(Suit.Diamond, Value.K)
            };

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.Flush && madeHand.Value == Value.K && madeHand.Value2 == Value.Q && madeHand.Value3 == Value.J && madeHand.Value4 == Value.Num10 && madeHand.Value5 == Value.Num7;
        }

        public bool TestBoardFlushWithBetterFlushInHand()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.Num9), new Card(Suit.Diamond, Value.Num5));

            Board board = new()
            {
                Flop1 = new(Suit.Diamond, Value.J),
                Flop2 = new(Suit.Diamond, Value.Num7),
                Flop3 = new(Suit.Diamond, Value.Q),
                Turn = new(Suit.Diamond, Value.Num10),
                River = new(Suit.Diamond, Value.Num2)
            };

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.Flush && madeHand.Value == Value.Q && madeHand.Value2 == Value.J && madeHand.Value3 == Value.Num10 && madeHand.Value4 == Value.Num9 && madeHand.Value5 == Value.Num7;
        }

        public bool TestStraightCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.Num5), new Card(Suit.Diamond, Value.Num7));

            Board board = new()
            {
                Flop1 = new(Suit.Diamond, Value.Num4),
                Flop2 = new(Suit.Spade, Value.Num6),
                Flop3 = new(Suit.Club, Value.Num8),
                Turn = new(Suit.Heart, Value.A),
                River = new(Suit.Diamond, Value.K)
            };

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.Straight && madeHand.Value == Value.Num8;
        }

        public bool TestSixCardStraightCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.Num5), new Card(Suit.Diamond, Value.Num7));

            Board board = new()
            {
                Flop1 = new(Suit.Diamond, Value.Num4),
                Flop2 = new(Suit.Spade, Value.Num6),
                Flop3 = new(Suit.Club, Value.Num8),
                Turn = new(Suit.Heart, Value.Num3),
                River = new(Suit.Diamond, Value.K)
            };

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.Straight && madeHand.Value == Value.Num8;
        }

        public bool TestSevenCardStraightCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.Num5), new Card(Suit.Diamond, Value.Num7));

            Board board = new()
            {
                Flop1 = new(Suit.Diamond, Value.Num4),
                Flop2 = new(Suit.Spade, Value.Num6),
                Flop3 = new(Suit.Club, Value.Num8),
                Turn = new(Suit.Heart, Value.Num3),
                River = new(Suit.Diamond, Value.Num9)
            };

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.Straight && madeHand.Value == Value.Num9;
        }

        public bool TestThreeOfAKindCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.Num5), new Card(Suit.Club, Value.Num5));

            Board board = new()
            {
                Flop1 = new(Suit.Spade, Value.Num5),
                Flop2 = new(Suit.Spade, Value.Num6),
                Flop3 = new(Suit.Club, Value.Num8),
                Turn = new(Suit.Heart, Value.Num3),
                River = new(Suit.Diamond, Value.Num9)
            };

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.ThreeOfAKind && madeHand.Value == Value.Num5 && madeHand.Value2 == Value.Num9 && madeHand.Value3 == Value.Num8;
        }

        public bool TestTwoPairCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.Num5), new Card(Suit.Club, Value.Num5));

            Board board = new()
            {
                Flop1 = new(Suit.Club, Value.Num6),
                Flop2 = new(Suit.Spade, Value.Num6),
                Flop3 = new(Suit.Club, Value.Num8),
                Turn = new(Suit.Heart, Value.Num3),
                River = new(Suit.Diamond, Value.Num9)
            };

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.TwoPair && madeHand.Value == Value.Num6 && madeHand.Value2 == Value.Num5 && madeHand.Value3 == Value.Num9;
        }

        public bool TestOnePairCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.Num5), new Card(Suit.Club, Value.Num5));

            Board board = new()
            {
                Flop1 = new(Suit.Club, Value.Num6),
                Flop2 = new(Suit.Spade, Value.J),
                Flop3 = new(Suit.Club, Value.Num8),
                Turn = new(Suit.Heart, Value.Num3),
                River = new(Suit.Diamond, Value.Num9)
            };

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.OnePair && madeHand.Value == Value.Num5 && madeHand.Value2 == Value.J && madeHand.Value3 == Value.Num9 && madeHand.Value4 == Value.Num8;
        }

        public bool TestHighCardCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.A), new Card(Suit.Diamond, Value.K));

            Board board = new()
            {
                Flop1 = new(Suit.Diamond, Value.Num2),
                Flop2 = new(Suit.Spade, Value.Num3),
                Flop3 = new(Suit.Club, Value.Num4),
                Turn = new(Suit.Heart, Value.Num6),
                River = new(Suit.Diamond, Value.Num7)
            };

            var madeHand = DetermineMadeHand(hand, board);

            return madeHand.Hand == Hand.HighCard && madeHand.Value == Value.A && madeHand.Value2 == Value.K && madeHand.Value3 == Value.Num7 && madeHand.Value4 == Value.Num6 && madeHand.Value5 == Value.Num4;
        }

        private MadeHand DetermineMadeHand(PlayerHand hand, Board board)
        {
            _madeHandService = new MadeHandService(hand, board);

            var madeHand = _madeHandService.DetermineMadeHand();

            Console.WriteLine(madeHand);

            return madeHand;
        }


    }
}
