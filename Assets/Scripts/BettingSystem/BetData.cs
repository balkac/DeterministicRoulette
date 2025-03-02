using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Bet Data", menuName = "Roulette/Bet Data")]
public class BetData : ScriptableObject
{
    public BetType BetType;
    public int[] Numbers;
    public float PayoutMultiplier;
    public BetCondition Condition;
}