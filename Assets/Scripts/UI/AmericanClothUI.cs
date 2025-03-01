using UnityEngine;

public class AmericanClothUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RouletteManager _rouletteManager;

    private void Awake()
    {
        _rouletteManager.OnSpinStarted += OnSpinStarted;
        _rouletteManager.OnSpinCompleted += OnSpinCompleted;
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

    private void OnSpinButtonPressed()
    {
        Deactivate();
    }
}