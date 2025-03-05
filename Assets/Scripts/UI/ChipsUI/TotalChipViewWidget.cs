using System.Collections;
using TMPro;
using UnityEngine;

public class TotalChipViewWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalChipText;
    [SerializeField] private ChipRewardMovementController _chipRewardMovementController;
    [SerializeField] private float _punchScaleDuration = 0.5f;
    [SerializeField] private float _textChangeDuration = 0.5f;
    [SerializeField] private float _punchScaleAmount = 1.1f;
    private int _totalChipCount;
    private bool _isDestroying = false;

    private void Awake()
    {
        _totalChipCount = ChipManager.Instance.StartingChipCount;
        _totalChipText.text = _totalChipCount.ToString();
        ChipManager.Instance.OnTotalChipCountChanged += OnTotalChipCountChanged;
    }

    private void OnDestroy()
    {
        _isDestroying = true;

        if (ChipManager.Instance != null)
        {
            ChipManager.Instance.OnTotalChipCountChanged -= OnTotalChipCountChanged;
        }

        _chipRewardMovementController.OnChipMovementCompleted -= OnChipMovementCompleted;
    }

    private void OnTotalChipCountChanged(int totalChipCount)
    {
        if (totalChipCount > _totalChipCount)
        {
            _chipRewardMovementController.OnChipMovementCompleted -= OnChipMovementCompleted;
            _chipRewardMovementController.OnChipMovementCompleted += OnChipMovementCompleted;
            _totalChipCount = totalChipCount;
            return;
        }

        _totalChipCount = totalChipCount;
        _totalChipText.text = _totalChipCount.ToString();
    }

    private void OnChipMovementCompleted()
    {
        if (!_isDestroying)
        {
            StartCoroutine(PunchScale());
            StartCoroutine(AnimateTextChange(int.Parse(_totalChipText.text), _totalChipCount));
        }
    }

    private IEnumerator PunchScale()
    {
        Vector3 originalScale = transform.parent.localScale;
        Vector3 punchScale = originalScale * _punchScaleAmount;
        float elapsedTime = 0f;

        while (elapsedTime < _punchScaleDuration)
        {
            if (_isDestroying) yield break;

            elapsedTime += Time.deltaTime;
            float t = Mathf.Sin((elapsedTime / _punchScaleDuration) * Mathf.PI);
            transform.parent.localScale = Vector3.Lerp(originalScale, punchScale, t);
            yield return null;
        }

        transform.parent.localScale = originalScale;
    }

    private IEnumerator AnimateTextChange(int startValue, int endValue)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _textChangeDuration)
        {
            if (_isDestroying) yield break;

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _textChangeDuration;
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, t));
            _totalChipText.text = currentValue.ToString();
            yield return null;
        }

        _totalChipText.text = endValue.ToString();
    }
}