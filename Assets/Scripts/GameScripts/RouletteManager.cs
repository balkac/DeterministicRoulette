using UnityEngine;

public class RouletteManager : MonoBehaviour
{
    public WheelController WheelController;
    public BallController BallController;
    public int _predeterminedNumber = -2;

    private System.Random random = new System.Random();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpinRoulette();
        }
    }

    public void SpinRoulette()
    {
        // -1 for 00
        int winningNumber = (_predeterminedNumber >= -1 && _predeterminedNumber <= 36)
            ? _predeterminedNumber
            : random.Next(-1, 37);

        WheelNumberData resultData = WheelController.GetNumberData(winningNumber);

        BallController.StartSpin(resultData);

        Debug.Log("Winning Number: " + resultData.Number);
        Debug.Log("Winning Number Type: " + resultData.NumberType);
    }

    public void SetPredeterminedNumber(int number)
    {
        _predeterminedNumber = number;
    }
}