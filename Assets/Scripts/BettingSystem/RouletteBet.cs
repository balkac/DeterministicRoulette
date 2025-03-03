public class RouletteBet : Bet
{
    public RouletteBet(BetData betData, int amount) : base(betData, amount) {}

    public override float ResolveBet(int winningNumber)
    {
        return BetInfo.Condition.Evaluate(winningNumber,BetInfo.Numbers) 
            ? Amount * BetInfo.PayoutMultiplier 
            : 0;
    }
}