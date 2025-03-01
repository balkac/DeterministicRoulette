using UnityEngine;

public class RouletteManager : MonoBehaviour
{
    public WheelController WheelController;
    public BallController BallController;
    public int _predeterminedNumber = -1;

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
        int winningNumber = (_predeterminedNumber >= 0 && _predeterminedNumber <= 36)
            ? _predeterminedNumber
            : random.Next(0, 37);

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