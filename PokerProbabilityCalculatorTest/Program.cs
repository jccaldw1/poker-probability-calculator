// See https://aka.ms/new-console-template for more information
using PokerProbabilityCalculatorTest.Tests;

Console.WriteLine("Hello, World!");

MadeHandPossibilityServiceTest madeHandPossibilityServiceTest = new();

bool result = madeHandPossibilityServiceTest.StraightBlockedTest();

if (result)
{
    Console.WriteLine("Test Successful!!");
}
else
{
    Console.WriteLine("Test unsuccessful :(((((");
}
