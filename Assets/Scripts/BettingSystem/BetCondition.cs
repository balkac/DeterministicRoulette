using UnityEngine;

public abstract class BetCondition : ScriptableObject, IBetCondition
{
    public abstract bool Evaluate(int winningNumber, int[] numbers);
}