using UnityEngine;

public class BetData : ScriptableObject
{
    public BetType BetType;
    public int[] Numbers;
    public float PayoutMultiplier;
    public BetCondition BetCondition;
}