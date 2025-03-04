using TMPro;
using UnityEngine;

public class ResulsUI : MonoBehaviour
{
    [SerializeField] private RouletteManager _rouletteManager;

    [SerializeField] private Transform _lastNumberTextWidgetParent;
    [SerializeField] private Transform _scrollContent;
    [SerializeField] private TextMeshProUGUI _spinCountText;

    private LastNumberTextWidget _lastNumberTextWidget = null;

    private void Awake()
    {
        _rouletteManager.OnSpinDataReceived += OnSpinDataReceived;
        _spinCountText.text = "0";
    }

    private void OnDestroy()
    {
        _rouletteManager.OnSpinDataReceived -= OnSpinDataReceived;
    }

    private void OnSpinDataReceived(WheelNumberData resultData, bool isWin, int numberOfSpins)
    {
        _spinCountText.text = numberOfSpins.ToString();

        if (_lastNumberTextWidget != null)
        {
            _lastNumberTextWidget.Deactivate();
        }

        GameObject lastNumberTextWidgetGo = PoolManager.Instance.GetObjectFromPool<LastNumberTextWidget>();

        lastNumberTextWidgetGo.transform.SetParent(_lastNumberTextWidgetParent);

        _lastNumberTextWidget = lastNumberTextWidgetGo.GetComponent<LastNumberTextWidget>();

        RectTransform rectTransform = _lastNumberTextWidget.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;

        _lastNumberTextWidget.Initialize(resultData.Number, resultData.NumberType);

        GameObject scrollObject = PoolManager.Instance.GetObjectFromPool<LastNumberTextWidget>();

        scrollObject.transform.SetParent(_scrollContent);
        scrollObject.transform.SetSiblingIndex(0);

        LastNumberTextWidget lastNumberTextWidget = scrollObject.GetComponent<LastNumberTextWidget>();

        RectTransform scrollRectTransform = lastNumberTextWidget.GetComponent<RectTransform>();
        scrollRectTransform.localPosition = Vector3.zero;
        scrollRectTransform.localRotation = Quaternion.identity;
        scrollRectTransform.localScale = Vector3.one;

        lastNumberTextWidget.Initialize(resultData.Number, resultData.NumberType, isWin, true);
    }
}