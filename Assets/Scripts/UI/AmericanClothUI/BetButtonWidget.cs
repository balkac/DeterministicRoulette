using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BetButtonWidget : ButtonWidgetBase
{
    [SerializeField] protected BetType _betType = BetType.Straight;
    [SerializeField] protected Transform _chipWidgetsParent;
    [SerializeField] protected Button _button;
    [SerializeField] protected List<int> Numbers;
    [SerializeField] protected TextMeshProUGUI _chipValueText;

    protected bool _canSelect = true;
    private List<ChipWidget> _placedChips = new List<ChipWidget>();

    protected override void Awake()
    {
        base.Awake();
        _button.onClick.AddListener(OnButtonClick);
        BetManager.Instance.OnBetUpdated += OnBetUpdated;
        BetManager.Instance.OnAllBetsCleared += OnAllBetsCleared;
        UpdateChipValueText();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _button.onClick.RemoveListener(OnButtonClick);

        if (BetManager.Instance == null)
        {
            return;
        }

        BetManager.Instance.OnBetUpdated -= OnBetUpdated;
        BetManager.Instance.OnAllBetsCleared -= OnAllBetsCleared;
    }

    private void OnAllBetsCleared()
    {
        _placedChips.ForEach(chip => chip.Deactivate());
        _placedChips.Clear();
        UpdateChipValueText();
        OnAllBetsClearedCustomActions();
    }

    protected virtual void OnAllBetsClearedCustomActions()
    {
    }
    private void OnBetUpdated(RouletteBet rouletteBet)
    {
        if (rouletteBet.BetInfo.BetType == _betType)
        {
            if (Numbers.Count == 0)
            {
                UpdateChipImages(rouletteBet);
                UpdateChipValueText();
                return;
            }

            if (rouletteBet.BetInfo.Numbers.All(num => Numbers.Contains(num)))
            {
                UpdateChipImages(rouletteBet);
                UpdateChipValueText();
            }
        }
    }

    private void OnButtonClick()
    {
        if (!_canSelect)
        {
            return;
        }

        if (ChipManager.Instance.GetSelectedChip() != null)
        {
            PlaceBet();
            PlaceChipImage();
        }
        else
        {
            RemoveLastBetByType();
            RemoveChipImage();
        }

        UpdateChipValueText();
    }

    public List<int> GetNumbers()
    {
        return Numbers;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;
    }

    private void PlaceBet()
    {
        BetManager.Instance.PlaceBetByType(_betType, GetNumbers());
    }

    private void RemoveLastBetByType()
    {
        BetManager.Instance.TryRemoveLastBetByType(_betType, GetNumbers());
    }

    private void UpdateChipValueText()
    {
        if (_chipValueText == null)
        {
            return;
        }

        _chipValueText.text = _placedChips.Count > 0 ? _placedChips.Sum(chip => chip.GetChipValue()).ToString() : "0";
    }

    private void UpdateChipImages(RouletteBet rouletteBet)
    {
        int placedChipValue = _placedChips.Sum(chip => chip.GetChipValue());
        int newTotalBetAmount = ChipManager.Instance.GetTotalBetAmount(rouletteBet.BetInfo);

        if (newTotalBetAmount > placedChipValue)
        {
            PlaceChipImage(newTotalBetAmount - placedChipValue);
        }
        else
        {
            RemoveChipImage();
        }
    }

    private void PlaceChipImage(int chipValue = 0)
    {
        GameObject chipWidgetGo = PoolManager.Instance.GetObjectFromPool<ChipWidget>();

        chipWidgetGo.transform.SetParent(_chipWidgetsParent);

        ChipWidget chipWidget = chipWidgetGo.GetComponent<ChipWidget>();

        _placedChips.Add(chipWidget);

        RectTransform rectTransform = chipWidget.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero + new Vector3(0f, 5f * (_placedChips.Count), 0f);
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one * 0.45f;

        if (chipValue == 0)
        {
            chipWidget.Initialize(ChipManager.Instance.GetSelectedChip().Value);
        }
        else
        {
            chipWidget.Initialize(chipValue);
        }
    }

    private void RemoveChipImage()
    {
        if (_placedChips.Count > 0)
        {
            ChipWidget chipWidget = _placedChips[^1];
            chipWidget.Deactivate();
            _placedChips.RemoveAt(_placedChips.Count - 1);
        }
    }

    private void OnValidate()
    {
        UpdateGameObjectName();
    }

    private void UpdateGameObjectName()
    {
        if (Numbers.Count == 0)
        {
            gameObject.name = "BetButtonWidget_" + _betType;
            return;
        }

        gameObject.name = "BetButtonWidget_" + _betType + "_" +
                          string.Join("_", Numbers.Select(n => n == -1 ? "00" : n.ToString()));
    }
}