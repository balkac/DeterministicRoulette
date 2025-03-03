using UnityEngine;
using TMPro;

public class BetViewWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _betAmountText;

    private void Awake()
    {
        BetManager.Instance.OnBetAmountChanged += UpdateBetAmountText;
        UpdateBetAmountText(BetManager.Instance.GetTotalBetAmount());
    }

    private void OnDestroy()
    {
        if (BetManager.Instance != null)
        {
            BetManager.Instance.OnBetAmountChanged -= UpdateBetAmountText;
        }
    }

    private void UpdateBetAmountText(int totalBetAmount)
    {
        _betAmountText.text = "BET : " + totalBetAmount.ToString();
    }
}