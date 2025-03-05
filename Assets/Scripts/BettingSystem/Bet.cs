public abstract class Bet
{
    public BetData BetInfo { get; protected set; }
    public int Amount { get; set; }

    protected Bet(BetData betData, int amount)
    {
        BetInfo = betData;
        Amount = amount;
    }

    public abstract float ResolveBet(int winningNumber);
}