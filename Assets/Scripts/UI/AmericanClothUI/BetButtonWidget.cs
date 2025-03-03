using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BetButtonWidget : ButtonWidgetBase
{
    [SerializeField] protected BetType _betType = BetType.Straight;
    [SerializeField] protected Transform _chipWidgetsParent;
    [SerializeField] protected Button _button;
    [SerializeField] protected List<int> Numbers;

    private List<ChipWidget> _placedChips = new List<ChipWidget>();

    protected override void AwakeCustomActions()
    {
        base.AwakeCustomActions();
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
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


    private void PlaceChipImage()
    {
        GameObject chipWidgetGo = PoolManager.Instance.GetObjectFromPool<ChipWidget>();

        chipWidgetGo.transform.SetParent(_chipWidgetsParent);

        ChipWidget chipWidget = chipWidgetGo.GetComponent<ChipWidget>();

        _placedChips.Add(chipWidget);

        RectTransform rectTransform = chipWidget.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero + new Vector3(0f, 5f * (_placedChips.Count), 0f);
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one * 0.45f;

        chipWidget.Initialize(ChipManager.Instance.GetSelectedChip().Value);
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