using System.Collections.Generic;
using UnityEngine;

public class ChipsUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RouletteManager _rouletteManager;
    private List<ChipWidget> _chipWidgets;
    private ChipWidget _selectedChipWidget;

    private void Awake()
    {
        _chipWidgets = new List<ChipWidget>(GetComponentsInChildren<ChipWidget>());

        foreach (ChipWidget chipWidget in _chipWidgets)
        {
            chipWidget.OnChipSelected += OnChipSelected;
            chipWidget.OnChipDeselected += OnChipDeselected;
        }

        _rouletteManager.OnSpinStarted += OnSpinStarted;
        _rouletteManager.OnSpinCompleted += OnSpinCompleted;
    }

    private void OnChipDeselected()
    {
        _selectedChipWidget = null;
    }

    private void OnChipSelected(ChipWidget chipWidget)
    {
        if (_selectedChipWidget != null)
        {
            _selectedChipWidget.DeselectChip();
        }

        _selectedChipWidget = chipWidget;
    }

    private void OnSpinStarted()
    {
        Deactivate();
    }

    private void OnSpinCompleted()
    {
        Activate();
    }

    private void OnDestroy()
    {
        foreach (ChipWidget chipWidget in _chipWidgets)
        {
            chipWidget.OnChipSelected -= OnChipSelected;
            chipWidget.OnChipDeselected -= OnChipDeselected;
        }

        _rouletteManager.OnSpinStarted -= OnSpinStarted;
        _rouletteManager.OnSpinCompleted -= OnSpinCompleted;
    }

    private void Activate()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
    }

    private void Deactivate()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
    }
}