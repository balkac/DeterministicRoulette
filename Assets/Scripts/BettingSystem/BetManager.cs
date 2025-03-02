using System.Collections.Generic;

public class BetManager : Singleton<BetManager>
{
    private List<RouletteBet> _placedBets = new List<RouletteBet>();

    public void PlaceBet(BetData betData)
    {
        int chipValue = ChipManager.Instance.GetSelectedChip();
        RouletteBet bet = new RouletteBet(betData, chipValue);
        _placedBets.Add(bet);
    }
}