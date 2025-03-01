using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
    public Transform RotatePoint;
    public Vector3 Axis = Vector3.up;
    public float MinAngularSpeed = 60f;
    public float MaxAngularSpeed = 240f;
    public float MinSpinDuration = 3f;
    public float MaxSpinDuration = 5f;
    public float DistanceThreshold = 0.15f;
    
    private bool _spinning = false;
    private Coroutine _spinCoroutine;
    private Transform _initialParent;
    private Vector3 _initialLocalPosition;
    private float _angularSpeed;
    private void Awake()
    {
        _initialParent = transform.parent;
        _initialLocalPosition = transform.localPosition;
        _angularSpeed = MaxAngularSpeed;
    }

    private void Update()
    {
        if (_spinning)
        {
            transform.RotateAround(RotatePoint.position, Axis, _angularSpeed * Time.deltaTime);
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

        _angularSpeed = MaxAngularSpeed;
        _spinning = true;
        _spinCoroutine = StartCoroutine(SpinAndStop(resultData));
    }

    private IEnumerator SpinAndStop(WheelNumberData resultData)
    {
        float elapsedTime = 0f;

        while (elapsedTime < MaxSpinDuration)
        {
            elapsedTime += Time.deltaTime;
            _angularSpeed = Mathf.Max(Mathf.Lerp(MaxAngularSpeed, 0, elapsedTime / MaxSpinDuration), MinAngularSpeed);
            // Debug.Log($"Angular speed: {_angularSpeed}");
            yield return null;
            
            // Check distance condition
            float distance = Vector3.Distance(transform.position, resultData.NumberTransform.position);
            // Debug.Log($"Distance to target: {distance}");
            if (distance < DistanceThreshold && elapsedTime >= MinSpinDuration)
            {
                Debug.Log("Distance condition met");
                break;
            }

            // Check angle condition
            float angle = Vector3.Angle(transform.forward, resultData.NumberTransform.forward);
            // Debug.Log($"Angle to target: {angle}");
        }

        _spinning = false;
        transform.SetParent(resultData.NumberTransform);

        // Jump parameters
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = new Vector3(0f, -0.8f, 0f);
        float jumpHeight = 1f;
        float jumpDuration = 0.5f;
        int jumpCount = 3;

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
    }
}