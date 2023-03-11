// See https://aka.ms/new-console-template for more information
using PokerProbabilityCalculatorTest.Tests;

Console.WriteLine("Hello, World!");

HeadsUpTests headsUpTests = new();

HandsCanBeFoundTest handsCanBeFoundTest = new();

bool testResult = handsCanBeFoundTest.RunAllTests();

Console.WriteLine(testResult);
