using System.Collections.Generic;
using System.Linq;

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
        }
    }

    public void RemoveLastPlacedChip(BetData betData)
    {
        if (_betAmounts.TryGetValue(betData, out var chips) && chips.Count > 0)
        {
            chips.RemoveAt(chips.Count - 1);
            if (chips.Count == 0)
            {
                _betAmounts.Remove(betData);
            }
        }
    }
}