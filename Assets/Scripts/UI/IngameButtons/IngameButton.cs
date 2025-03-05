using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngameButton : ButtonWidgetBase
{
    [SerializeField] protected CanvasGroup _canvasGroup;
    [SerializeField] protected Button _button;

    public Action OnButtonClicked;

    protected override void Awake()
    {
        base.Awake();

        _button.onClick.AddListener(OnButtonClick);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        OnButtonClicked?.Invoke();
        OnButtonClickCustomActions();
    }

    protected virtual void OnButtonClickCustomActions()
    {
        SoundManager.Instance.PlayButtonClickSound();
    }

    protected void Activate()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
    }

    protected void Deactivate()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
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