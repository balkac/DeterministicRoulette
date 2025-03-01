using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RouletteManager : MonoBehaviour
{
    [SerializeField] private WheelController _wheelController;
    [SerializeField] private BallController _ballController;
    [SerializeField] private SpinButtonWidget _spinButtonWidget;

    [Tooltip("-2 means random")] [SerializeField]
    private int _predeterminedNumber = -2;

    public Action OnSpinStarted;
    public Action OnSpinCompleted;

    private void Awake()
    {
        _spinButtonWidget.OnSpinButtonPressed += OnSpinButtonPressed;
    }

    private void OnDestroy()
    {
        _spinButtonWidget.OnSpinButtonPressed -= OnSpinButtonPressed;
        _ballController.OnBallSpinCompleted -= OnSpinCompleted;
    }

    private void OnSpinButtonPressed()
    {
        SpinRoulette();
    }

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
            : Random.Range(-1, 37);

        WheelNumberData resultData = _wheelController.GetNumberData(winningNumber);

        _ballController.StartSpin(resultData);
        OnSpinStarted?.Invoke();

        _ballController.OnBallSpinCompleted -= OnBallSpinComplete;
        _ballController.OnBallSpinCompleted += OnBallSpinComplete;

        Debug.Log("Winning Number: " + resultData.Number);
        Debug.Log("Winning Number Type: " + resultData.NumberType);
    }

    private void OnBallSpinComplete()
    {
        OnSpinCompleted?.Invoke();
    }

    public void SetPredeterminedNumber(int number)
    {
        _predeterminedNumber = number;
    }
}