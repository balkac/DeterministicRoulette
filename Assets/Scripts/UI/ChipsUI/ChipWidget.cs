using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChipWidget : PoolObject
{
    [SerializeField] private int _chipValue;
    [SerializeField] private TextMeshProUGUI _chipText;
    [SerializeField] private Button _chipButton;
    [SerializeField] private Image _chipImage;
    [SerializeField] private Image _highlightedImage;
    [SerializeField] private Vector3 _selectedScale = new Vector3(1.2f, 1.2f, 1.2f);
    [SerializeField] private float _animationDuration = 0.125f;
    [SerializeField] private List<ChipWidgetData> _chipWidgetDatas;

    private ChipManager _chipManager;
    private Vector3 _originalScale;
    private Coroutine _scaleCoroutine;

    public Action<ChipWidget> OnChipSelected;
    public Action OnChipDeselected;
    private bool _isSelected = false;
    private Chip _chip = null;

    private void Awake()
    {
        _chipManager = ChipManager.Instance;
        _chipButton.onClick.AddListener(OnChipButtonClicked);
        _highlightedImage.enabled = false;
        _originalScale = transform.localScale;
        _chip = new Chip(_chipValue);
        UpdateChipText();
    }

    public void Initialize(int chipValue)
    {
        _chipImage.raycastTarget = false;
        _chipValue = chipValue;
        _chip = new Chip(_chipValue);
        UpdateChipText();
        UpdateChipImage();
    }

    private void OnDestroy()
    {
        _chipButton.onClick.RemoveListener(OnChipButtonClicked);
    }

    public int GetChipValue()
    {
        return _chipValue;
    }

    private void UpdateChipText()
    {
        if (_chipText != null)
        {
            _chipText.text = _chipValue.ToString();
        }
    }

    private void UpdateChipImage()
    {
        _chipImage.sprite = _chipWidgetDatas.Find(x => x.ChipValue == _chipValue).ChipSprite;
    }

    private void OnChipButtonClicked()
    {
        SoundManager.Instance.PlayButtonClickSound();

        if (_isSelected)
        {
            DeselectChip();
            _chipManager.SetSelectedChip(null);
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
        _chipManager.SetSelectedChip(_chip);
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

[Serializable]
public class ChipWidgetData
{
    public int ChipValue;
    public Sprite ChipSprite;
}