using UnityEngine;

public class ClearButtonWidget : IngameButton
{
    [SerializeField] private RouletteManager _rouletteManager;

    protected override void AwakeCustomActions()
    {
        base.AwakeCustomActions();
        _rouletteManager.OnSpinCompleted += OnSpinCompleted;
        _rouletteManager.OnSpinStarted += OnSpinStarted;
    }

    protected override void OnDestroyCustomActions()
    {
        base.OnDestroyCustomActions();
        _rouletteManager.OnSpinCompleted -= OnSpinCompleted;
        _rouletteManager.OnSpinStarted -= OnSpinStarted;
    }

    private void OnSpinCompleted()
    {
        Activate();
    }

    private void OnSpinStarted()
    {
        Deactivate();
    }

    protected override void OnButtonClickCustomActions()
    {
        base.OnButtonClickCustomActions();

        BetManager.Instance.ClearAllBets();
    }
}