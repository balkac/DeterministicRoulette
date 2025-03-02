using UnityEngine;

public class TestBetting : MonoBehaviour
{
    [SerializeField] private BetData testBetData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BetManager.Instance.PlaceBet(testBetData);
            Debug.Log("Bet placed with chip value: " + ChipManager.Instance.GetSelectedChip());
        }
    }
}