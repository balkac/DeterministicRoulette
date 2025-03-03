using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;

public class NumberBetWidget : ButtonWidgetBase
{
    [SerializeField] BetType _betType = BetType.Straight;
    [SerializeField] private Color _redContainerColor;
    [SerializeField] private Color _blackContainerColor;
    [SerializeField] private Color _greenContainerColor;

    [SerializeField] private Material _redTextMaterial;
    [SerializeField] private Material _blackTextMaterial;
    [SerializeField] private Material _greenTextMaterial;

    [SerializeField] private Image _highlightedImage;
    [SerializeField] private Transform _chipWidgetsParent;
    [SerializeField] private Button _button;
    public ENumberType NumberType;
    public List<int> Numbers;
    public Image ContainerImage;
    public TextMeshProUGUI NumberText;

    private Coroutine _pressCoroutine;
    private bool _isSelectedAsPredetermined = false;
    private List<ChipWidget> _placedChips = new List<ChipWidget>();

    public Action<NumberBetWidget> OnNumberBetWidgetSelected;
    public Action<NumberBetWidget> OnNumberBetWidgetDeselected;

    protected override void AwakeCustomActions()
    {
        base.AwakeCustomActions();
        _highlightedImage.enabled = false;
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (ChipManager.Instance.GetSelectedChip() != null)
        {
            PlaceBet();
            PlaceChipImage();
        }
        else
        {
            RemoveLastBetByType();
            RemoveChipImage();
        }
    }

    private void PlaceBet()
    {
        BetManager.Instance.PlaceBetByType(_betType, GetNumbers());
    }

    private void RemoveLastBetByType()
    {
        BetManager.Instance.TryRemoveLastBetByType(_betType, GetNumbers());
    }

    private void PlaceChipImage()
    {
        GameObject chipWidgetGo = PoolManager.Instance.GetObjectFromPool<ChipWidget>();

        chipWidgetGo.transform.SetParent(_chipWidgetsParent);

        ChipWidget chipWidget = chipWidgetGo.GetComponent<ChipWidget>();

        _placedChips.Add(chipWidget);

        RectTransform rectTransform = chipWidget.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero + new Vector3(0f, 5f * (_placedChips.Count), 0f);
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one * 0.5f;

        chipWidget.Initialize(ChipManager.Instance.GetSelectedChip().Value);
    }
    
    private void RemoveChipImage()
    {
        if (_placedChips.Count > 0)
        {
            ChipWidget chipWidget = _placedChips[^1];
            chipWidget.Deactivate();
            _placedChips.RemoveAt(_placedChips.Count - 1);
        }
    }

    private void OnValidate()
    {
        if (_betType == BetType.Straight && Numbers.Count > 0)
        {
            UpdateImageColor();
            UpdateText();
            UpdateTextMaterial();
            UpdateGameObjectName();
        }
    }

    public List<int> GetNumbers()
    {
        return Numbers;
    }

    private void UpdateImageColor()
    {
        switch (NumberType)
        {
            case ENumberType.Red:
                ContainerImage.color = _redContainerColor;
                break;
            case ENumberType.Black:
                ContainerImage.color = _blackContainerColor;
                break;
            case ENumberType.Green:
                ContainerImage.color = _greenContainerColor;
                break;
        }
    }

    private void UpdateText()
    {
        if (NumberText != null)
        {
            if (Numbers[0] == -1)
            {
                NumberText.text = "00";
                return;
            }

            NumberText.text = Numbers[0].ToString();
        }
    }

    private void UpdateTextMaterial()
    {
        switch (NumberType)
        {
            case ENumberType.Red:
                NumberText.fontMaterial = _redTextMaterial;
                break;
            case ENumberType.Black:
                NumberText.fontMaterial = _blackTextMaterial;
                break;
            case ENumberType.Green:
                NumberText.fontMaterial = _greenTextMaterial;
                break;
        }
    }

    private void UpdateGameObjectName()
    {
        gameObject.name = "NumberBetWidget_" + string.Join("_", Numbers.Select(n => n == -1 ? "00" : n.ToString()));
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
        _pressCoroutine = StartCoroutine(PressDurationCoroutine());
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;
        if (_pressCoroutine != null)
        {
            StopCoroutine(_pressCoroutine);
        }

        if (!_isSelectedAsPredetermined)
        {
            _highlightedImage.enabled = false;
        }
    }

    private IEnumerator PressDurationCoroutine()
    {
        yield return new WaitForSeconds(1f);
        if (_isPointerDown)
        {
            if (_isSelectedAsPredetermined)
            {
                OnNumberBetWidgetDeselected?.Invoke(this);
                _isSelectedAsPredetermined = false;
                _highlightedImage.enabled = false;
            }
            else
            {
                OnNumberBetWidgetSelected?.Invoke(this);
                _highlightedImage.enabled = true;
                _isSelectedAsPredetermined = true;
            }
        }
    }

    public void TryDeselect()
    {
        if (_isSelectedAsPredetermined)
        {
            _isSelectedAsPredetermined = false;
            _highlightedImage.enabled = false;
        }
    }
}