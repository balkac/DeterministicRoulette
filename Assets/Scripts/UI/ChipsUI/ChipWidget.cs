using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChipWidget : MonoBehaviour
{
    [SerializeField] private int _chipValue;
    [SerializeField] private TextMeshProUGUI _chipText;
    [SerializeField] private Button _chipButton;
    private ChipManager _chipManager;

    private void Awake()
    {
        _chipManager = ChipManager.Instance;
        _chipButton.onClick.AddListener(OnChipSelected);
    }

    private void Start()
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

    private void OnChipSelected()
    {
        _chipManager.SetSelectedChip(_chipValue);
    }
}