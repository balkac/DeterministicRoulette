using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private RouletteManager _rouletteManager;
    [SerializeField] private Vector3 _spinPosition;
    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private Quaternion _spinRotation;
    [SerializeField] private Quaternion _initialRotation;
    [SerializeField] private float _transitionDuration = 1f;

    private void OnEnable()
    {
        _rouletteManager.OnSpinStarted += HandleSpinStarted;
        _rouletteManager.OnSpinCompleted += HandleSpinCompleted;
    }

    private void OnDisable()
    {
        _rouletteManager.OnSpinStarted -= HandleSpinStarted;
        _rouletteManager.OnSpinCompleted -= HandleSpinCompleted;
    }

    private void HandleSpinStarted()
    {
        StartCoroutine(TransitionToPosition(_spinPosition, _spinRotation));
    }

    private void HandleSpinCompleted()
    {
        StartCoroutine(TransitionToPosition(_initialPosition, _initialRotation));
    }

    private IEnumerator TransitionToPosition(Vector3 targetPosition, Quaternion targetRotation)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < _transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _transitionDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }
}