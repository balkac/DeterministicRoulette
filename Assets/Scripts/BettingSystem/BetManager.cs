using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BetTypeList
{
    public BetType BetType;
    public List<BetData> Bets;
}

public class BetManager : Singleton<BetManager>
{
    [SerializeField] private List<BetTypeList> _betTypeLists = new List<BetTypeList>();
    private List<RouletteBet> _placedBets = new List<RouletteBet>();
    private Stack<RouletteBet> _betHistory = new Stack<RouletteBet>();
    private Stack<bool> _betActions = new Stack<bool>(); // true -> bet added, false -> bet removed

    public Action<int> OnBetAmountChanged;
    public Action<RouletteBet> OnBetUpdated;
    public Action OnAllBetsCleared;

    public void PlaceBet(BetData betData)
    {
        Chip chip = ChipManager.Instance.GetSelectedChip();
        RouletteBet bet = new RouletteBet(betData, chip.Value);
        _placedBets.Add(bet);
        ChipManager.Instance.PlaceBet(betData, chip);
        _betHistory.Push(bet);
        _betActions.Push(true);
        // Debug.Log(
        //     $"[BetManager] Bet placed: {betData.BetType} - Numbers: {string.Join(", ", betData.Numbers)} - Amount: {chip.Value}");

        OnBetAmountChanged?.Invoke(GetTotalBetAmount());
    }

    public void PlaceBetByType(BetType betType, List<int> numbers)
    {
        List<BetData> bets = GetRelatedBets(betType, numbers);
        foreach (var bet in bets)
        {
            PlaceBet(bet);
        }
    }

    public bool TryRemoveLastBetByType(BetType betType, List<int> numbers)
    {
        List<BetData> bets = GetRelatedBets(betType, numbers);
        if (bets.Count > 0)
        {
            BetData lastBet = bets.Last();
            if (ChipManager.Instance.TryRemoveLastPlacedChip(lastBet))
            {
                RouletteBet betToRemove = _placedBets.LastOrDefault(bet => bet.BetInfo == lastBet);
                if (betToRemove != null)
                {
                    _placedBets.Remove(betToRemove);
                    _betHistory.Push(betToRemove);
                    _betActions.Push(false);
                }

                // Debug.Log(
                //     $"[BetManager] Removed last placed chip from {betType} Bet: {lastBet.BetType} - Remaining Total Bet: {ChipManager.Instance.GetBetAmount(lastBet)}");
                OnBetAmountChanged?.Invoke(GetTotalBetAmount());
                return true;
            }
        }

        return false;
    }

    public void UndoLastAction()
    {
        if (_betHistory.Count == 0 || _betActions.Count == 0)
            return;

        RouletteBet lastBet = _betHistory.Pop();
        bool lastAction = _betActions.Pop();

        if (lastAction) // If last action was placing a bet, remove it
        {
            _placedBets.Remove(lastBet);
            ChipManager.Instance.TryRemoveLastPlacedChip(lastBet.BetInfo);
            // Debug.Log($"[BetManager] Undo: Removed last placed bet: {lastBet.BetInfo.BetType}");
        }
        else // If last action was removing a bet, re-add it
        {
            _placedBets.Add(lastBet);
            ChipManager.Instance.PlaceBet(lastBet.BetInfo, new Chip(lastBet.Amount));
            // Debug.Log($"[BetManager] Undo: Restored last removed bet: {lastBet.BetInfo.BetType}");
        }

        OnBetUpdated?.Invoke(lastBet);
        OnBetAmountChanged?.Invoke(GetTotalBetAmount());
    }

    public void ClearAllBets()
    {
        _placedBets.Clear();
        _betHistory.Clear();
        _betActions.Clear();
        ChipManager.Instance.ClearAllBets();
        OnAllBetsCleared?.Invoke();
        OnBetAmountChanged?.Invoke(0);
        // Debug.Log("[BetManager] All bets cleared.");
    }

    public List<RouletteBet> GetAllPlacedBets()
    {
        return new List<RouletteBet>(_placedBets);
    }

    private List<BetData> GetRelatedBets(BetType betType, List<int> numbers)
    {
        return _betTypeLists.FirstOrDefault(bt => bt.BetType == betType)?.Bets
            .Where(bet => numbers.All(num => bet.Numbers.Contains(num)))
            .ToList() ?? new List<BetData>();
    }

    public int GetTotalBetAmount()
    {
        return _placedBets.Sum(bet => bet.Amount);
    }
}