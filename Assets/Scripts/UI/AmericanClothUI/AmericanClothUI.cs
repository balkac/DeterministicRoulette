using System;
using System.Collections.Generic;
using UnityEngine;

public class AmericanClothUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RouletteManager _rouletteManager;

    private List<NumberBetWidget> _numberBetWidgets;
    private NumberBetWidget _selectedNumberBetWidget;

    public Action<int> OnNumberSelected;
    public Action OnNumberDeselected;

    private void Awake()
    {
        _numberBetWidgets = new List<NumberBetWidget>(GetComponentsInChildren<NumberBetWidget>());

        foreach (NumberBetWidget numberBetWidget in _numberBetWidgets)
        {
            numberBetWidget.OnNumberBetWidgetSelected += OnNumberBetWidgetSelected;
            numberBetWidget.OnNumberBetWidgetDeselected += OnNumberBetWidgetDeselected;
        }

        _rouletteManager.OnSpinStarted += OnSpinStarted;
        _rouletteManager.OnSpinCompleted += OnSpinCompleted;
    }

    private void OnNumberBetWidgetDeselected(NumberBetWidget obj)
    {
        _selectedNumberBetWidget = null;
        OnNumberDeselected?.Invoke();
    }

    private void OnNumberBetWidgetSelected(NumberBetWidget numberBetWidget)
    {
        if (_selectedNumberBetWidget != null)
        {
            _selectedNumberBetWidget.TryDeselect();
        }

        _selectedNumberBetWidget = numberBetWidget;
        OnNumberSelected?.Invoke(numberBetWidget.GetNumber());
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
        foreach (NumberBetWidget numberBetWidget in _numberBetWidgets)
        {
            numberBetWidget.OnNumberBetWidgetDeselected -= OnNumberBetWidgetDeselected;
            numberBetWidget.OnNumberBetWidgetSelected -= OnNumberBetWidgetSelected;
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