using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image[] _highlightElements;
    [SerializeField] private CanvasGroup _hoverCanvasGroup;
    [SerializeField] private Color _highlightColor = new Color(1f, 1f, 1f, 0.46f);
    private Color[] originalColors;

    private void Start()
    {
        originalColors = new Color[_highlightElements.Length];
        for (int i = 0; i < _highlightElements.Length; i++)
        {
            originalColors[i] = _highlightElements[i].color;
        }

        if (_hoverCanvasGroup != null)
        {
            _hoverCanvasGroup.alpha = 0f;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < _highlightElements.Length; i++)
        {
            _highlightElements[i].color = _highlightColor;
        }

        if (_hoverCanvasGroup != null)
        {
            _hoverCanvasGroup.alpha = 1f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < _highlightElements.Length; i++)
        {
            _highlightElements[i].color = originalColors[i];
        }

        if (_hoverCanvasGroup != null)
        {
            _hoverCanvasGroup.alpha = 0f;
        }
    }
}