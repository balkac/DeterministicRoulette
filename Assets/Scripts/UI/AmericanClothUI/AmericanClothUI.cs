using System;
using System.Collections.Generic;
using UnityEngine;

public class AmericanClothUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RouletteManager _rouletteManager;

    private List<NumberBetButtonWidget> _numberBetWidgets;
    private NumberBetButtonWidget _selectedNumberBetButtonWidget;

    public Action<int> OnNumberSelected;
    public Action OnNumberDeselected;

    private void Awake()
    {
        _numberBetWidgets = new List<NumberBetButtonWidget>(GetComponentsInChildren<NumberBetButtonWidget>());

        foreach (NumberBetButtonWidget numberBetWidget in _numberBetWidgets)
        {
            numberBetWidget.OnNumberBetWidgetSelected += OnNumberBetWidgetSelected;
            numberBetWidget.OnNumberBetWidgetDeselected += OnNumberBetWidgetDeselected;
        }

        _rouletteManager.OnSpinStarted += OnSpinStarted;
        _rouletteManager.OnSpinCompleted += OnSpinCompleted;
    }

    private void OnNumberBetWidgetDeselected(NumberBetButtonWidget obj)
    {
        _selectedNumberBetButtonWidget = null;
        OnNumberDeselected?.Invoke();
    }

    private void OnNumberBetWidgetSelected(NumberBetButtonWidget numberBetButtonWidget)
    {
        if (_selectedNumberBetButtonWidget != null)
        {
            _selectedNumberBetButtonWidget.TryDeselect();
        }

        _selectedNumberBetButtonWidget = numberBetButtonWidget;
        OnNumberSelected?.Invoke(numberBetButtonWidget.GetNumbers()[0]);
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
        foreach (NumberBetButtonWidget numberBetWidget in _numberBetWidgets)
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