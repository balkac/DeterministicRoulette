using TMPro;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _activeDuration = 1.0f;
    [SerializeField] private RouletteManager _rouletteManager;
    [SerializeField] private TextMeshProUGUI _payOutText;

    private void Awake()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        _rouletteManager.OnRouletteWon += OnRouletteWon;
    }

    private void OnDestroy()
    {
        _rouletteManager.OnRouletteWon -= OnRouletteWon;
    }

    private void OnRouletteWon(int payout)
    {
        _payOutText.text = payout.ToString();
        Activate();
    }

    public void Activate()
    {
        SoundManager.Instance.PlayWinSound();

        StartCoroutine(FadeCanvasGroup(_canvasGroup, _canvasGroup.alpha, 1, _animationDuration, () =>
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            StartCoroutine(DeactivateAfterDelay(_activeDuration));
        }));
    }

    private System.Collections.IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Deactivate();
    }

    public void Deactivate()
    {
        StartCoroutine(FadeCanvasGroup(_canvasGroup, _canvasGroup.alpha, 0, _animationDuration, () =>
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }));
    }

    private System.Collections.IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end,
        float duration, System.Action onComplete)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = end;
        onComplete?.Invoke();
    }
}