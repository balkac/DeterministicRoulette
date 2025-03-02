using UnityEngine;
using TMPro;

public class ChipWidget : MonoBehaviour
{
    [SerializeField] private int _chipValue;

    [SerializeField] private TextMeshProUGUI _chipText;

    private void OnValidate()
    {
        UpdateChipText();
    }

    private void UpdateChipText()
    {
        if (_chipText != null)
        {
            _chipText.text = _chipValue.ToString();
        }
    }
}