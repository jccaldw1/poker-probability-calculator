// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using PokerProbabilityCalculatorTest.Tests;

Console.WriteLine("Hello, World!");

MadeHandPossibilityServiceTest madeHandPossibilityServiceTest = new();

Debug.Assert(madeHandPossibilityServiceTest.RoyalFlushHappyPathTest());
Debug.Assert(madeHandPossibilityServiceTest.NoRoyalFlushesPossibleTest());
Debug.Assert(madeHandPossibilityServiceTest.StraightFlushHappyPathTest());
Debug.Assert(madeHandPossibilityServiceTest.StraightFlushRemovedTest());
Debug.Assert(madeHandPossibilityServiceTest.LowAStraightFlushTest());
Debug.Assert(madeHandPossibilityServiceTest.KingHighStraightFlushTest());
Debug.Assert(madeHandPossibilityServiceTest.FourOfAKindHappyPathTest());
Debug.Assert(madeHandPossibilityServiceTest.FourOfAKindRemovedTest());
Debug.Assert(madeHandPossibilityServiceTest.FullHouseHappyPathTest());
Debug.Assert(madeHandPossibilityServiceTest.NoFullHouseOnNonPairedBoardTest());
Debug.Assert(madeHandPossibilityServiceTest.FlushHappyPathTest());
Debug.Assert(madeHandPossibilityServiceTest.FlushNotPossibleTest());
Debug.Assert(madeHandPossibilityServiceTest.StraightHappyPathTest());
Debug.Assert(madeHandPossibilityServiceTest.StraightBlockedTest());