using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LastNumberTextWidget : PoolObject
{
    [SerializeField] private Color _redContainerColor;
    [SerializeField] private Color _blackContainerColor;
    [SerializeField] private Color _greenContainerColor;

    [SerializeField] private Material _redTextMaterial;
    [SerializeField] private Material _blackTextMaterial;
    [SerializeField] private Material _greenTextMaterial;

    [SerializeField] private Image _containerImage;
    [SerializeField] private TextMeshProUGUI _numberText;
    [SerializeField] private TextMeshProUGUI _winLoseText;
    public ENumberType NumberType { get; set; }
    public string Number { get; set; }

    private void Awake()
    {
        _winLoseText.enabled = false;
    }

    public void Initialize(int number, ENumberType numberType, bool isWin = false, bool canShownWinLose = false)
    {
        if (canShownWinLose)
        {
            _winLoseText.enabled = true;
            _winLoseText.text = isWin ? "W" : "L";
        }

        Number = number == -1 ? "00" : number.ToString();
        NumberType = numberType;
        UpdateImageColor();
        UpdateText();
        UpdateTextMaterial();
    }

    protected override void DeactivateCustomActions()
    {
        base.DeactivateCustomActions();
        _winLoseText.enabled = false;
    }

    private void UpdateImageColor()
    {
        switch (NumberType)
        {
            case ENumberType.Red:
                _containerImage.color = _redContainerColor;
                break;
            case ENumberType.Black:
                _containerImage.color = _blackContainerColor;
                break;
            case ENumberType.Green:
                _containerImage.color = _greenContainerColor;
                break;
        }
    }

    private void UpdateText()
    {
        if (_numberText != null)
        {
            _numberText.text = Number;
        }
    }

    private void UpdateTextMaterial()
    {
        switch (NumberType)
        {
            case ENumberType.Red:
                _numberText.fontMaterial = _redTextMaterial;
                break;
            case ENumberType.Black:
                _numberText.fontMaterial = _blackTextMaterial;
                break;
            case ENumberType.Green:
                _numberText.fontMaterial = _greenTextMaterial;
                break;
        }
    }
}