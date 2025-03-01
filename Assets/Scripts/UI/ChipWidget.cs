using UnityEngine;
using TMPro;

public class ChipWidget : MonoBehaviour
{
    [SerializeField] private int _chipValue;

    public TextMeshProUGUI ChipText;

    private void OnValidate()
    {
        UpdateChipText();
    }

    private void UpdateChipText()
    {
        if (ChipText != null)
        {
            ChipText.text = _chipValue.ToString();
        }
    }
}