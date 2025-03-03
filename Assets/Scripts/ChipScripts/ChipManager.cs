using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChipManager : Singleton<ChipManager>
{
    private Chip _selectedChip;
    private Dictionary<BetData, List<Chip>> _betAmounts = new Dictionary<BetData, List<Chip>>();

    public void SetSelectedChip(Chip chip)
    {
        _selectedChip = chip;
    }

    public Chip GetSelectedChip()
    {
        return _selectedChip;
    }

    public void PlaceBet(BetData betData, Chip chip)
    {
        if (!_betAmounts.ContainsKey(betData))
        {
            _betAmounts[betData] = new List<Chip>();
        }

        _betAmounts[betData].Add(chip);
        Debug.Log(
            $"[ChipManager] Chip placed on {betData.BetType} - Numbers: {string.Join(", ", betData.Numbers)} - Total Bet: {GetBetAmount(betData)}");
    }

    public int GetBetAmount(BetData betData)
    {
        return _betAmounts.TryGetValue(betData, out var chips) ? chips.Sum(chip => chip.Value) : 0;
    }

    public void RemoveBet(BetData betData)
    {
        if (_betAmounts.ContainsKey(betData))
        {
            _betAmounts.Remove(betData);
            Debug.Log(
                $"[ChipManager] Bet removed: {betData.BetType} - Numbers: {string.Join(", ", betData.Numbers)} - Remaining Total Bet: {GetBetAmount(betData)}");
        }
    }

    public bool TryRemoveLastPlacedChip(BetData betData)
    {
        if (_betAmounts.TryGetValue(betData, out var chips) && chips.Count > 0)
        {
            Chip removedChip = chips.Last();
            chips.RemoveAt(chips.Count - 1);
            Debug.Log(
                $"[ChipManager] Removed last chip ({removedChip.Value}) from {betData.BetType} - Numbers: {string.Join(", ", betData.Numbers)} - Remaining Total Bet: {GetBetAmount(betData)}");
            if (chips.Count == 0)
            {
                _betAmounts.Remove(betData);
            }

            return true;
        }

        return false;
    }
}