using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextFeedbackManager : Singleton<TextFeedbackManager>
{
    [SerializeField] private TextFeedbackUI _textFeedbackUI;
    [SerializeField] private float _cooldownDuration = 0.5f;

    private bool _isOnCooldown = false;
    private List<string> _floatingTexts = new List<string>();
    private List<Transform> _floatingParents = new List<Transform>();
    private Coroutine _cooldownCoroutine;

    public void ShowText(string text, Vector2 position, Transform parent = null)
    {
        if (_isOnCooldown && _floatingTexts.Contains(text) && _floatingParents.Contains(parent))
        {
            return;
        }

        _floatingParents.Add(parent);
        _floatingTexts.Add(text);

        _textFeedbackUI.ShowText(text, position, parent);
        _cooldownCoroutine = StartCoroutine(CooldownCoroutine(text, parent));
    }

    private IEnumerator CooldownCoroutine(string text, Transform parent)
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(_cooldownDuration);

        _floatingParents.Remove(parent);
        _floatingTexts.Remove(text);
        _isOnCooldown = false;
        _cooldownCoroutine = null;
    }
}