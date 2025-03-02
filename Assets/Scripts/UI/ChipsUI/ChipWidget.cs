using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChipWidget : MonoBehaviour
{
    [SerializeField] private int _chipValue;
    [SerializeField] private TextMeshProUGUI _chipText;
    [SerializeField] private Button _chipButton;
    [SerializeField] private Image _highlightedImage;
    [SerializeField] private Vector3 _selectedScale = new Vector3(1.2f, 1.2f, 1.2f);
    [SerializeField] private float _animationDuration = 0.125f;

    private ChipManager _chipManager;
    private Vector3 _originalScale;
    private Coroutine _scaleCoroutine;

    public Action<ChipWidget> OnChipSelected;
    public Action OnChipDeselected;
    private bool _isSelected = false;

    private void Awake()
    {
        _chipManager = ChipManager.Instance;
        _chipButton.onClick.AddListener(OnChipButtonClicked);
        _highlightedImage.enabled = false;
        _originalScale = transform.localScale;
        UpdateChipText();
    }

    private void OnDestroy()
    {
        _chipButton.onClick.RemoveListener(OnChipButtonClicked);
    }

    private void UpdateChipText()
    {
        if (_chipText != null)
        {
            _chipText.text = _chipValue.ToString();
        }
    }

    private void OnChipButtonClicked()
    {
        if (_isSelected)
        {
            DeselectChip();
            OnChipDeselected?.Invoke();
            return;
        }

        SelectChip();
        OnChipSelected?.Invoke(this);
    }

    private void SelectChip()
    {
        _isSelected = true;
        _highlightedImage.enabled = true;
        _chipManager.SetSelectedChip(_chipValue);
        if (_scaleCoroutine != null)
        {
            StopCoroutine(_scaleCoroutine);
        }

        _scaleCoroutine = StartCoroutine(ScaleTo(_selectedScale));
    }

    public void DeselectChip()
    {
        _isSelected = false;
        _highlightedImage.enabled = false;
        _chipManager.SetSelectedChip(0);
        if (_scaleCoroutine != null)
        {
            StopCoroutine(_scaleCoroutine);
        }

        _scaleCoroutine = StartCoroutine(ScaleTo(_originalScale));
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < _animationDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / _animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}