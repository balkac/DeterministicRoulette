using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpinButtonWidget : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RouletteManager _rouletteManager;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _spinButton;
    [SerializeField] private float _animationDuration = 0.125f;
    [SerializeField] private Vector3 _targetScale = new Vector3(0.95f, 0.95f, 0.95f);

    private Vector3 _initialScale;
    private bool _isPointerDown = false;

    public Action OnSpinButtonPressed;

    private void Awake()
    {
        _initialScale = transform.localScale;
        _rouletteManager.OnSpinCompleted += OnSpinCompleted;
        _spinButton.onClick.AddListener(OnSpinButtonClicked);
    }

    private void OnDestroy()
    {
        _rouletteManager.OnSpinCompleted -= OnSpinCompleted;
        _spinButton.onClick.RemoveListener(OnSpinButtonClicked);
    }

    private void Update()
    {
        if (_isPointerDown)
        {
            transform.localScale =
                Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime / _animationDuration);
        }
        else
        {
            transform.localScale =
                Vector3.Lerp(transform.localScale, _initialScale, Time.deltaTime / _animationDuration);
        }
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

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;
    }
}