using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpinButtonWidget : ButtonWidgetBase
{
    [SerializeField] private RouletteManager _rouletteManager;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _spinButton;

    public Action OnSpinButtonPressed;

    protected override void AwakeCustomActions()
    {
        base.AwakeCustomActions();
        _rouletteManager.OnSpinCompleted += OnSpinCompleted;
        _spinButton.onClick.AddListener(OnSpinButtonClicked);
    }

    private void OnDestroy()
    {
        _rouletteManager.OnSpinCompleted -= OnSpinCompleted;
        _spinButton.onClick.RemoveListener(OnSpinButtonClicked);
    }

    private void OnSpinCompleted()
    {
        Activate();
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

    private void OnSpinButtonClicked()
    {
        OnSpinButtonPressed?.Invoke();
        Deactivate();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;
    }
}