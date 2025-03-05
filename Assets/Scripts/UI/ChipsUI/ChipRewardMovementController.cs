using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChipRewardMovementController : MonoBehaviour
{
    [SerializeField] private RectTransform _chipParent;
    [SerializeField] private RectTransform _spawnPosition;
    [SerializeField] private RectTransform _destinationPosition;
    [SerializeField] private int _chipCount = 10;
    [SerializeField] private float _spawnRadius = 0.05f;
    [SerializeField] private float _movementDuration = 1.5f;
    [SerializeField] private float _preMoveDelay = 1f;
    [SerializeField] private AnimationCurve _movementCurve;
    [SerializeField] private RouletteManager _rouletteManager;
    [SerializeField] private Camera _uiCamera;
    private WaitForSeconds _preMoveDelayWait;
    public Action OnChipMovementCompleted;

    private int _completedChips = 0;

    private void Awake()
    {
        _preMoveDelayWait = new WaitForSeconds(_preMoveDelay);
        _rouletteManager.OnRouletteWon += OnRouletteWon;
    }

    private void OnDestroy()
    {
        _rouletteManager.OnRouletteWon -= OnRouletteWon;
    }

    private void OnRouletteWon(int payoutCount)
    {
        MoveChipsToDestination();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveChipsToDestination();
        }
    }

    public void MoveChipsToDestination()
    {
        _completedChips = 0;
        StartCoroutine(SpawnAndPrepareChips());
    }

    private IEnumerator SpawnAndPrepareChips()
    {
        GameObject[] spawnedChips = new GameObject[_chipCount];

        for (int i = 0; i < _chipCount; i++)
        {
            GameObject chip = PoolManager.Instance.GetObjectFromPool<ChipReward>();
            chip.transform.SetParent(_chipParent, false);

            RectTransform chipTransform = chip.GetComponent<RectTransform>();
            chipTransform.localPosition = Vector3.zero;
            chipTransform.localRotation = Quaternion.identity;
            chipTransform.localScale = Vector3.one;

            Vector2 randomOffset = new Vector2(
                Random.Range(-_spawnRadius, _spawnRadius),
                Random.Range(-_spawnRadius, _spawnRadius)
            );

            chipTransform.localPosition = ConvertWorldToLocal(_spawnPosition.position + (Vector3)randomOffset);
            spawnedChips[i] = chip;

            StartCoroutine(ApplyPunchEffect(chipTransform));
        }

        yield return _preMoveDelayWait;

        foreach (GameObject chip in spawnedChips)
        {
            StartCoroutine(AnimateChipMovement(chip.GetComponent<RectTransform>()));
        }
    }

    private IEnumerator ApplyPunchEffect(RectTransform chipTransform)
    {
        float duration = _preMoveDelay;
        float elapsedTime = 0f;
        Vector3 initialScale = Vector3.one * 0.8f;
        Vector3 punchScale = Vector3.one * 1.2f;
        Vector3 targetScale = Vector3.one;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime * 2f;
            float progress = Mathf.Sin(elapsedTime * Mathf.PI);
            chipTransform.localScale = Vector3.Lerp(initialScale, punchScale, progress);
            yield return null;
        }

        chipTransform.localScale = targetScale;
    }

    private Vector2 ConvertWorldToLocal(Vector3 worldPosition)
    {
        if (_uiCamera == null || _chipParent == null)
            return Vector2.zero;

        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(_uiCamera, worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _chipParent, screenPosition, _uiCamera, out Vector2 localPosition);

        return localPosition;
    }

    private IEnumerator AnimateChipMovement(RectTransform chipTransform)
    {
        Vector2 startPosition = chipTransform.localPosition;
        Vector2 endPosition = ConvertWorldToLocal(_destinationPosition.position);
        float elapsedTime = 0f;

        while (elapsedTime < _movementDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / _movementDuration;
            progress = _movementCurve.Evaluate(progress);

            chipTransform.localPosition = Vector2.Lerp(startPosition, endPosition, progress);
            yield return null;
        }

        chipTransform.localPosition = endPosition;
        chipTransform.GetComponent<ChipReward>().Deactivate();

        _completedChips++;
        if (_completedChips == _chipCount)
        {
            OnChipMovementCompleted?.Invoke();
        }
    }
}