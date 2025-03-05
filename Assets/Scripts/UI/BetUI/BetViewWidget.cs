using UnityEngine;
using TMPro;

public class BetViewWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _betAmountText;

    private void Awake()
    {
        BetManager.Instance.OnBetAmountChanged += OnBetAmountChanged;
        OnBetAmountChanged(BetManager.Instance.GetTotalBetAmount());
    }

    private void OnDestroy()
    {
        if (BetManager.Instance != null)
        {
            BetManager.Instance.OnBetAmountChanged -= OnBetAmountChanged;
        }
    }

    private void OnBetAmountChanged(int totalBetAmount)
    {
        _betAmountText.text = "BET : " + totalBetAmount.ToString();
    }
}