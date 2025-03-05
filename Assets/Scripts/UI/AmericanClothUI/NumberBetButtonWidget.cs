using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;

public class NumberBetButtonWidget : BetButtonWidget
{
    [SerializeField] private Color _redContainerColor;
    [SerializeField] private Color _blackContainerColor;
    [SerializeField] private Color _greenContainerColor;

    [SerializeField] private Material _redTextMaterial;
    [SerializeField] private Material _blackTextMaterial;
    [SerializeField] private Material _greenTextMaterial;

    [SerializeField] private Image _highlightedImage;

    public ENumberType NumberType;
    public Image ContainerImage;
    public TextMeshProUGUI NumberText;

    private Coroutine _holdCoroutine;
    private bool _isSelectedAsPredetermined = false;

    public Action<NumberBetButtonWidget> OnNumberBetWidgetSelected;
    public Action<NumberBetButtonWidget> OnNumberBetWidgetDeselected;

    protected override void AwakeCustomActions()
    {
        base.AwakeCustomActions();
        _highlightedImage.enabled = false;
    }

    protected override void OnAllBetsClearedCustomActions()
    {
        base.OnAllBetsClearedCustomActions();
        
        TryDeselect();
        OnNumberBetWidgetDeselected?.Invoke(this);
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
        gameObject.name = "NumberBetWidget_" + _betType + "_" +
                          string.Join("_", Numbers.Select(n => n == -1 ? "00" : n.ToString()));
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
        _canSelect = false;
        _holdCoroutine = StartCoroutine(PressDurationCoroutine());
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;
        if (_holdCoroutine != null)
        {
            StopCoroutine(_holdCoroutine);
            _canSelect = true;
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

        _holdCoroutine = null;
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