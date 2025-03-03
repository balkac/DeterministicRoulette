using System;
using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
    [SerializeField] private Transform _rotatePoint;
    [SerializeField] private Vector3 _axis = Vector3.up;
    [SerializeField] private float _minAngularSpeed = 60f;
    [SerializeField] private float _maxAngularSpeed = 240f;
    [SerializeField] private float _minSpinDuration = 5f;

    private float _distanceThreshold = 0.15f;
    private bool _spinning = false;
    private Coroutine _spinCoroutine;
    private Transform _initialParent;
    private Vector3 _initialLocalPosition;
    private float _angularSpeed;

    public Action OnBallSpinCompleted;

    private void Awake()
    {
        _initialParent = transform.parent;
        _initialLocalPosition = transform.localPosition;
        _angularSpeed = _maxAngularSpeed;
    }

    private void Update()
    {
        if (_spinning)
        {
            transform.RotateAround(_rotatePoint.position, _axis, _angularSpeed * Time.deltaTime);
        }
    }

    public void StartSpin(WheelNumberData resultData)
    {
        transform.SetParent(_initialParent);
        transform.localPosition = _initialLocalPosition;
        if (_spinCoroutine != null)
        {
            StopCoroutine(_spinCoroutine);
        }

        _angularSpeed = _maxAngularSpeed;
        _spinning = true;
        _spinCoroutine = StartCoroutine(SpinAndStop(resultData));
    }

    private IEnumerator SpinAndStop(WheelNumberData resultData)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _minSpinDuration)
        {
            elapsedTime += Time.deltaTime;
            _angularSpeed = Mathf.Lerp(_maxAngularSpeed, _minAngularSpeed, elapsedTime / _minSpinDuration);
            yield return null;

            // Check angle condition
            // float angle = Vector3.Angle(transform.forward, resultData.NumberTransform.forward);
        }

        elapsedTime = 0f;

        while (elapsedTime < _minSpinDuration)
        {
            elapsedTime += Time.deltaTime;

            // Check distance condition
            float distance = Vector3.Distance(transform.position, resultData.NumberTransform.position);
            Debug.Log($"Distance to target: {distance}");

            // Normalize distance to control angular speed smoothly

            if (distance - _distanceThreshold > _distanceThreshold)
            {
                yield return null;
            }

            float nextSpeed = Mathf.Lerp(_minAngularSpeed, 0, 1 / (distance / _distanceThreshold));

            if (nextSpeed < _angularSpeed)
            {
                _angularSpeed = Mathf.Max(_angularSpeed - 0.1f, nextSpeed);
            }

            if (distance < _distanceThreshold)
            {
                Debug.Log("Distance condition met");
                break;
            }

            yield return null;
        }


        _spinning = false;
        transform.SetParent(resultData.NumberTransform);

        // Jump parameters
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = new Vector3(0f, -0.8f, 0f);
        float jumpHeight = 1f;
        float jumpDuration = 0.5f;
        int jumpCount = 2;

        for (int i = 0; i < jumpCount; i++)
        {
            float jumpElapsedTime = 0f;

            while (jumpElapsedTime < jumpDuration)
            {
                jumpElapsedTime += Time.deltaTime;
                float t = jumpElapsedTime / jumpDuration;
                float height = Mathf.Sin(Mathf.PI * t) * jumpHeight;
                transform.localPosition = Vector3.Lerp(startPosition, endPosition, t) + new Vector3(0f, height, 0f);
                yield return null;
            }

            transform.localPosition = endPosition;
            startPosition = endPosition;
            endPosition = new Vector3(0f, -0.8f, 0f); // Reset end position for the next jump
        }

        // Move to final position with jumps
        Vector3 finalPosition = new Vector3(0f, -0.8f, 0f);
        float moveDuration = 1f;
        float moveElapsedTime = 0f;

        while (moveElapsedTime < moveDuration)
        {
            moveElapsedTime += Time.deltaTime;
            float t = moveElapsedTime / moveDuration;
            float height = Mathf.Sin(Mathf.PI * t) * jumpHeight;
            transform.localPosition = Vector3.Lerp(startPosition, finalPosition, t) + new Vector3(0f, height, 0f);
            yield return null;
        }

        transform.localPosition = finalPosition;
        OnBallSpinCompleted?.Invoke();
    }
}