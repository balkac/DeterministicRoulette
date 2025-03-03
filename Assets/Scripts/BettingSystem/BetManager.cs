using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class BetTypeList
{
    public BetType BetType;
    public List<BetData> Bets;
}

public class BetManager : Singleton<BetManager>
{
    [SerializeField] private List<BetTypeList> _betTypeLists = new List<BetTypeList>();
    private List<RouletteBet> _placedBets = new List<RouletteBet>();

    public void PlaceBet(BetData betData)
    {
        Chip chip = ChipManager.Instance.GetSelectedChip();
        _placedBets.Add(new RouletteBet(betData, chip.Value));
        ChipManager.Instance.PlaceBet(betData, chip);
    }

    public void PlaceSplitBet(int number)
    {
        List<BetData> splitBets = GetRelatedSplitBets(number);
        foreach (var bet in splitBets)
        {
            PlaceBet(bet);
        }
    }

    private List<BetData> GetRelatedSplitBets(int number)
    {
        return _betTypeLists.FirstOrDefault(bt => bt.BetType == BetType.Split)?.Bets
            .Where(bet => bet.Numbers.Contains(number))
            .ToList() ?? new List<BetData>();
    }

    public List<BetData> GetAllBets()
    {
        return _betTypeLists.SelectMany(betTypeList => betTypeList.Bets).ToList();
    }
}