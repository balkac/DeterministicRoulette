using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RouletteManager : MonoBehaviour
{
    [SerializeField] private WheelController _wheelController;
    [SerializeField] private BallController _ballController;
    [SerializeField] private SpinButtonWidget _spinButtonWidget;
    [SerializeField] private AmericanClothUI _americanClothUI;

    [Tooltip("-2 means random")] [SerializeField]
    private int _predeterminedNumber = -2;

    public Action OnSpinStarted;
    public Action OnSpinCompleted;
    public Action<WheelNumberData> OnSpinDataReceived;

    private WheelNumberData _resultData;

    private void Awake()
    {
        _americanClothUI.OnNumberSelected += OnNumberSelected;
        _americanClothUI.OnNumberDeselected += OnNumberDeselected;
        _spinButtonWidget.OnSpinButtonPressed += OnSpinButtonPressed;
    }

    private void OnNumberDeselected()
    {
        SetPredeterminedNumber(-2);
    }

    private void OnNumberSelected(int number)
    {
        SetPredeterminedNumber(number);
    }

    private void OnDestroy()
    {
        _americanClothUI.OnNumberSelected -= OnNumberSelected;
        _americanClothUI.OnNumberDeselected -= OnNumberDeselected;
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

        _resultData = _wheelController.GetNumberData(winningNumber);

        _ballController.StartSpin(_resultData);
        OnSpinStarted?.Invoke();

        _ballController.OnBallSpinCompleted -= OnBallSpinComplete;
        _ballController.OnBallSpinCompleted += OnBallSpinComplete;

        Debug.Log("Winning Number: " + _resultData.Number);
        Debug.Log("Winning Number Type: " + _resultData.NumberType);
    }

    private void OnBallSpinComplete()
    {
        OnSpinCompleted?.Invoke();
        OnSpinDataReceived?.Invoke(_resultData);
        ResolveBets(_resultData.Number);
    }

    private void ResolveBets(int winningNumber)
    {
        List<RouletteBet> bets = BetManager.Instance.GetAllPlacedBets();

        int totalPayout = 0;

        foreach (var bet in bets)
        {
            int payout = (int)bet.ResolveBet(winningNumber);
            if (payout > 0)
            {
                Debug.Log(
                    $"[RouletteManager] Bet WON: {bet.BetInfo.BetType} - Winning Number: {winningNumber} - Payout: {payout}");
            }

            totalPayout += payout;
        }

        Debug.Log("[RouletteManager] Total Payout: " + totalPayout);
    }

    private void SetPredeterminedNumber(int number)
    {
        _predeterminedNumber = number;
    }
}