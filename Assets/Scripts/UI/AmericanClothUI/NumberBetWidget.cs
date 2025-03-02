using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class NumberBetWidget : ButtonWidgetBase
{
    [SerializeField] private Color _redContainerColor;
    [SerializeField] private Color _blackContainerColor;
    [SerializeField] private Color _greenContainerColor;

    [SerializeField] private Material _redTextMaterial;
    [SerializeField] private Material _blackTextMaterial;
    [SerializeField] private Material _greenTextMaterial;

    [SerializeField] private Image _highlightedImage;

    public ENumberType NumberType;
    public string Number;
    public Image ContainerImage;
    public TextMeshProUGUI NumberText;

    private Coroutine _pressCoroutine;
    private bool _isSelectedAsPredetermined = false;

    public Action<NumberBetWidget> OnNumberBetWidgetSelected;
    public Action<NumberBetWidget> OnNumberBetWidgetDeselected;

    protected override void AwakeCustomActions()
    {
        base.AwakeCustomActions();
        _highlightedImage.enabled = false;
    }


    private void OnValidate()
    {
        UpdateImageColor();
        UpdateText();
        UpdateTextMaterial();
        UpdateGameObjectName();
    }

    public int GetNumber()
    {
        if (Number == "00")
        {
            return -1;
        }

        return int.Parse(Number);
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
            NumberText.text = Number;
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
        gameObject.name = "NumberBetWidget_" + Number;
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