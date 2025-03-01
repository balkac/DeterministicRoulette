using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberBetWidget : MonoBehaviour
{
    [SerializeField] private Color _redContainerColor;
    [SerializeField] private Color _blackContainerColor;
    [SerializeField] private Color _greenContainerColor;

    [SerializeField] private Material _redTextMaterial;
    [SerializeField] private Material _blackTextMaterial;
    [SerializeField] private Material _greenTextMaterial;

    public ENumberType NumberType;
    public string Number; // Changed to string
    public Image ContainerImage;
    public TextMeshProUGUI NumberText;

    private void OnValidate()
    {
        UpdateImageColor();
        UpdateText();
        UpdateTextMaterial();
        UpdateGameObjectName();
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
}