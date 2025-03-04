public class RouletteBet : Bet
{
    public RouletteBet(BetData betData, int amount) : base(betData, amount) {}

    public override float ResolveBet(int winningNumber)
    {
        return BetInfo.BetCondition.Evaluate(winningNumber,BetInfo.Numbers) 
            ? Amount + Amount * BetInfo.PayoutMultiplier 
            : 0;
    }
}