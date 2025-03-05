using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    
    [SerializeField] private Animator _animator;
    
    private Action _onAnimationStop;
    
    public void Initialize(string text, Action onDestroy)
    {
        _text.text = text;
        gameObject.SetActive(true);
        PlayAnimation(() =>
        {
            onDestroy?.Invoke();
            Destroy(this.gameObject);
        });
    }

    public void PlayAnimation(Action onAnimationStop = null)
    {
        _onAnimationStop = onAnimationStop;
        _animator.Play("Floating", 0, 0);
        StartCoroutine(WaitForAnimationEnd());
    }

    private IEnumerator WaitForAnimationEnd()
    {
        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        _onAnimationStop?.Invoke();
    }
}