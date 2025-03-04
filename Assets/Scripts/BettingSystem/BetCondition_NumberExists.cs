using UnityEngine;

[CreateAssetMenu(fileName = "New Bet Number Exists Condition", menuName = "Roulette/Number Exists Bet Condition")]
public class BetCondition_NumberExists : BetCondition
{
    public override bool Evaluate(int winningNumber, int[] numbers)
    {
        return System.Array.Exists(numbers, n => n == winningNumber);
    }
}