using PokerProbabilityCalculator.Model;
using PokerProbabilityCalculator.Service;

namespace PokerProbabilityCalculatorTest.Tests
{
    public class HandsCanBeFoundTest
    {
        private MadeHandService _madeHandService;

        public void TestRoyalFlushCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.A), new Card(Suit.Club, Value.Num2));

            Board board = new();
            board.Flop1 = new Card(Suit.Diamond, Value.K);
            board.Flop2 = new Card(Suit.Diamond, Value.Q);
            board.Flop3 = new Card(Suit.Diamond, Value.Num10);
            board.Turn = new Card(Suit.Diamond, Value.J);
            board.River = new Card(Suit.Diamond, Value.Num9);

            DetermineMadeHand(hand, board);
        }
        public void TestStraightFlushCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.Num9), new Card(Suit.Club, Value.Num2));

            Board board = new();
            board.Flop1 = new Card(Suit.Diamond, Value.K);
            board.Flop2 = new Card(Suit.Diamond, Value.Q);
            board.Flop3 = new Card(Suit.Diamond, Value.Num10);
            board.Turn = new Card(Suit.Diamond, Value.J);
            board.River = new Card(Suit.Spade, Value.Num8);

            DetermineMadeHand(hand, board);
        }

        public void TestFourOfAKindCanBeFound()
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

            DetermineMadeHand(hand, board);
        }
        public void TestFullHouseCanBeFound()
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

            DetermineMadeHand(hand, board);
        }

        public void TestFullHouseCanBeFoundWithTwoThreesOfAKind()
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

            DetermineMadeHand(hand, board);
        }

        public void TestFlushCanBeFound()
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

            DetermineMadeHand(hand, board);
        }

        public void TestBoardFlush()
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

            DetermineMadeHand(hand, board);
        }

        public void TestBoardFlushWithBetterFlushInHand()
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

            DetermineMadeHand(hand, board);
        }

        public void TestStraightCanBeFound()
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

            DetermineMadeHand(hand, board);
        }

        public void TestSixCardStraightCanBeFound()
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

            DetermineMadeHand(hand, board);
        }

        public void TestSevenCardStraightCanBeFound()
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

            DetermineMadeHand(hand, board);
        }

        public void TestThreeOfAKindCanBeFound()
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

            DetermineMadeHand(hand, board);
        }

        public void TestTwoPairCanBeFound()
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

            DetermineMadeHand(hand, board);
        }

        public void TestOnePairCanBeFound()
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

            DetermineMadeHand(hand, board);
        }

        public void TestHighCardCanBeFound()
        {
            PlayerHand hand = new(new Card(Suit.Diamond, Value.A), new Card(Suit.Diamond, Value.K));

            Board board = new()
            {
                Flop1 = new(Suit.Diamond, Value.Num2),
                Flop2 = new(Suit.Spade, Value.Num3),
                Flop3 = new(Suit.Club, Value.Num4),
                Turn = new(Suit.Heart, Value.Num5),
                River = new(Suit.Diamond, Value.Num7)
            };

            DetermineMadeHand(hand, board);

        }

        private void DetermineMadeHand(PlayerHand hand, Board board)
        {
            _madeHandService = new MadeHandService(hand, board);

            var madeHand = _madeHandService.DetermineMadeHand();
            Console.WriteLine(madeHand);
        }


    }
}
