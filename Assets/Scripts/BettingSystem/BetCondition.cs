using UnityEngine;

[CreateAssetMenu(fileName = "New Bet Condition", menuName = "Roulette/Bet Condition")]
public class BetCondition : ScriptableObject, IBetCondition
{
    public BetType BetType;

    public bool Evaluate(int winningNumber)
    {
        switch (BetType)
        {
            case BetType.Red: return IsRed(winningNumber);
            case BetType.Black: return !IsRed(winningNumber);
            case BetType.Even: return winningNumber % 2 == 0;
            case BetType.Odd: return winningNumber % 2 != 0;
            case BetType.High: return winningNumber >= 19;
            case BetType.Low: return winningNumber <= 18;
            case BetType.FirstDozen: return winningNumber >= 1 && winningNumber <= 12;
            case BetType.SecondDozen: return winningNumber >= 13 && winningNumber <= 24;
            case BetType.ThirdDozen: return winningNumber >= 25 && winningNumber <= 36;
            case BetType.FirstColumn: return (winningNumber - 1) % 3 == 0;
            case BetType.SecondColumn: return (winningNumber - 2) % 3 == 0;
            case BetType.ThirdColumn: return (winningNumber - 3) % 3 == 0;
            default: return false;
        }
    }

    private bool IsRed(int number)
    {
        int[] redNumbers = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
        return System.Array.Exists(redNumbers, n => n == number);
    }
}