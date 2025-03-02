using UnityEngine;

public class ChipManager : Singleton<ChipManager>
{
    private int _selectedChipValue = 1;

    public void SetSelectedChip(int value)
    {
        _selectedChipValue = value;
    }

    public int GetSelectedChip()
    {
        return _selectedChipValue;
    }
}