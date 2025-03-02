using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class NumberBetWidget : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _animationDuration = 0.125f;
    [SerializeField] private Vector3 _targetScale = new Vector3(0.95f, 0.95f, 0.95f);

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

    private Vector3 _initialScale;
    private bool _isPointerDown = false;
    private Coroutine _pressCoroutine;
    private bool _isSelectedAsPredetermined = false;

    public Action<NumberBetWidget> OnNumberBetWidgetSelected;
    public Action<NumberBetWidget> OnNumberBetWidgetDeselected;

    private void Awake()
    {
        _initialScale = transform.localScale;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
        _pressCoroutine = StartCoroutine(PressDurationCoroutine());
    }

    public void OnPointerUp(PointerEventData eventData)
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