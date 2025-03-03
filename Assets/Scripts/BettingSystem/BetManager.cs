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
        Debug.Log(
            $"[BetManager] Bet placed: {betData.BetType} - Numbers: {string.Join(", ", betData.Numbers)} - Amount: {chip.Value}");
    }

    public void PlaceBetByType(BetType betType, List<int> numbers)
    {
        List<BetData> bets = GetRelatedBets(betType, numbers);
        foreach (var bet in bets)
        {
            PlaceBet(bet);
        }
    }

    public bool TryRemoveLastBetByType(BetType betType, List<int> numbers)
    {
        List<BetData> bets = GetRelatedBets(betType, numbers);
        if (bets.Count > 0)
        {
            BetData lastBet = bets.Last();

            if (ChipManager.Instance.TryRemoveLastPlacedChip(lastBet))
            {
                Debug.Log(
                    $"[BetManager] Removed last placed chip from {betType} Bet: {lastBet.BetType} - Numbers: {string.Join(", ", lastBet.Numbers)} - Remaining Total Bet: {ChipManager.Instance.GetBetAmount(lastBet)}");
                return true;
            }
        }

        return false;
    }

    private List<BetData> GetRelatedBets(BetType betType, List<int> numbers)
    {
        return _betTypeLists.FirstOrDefault(bt => bt.BetType == betType)?.Bets
            .Where(bet => numbers.All(num => bet.Numbers.Contains(num)))
            .ToList() ?? new List<BetData>();
    }

    public List<BetData> GetAllBets()
    {
        return _betTypeLists.SelectMany(betTypeList => betTypeList.Bets).ToList();
    }
}