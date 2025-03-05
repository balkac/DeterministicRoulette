using System.Collections.Generic;
using UnityEngine;

public class TextFeedbackUI : MonoBehaviour
{
    [SerializeField] private TextContainer _textContainer;

    [SerializeField] private Transform _textContainerParent;

    private List<TextContainer> _texts = new List<TextContainer>();

    public void ShowText(string text, Vector2 position, Transform parent = null)
    {
        Transform targetParent = parent != null ? parent : _textContainerParent;

        var go = Instantiate(_textContainer.gameObject, position, Quaternion.identity, targetParent);
        TextContainer textContainer = go.GetComponent<TextContainer>();
        _texts.Add(textContainer);
        textContainer.Initialize(text, () => { _texts.Remove(textContainer); });
        go.transform.localPosition = new Vector3(go.transform.localPosition.x,
            go.transform.localPosition.y, 0);
        (go.transform as RectTransform).anchoredPosition = position;
    }
}