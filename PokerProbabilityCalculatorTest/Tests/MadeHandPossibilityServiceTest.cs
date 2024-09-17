using PokerProbabilityCalculator.Model;
using PokerProbabilityCalculator.Service;

namespace PokerProbabilityCalculatorTest.Tests;
public class MadeHandPossibilityServiceTest
{
    #region Royal Flush Tests
    public bool RoyalFlushHappyPathTest()
    {
        PlayerHand handToMakeRoyalFlush = new(new(Suit.Spade, Value.A), new(Suit.Spade, Value.K));

        List<PlayerHand> hands = new()
        {
            handToMakeRoyalFlush
        };

        Board royalFlushBoard = new();
        royalFlushBoard.Flop1 = new(Suit.Spade, Value.Q);
        royalFlushBoard.Flop2 = new(Suit.Diamond, Value.Q);
        royalFlushBoard.Flop3 = new(Suit.Heart, Value.Q);

        MadeHandPossibilityService madeHandPossibilityService = new(new() { handToMakeRoyalFlush}, royalFlushBoard);

        return madeHandPossibilityService.NecessaryRoyalFlushCards(handToMakeRoyalFlush) != null;
    }

    public bool NoRoyalFlushesPossibleTest()
    {
        // This collection of hands makes it impossible for any one to make a royal flush.
        PlayerHand hand1 = new(new(Suit.Spade, Value.A), new(Suit.Diamond, Value.A));
        PlayerHand hand2 = new(new(Suit.Club, Value.A), new(Suit.Heart, Value.A));
        PlayerHand hand3 = new(new(Suit.Spade, Value.K), new(Suit.Diamond, Value.K));
        PlayerHand hand4 = new(new(Suit.Club, Value.K), new(Suit.Heart, Value.K));

        MadeHandPossibilityService madeHandPossibilityService = new(new() { hand1, hand2, hand3, hand4 }, new());

        bool result1 = madeHandPossibilityService.NecessaryRoyalFlushCards(hand1) != null;
        bool result2 = madeHandPossibilityService.NecessaryRoyalFlushCards(hand2) != null;

        // expect both false
        return !(result1 && result2);
    }

    #endregion

    #region Straight Flush Tests
    public bool StraightFlushHappyPathTest()
    {
        PlayerHand handToMakeStraightFlush = new(new(Suit.Spade, Value.Num7), new(Suit.Spade, Value.Num6));

        Board straightFlushBoard = new();
        straightFlushBoard.Flop1 = new(Suit.Spade, Value.Num5);
        straightFlushBoard.Flop2 = new(Suit.Diamond, Value.Num5);
        straightFlushBoard.Flop3 = new(Suit.Heart, Value.Num5);

        MadeHandPossibilityService madeHandPossibilityService = new(new() { handToMakeStraightFlush }, straightFlushBoard);

        return madeHandPossibilityService.NecessaryStraightFlushCards(handToMakeStraightFlush) != null;
    }

    public bool StraightFlushRemovedTest()
    {
        PlayerHand handToMakeStraightFlush = new(new(Suit.Spade, Value.Num7), new(Suit.Spade, Value.Num6));
        PlayerHand handToBlock = new(new(Suit.Spade, Value.Num4), new(Suit.Spade, Value.Num8));

        Board straightFlushBoard = new();
        straightFlushBoard.Flop1 = new(Suit.Spade, Value.Num5);
        straightFlushBoard.Flop2 = new(Suit.Diamond, Value.Num5);
        straightFlushBoard.Flop3 = new(Suit.Heart, Value.Num5);

        MadeHandPossibilityService madeHandPossibilityService1 = new(new() { handToMakeStraightFlush, handToBlock }, straightFlushBoard);

        var cards = madeHandPossibilityService1.NecessaryStraightFlushCards(handToMakeStraightFlush);

        bool straightFlushPossible = cards != null;

        return !straightFlushPossible;
    }

    public bool LowAStraightFlushTest()
    {
        PlayerHand handToMake5HighStraightFlush = new(new(Suit.Spade, Value.A), new(Suit.Spade, Value.Num2));

        Board boardToMake5HighStraightFlush = new();
        boardToMake5HighStraightFlush.Flop1 = new(Suit.Spade, Value.Num4);
        boardToMake5HighStraightFlush.Flop2 = new(Suit.Diamond, Value.Num3);
        boardToMake5HighStraightFlush.Flop3 = new(Suit.Heart, Value.Num3);

        MadeHandPossibilityService madeHandPossibilityService = new(new() { handToMake5HighStraightFlush }, boardToMake5HighStraightFlush);

        return madeHandPossibilityService.NecessaryStraightFlushCards(handToMake5HighStraightFlush) != null;
    }

    public bool KingHighStraightFlushTest()
    {
        PlayerHand handToMakeKingHighStraightFlush = new(new(Suit.Spade, Value.K), new(Suit.Spade, Value.Q));

        Board boardToMake5HighStraightFlush = new();
        boardToMake5HighStraightFlush.Flop1 = new(Suit.Spade, Value.Num10);
        boardToMake5HighStraightFlush.Flop2 = new(Suit.Diamond, Value.Num3);
        boardToMake5HighStraightFlush.Flop3 = new(Suit.Heart, Value.Num3);

        MadeHandPossibilityService madeHandPossibilityService = new(new() { handToMakeKingHighStraightFlush }, boardToMake5HighStraightFlush);

        return madeHandPossibilityService.NecessaryStraightFlushCards(handToMakeKingHighStraightFlush) != null;
    }

    #endregion

    #region Four Of A Kind Tests

    public bool FourOfAKindHappyPathTest()
    {
        PlayerHand handToMakeFourOfAKind = new(new(Suit.Spade, Value.Num7), new(Suit.Club, Value.Num7));

        Board fourOfAKindBoard = new();
        fourOfAKindBoard.Flop1 = new(Suit.Spade, Value.K);
        fourOfAKindBoard.Flop2 = new(Suit.Spade, Value.A);
        fourOfAKindBoard.Flop3 = new(Suit.Spade, Value.Q);

        MadeHandPossibilityService madeHandPossibilityService = new(new() { handToMakeFourOfAKind }, fourOfAKindBoard);

        bool fourOfAKindFound = madeHandPossibilityService.NecessaryCardsToMakeFourOfAKind(handToMakeFourOfAKind) != null;

        return fourOfAKindFound;
    }

    public bool FourOfAKindRemovedTest()
    {
        PlayerHand handToMakeFourOfAKind = new(new(Suit.Spade, Value.Num7), new(Suit.Club, Value.Num7));
        PlayerHand handToBlockFourOfAKind = new(new(Suit.Diamond, Value.Num7), new(Suit.Club, Value.Num6));

        Board fourOfAKindBoard = new();
        fourOfAKindBoard.Flop1 = new(Suit.Spade, Value.K);
        fourOfAKindBoard.Flop2 = new(Suit.Spade, Value.A);
        fourOfAKindBoard.Flop3 = new(Suit.Spade, Value.Q);

        MadeHandPossibilityService madeHandPossibilityService = new(new() { handToMakeFourOfAKind, handToBlockFourOfAKind }, fourOfAKindBoard);

        bool fourOfAKindFound = madeHandPossibilityService.NecessaryCardsToMakeFourOfAKind(handToMakeFourOfAKind) != null;

        return !fourOfAKindFound;
    }

    #endregion

    #region Full House Tests
    public bool FullHouseHappyPathTest()
    {
        PlayerHand handToMakeFullHouse = new(new(Suit.Spade, Value.A), new(Suit.Spade, Value.K));

        Board boardToMakeFullHouse = new();
        boardToMakeFullHouse.Flop1 = new(Suit.Diamond, Value.A);
        boardToMakeFullHouse.Flop2 = new(Suit.Club, Value.Num2);
        boardToMakeFullHouse.Flop3 = new(Suit.Club, Value.A);

        MadeHandPossibilityService madeHandPossibilityService = new(new() { handToMakeFullHouse }, boardToMakeFullHouse);

        return madeHandPossibilityService.NecessaryCardsToMakeFullHouse(handToMakeFullHouse) != null;
    }

    public bool NoFullHouseOnNonPairedBoardTest()
    {
        PlayerHand handToMakeFullHouse = new(new(Suit.Spade, Value.A), new(Suit.Spade, Value.K));
        PlayerHand handToBlock = new(new(Suit.Heart, Value.A), new(Suit.Heart, Value.K));
        PlayerHand handToBlock2 = new(new(Suit.Club, Value.A), new(Suit.Diamond, Value.K));
        PlayerHand handToBlock3 = new(new(Suit.Spade, Value.Num2), new(Suit.Diamond, Value.Num2));

        Board boardToMakeFullHouse = new();
        boardToMakeFullHouse.Flop1 = new(Suit.Diamond, Value.A);
        boardToMakeFullHouse.Flop2 = new(Suit.Club, Value.Num2);
        boardToMakeFullHouse.Flop3 = new(Suit.Club, Value.K);

        MadeHandPossibilityService madeHandPossibilityService = new(new() { handToMakeFullHouse, handToBlock, handToBlock2 }, boardToMakeFullHouse);

        return madeHandPossibilityService.NecessaryCardsToMakeFullHouse(handToMakeFullHouse) != null;
    }

    #endregion

    #region Flush Tests
    public bool FlushHappyPathTest()
    {
        PlayerHand handToMakeFlush = new(new(Suit.Spade, Value.A), new(Suit.Spade, Value.Num7));

        Board boardToMakeFlush = new();
        boardToMakeFlush.Flop1 = new(Suit.Spade, Value.Num6);
        boardToMakeFlush.Flop2 = new(Suit.Diamond, Value.Num6);
        boardToMakeFlush.Flop3 = new(Suit.Heart, Value.Num6);

        MadeHandPossibilityService madeHandPossibilityService = new(new() { handToMakeFlush }, boardToMakeFlush);

        return madeHandPossibilityService.NecessaryCardsToMakeFlush(handToMakeFlush) != null;
    }

    public bool FlushNotPossibleTest()
    {
        PlayerHand handToNotMakeFlush = new(new(Suit.Spade, Value.A), new(Suit.Spade, Value.K));

        Board boardToNotMakeFlush = new();
        boardToNotMakeFlush.Flop1 = new(Suit.Diamond, Value.Num7);
        boardToNotMakeFlush.Flop2 = new(Suit.Heart, Value.Num6);
        boardToNotMakeFlush.Flop3 = new(Suit.Diamond, Value.Num5);

        MadeHandPossibilityService madeHandPossibilityService = new(new() { handToNotMakeFlush }, boardToNotMakeFlush);

        // Cards to make flush must be empty; i.e. a flush is not possible.
        return madeHandPossibilityService.NecessaryCardsToMakeFlush(handToNotMakeFlush) == null;
    }
    #endregion

    #region Straight Tests ;)

    public bool StraightHappyPathTest()
    {
        PlayerHand handToMakeStraight = new(new(Suit.Spade, Value.Num3), new(Suit.Diamond, Value.Num5));

        Board boardToMakeStraight = new();
        boardToMakeStraight.PlayCardOnBoard(new(Suit.Heart, Value.Num7));
        boardToMakeStraight.PlayCardOnBoard(new(Suit.Heart, Value.A));
        boardToMakeStraight.PlayCardOnBoard(new(Suit.Heart, Value.K));

        MadeHandPossibilityService madeHandPossibilityService = new(new() { handToMakeStraight }, boardToMakeStraight);

        return madeHandPossibilityService.NecessaryCardsToMakeStraight(handToMakeStraight) != null;
    }

    public bool StraightBlockedTest()
    {
        PlayerHand handToMakeStraight = new(new(Suit.Spade, Value.Num3), new(Suit.Diamond, Value.Num5));
        PlayerHand handToBlockStraight = new(new(Suit.Heart, Value.Num4), new(Suit.Diamond, Value.Num4));
        PlayerHand handToBlockStraight2 = new(new(Suit.Club, Value.Num4), new(Suit.Spade, Value.Num4));

        Board boardToMakeStraight = new();
        boardToMakeStraight.PlayCardOnBoard(new(Suit.Heart, Value.Num7));
        boardToMakeStraight.PlayCardOnBoard(new(Suit.Heart, Value.J));
        boardToMakeStraight.PlayCardOnBoard(new(Suit.Heart, Value.K));

        MadeHandPossibilityService madeHandPossibilityService = new(new() { handToMakeStraight, handToBlockStraight, handToBlockStraight2 }, boardToMakeStraight);
        
        return madeHandPossibilityService.NecessaryCardsToMakeStraight(handToMakeStraight) == null;
    }

    #endregion

    #region Three of a Kind Tests

    public bool ThreeOfAKindHappyPathTest()
    {
        PlayerHand hand = new(new(Suit.Club, Value.Num2), new(Suit.Spade, Value.Num2));
    }

    public bool ThreeOfAKindImpossibleTest()
    {
        
    }
    
    #endregion
}
