using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
    public Transform RotatePoint;
    public Vector3 Axis = Vector3.up;
    public float AngularSpeed = 150f;
    public float SpinDuration = 5f;

    private bool _spinning = false;
    private Coroutine _spinCoroutine;
    private Transform _initialParent;
    private Vector3 _initialLocalPosition;

    private void Awake()
    {
        _initialParent = transform.parent;
        _initialLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (_spinning)
        {
            transform.RotateAround(RotatePoint.position, Axis, AngularSpeed * Time.deltaTime);
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

        _spinning = true;
        _spinCoroutine = StartCoroutine(SpinAndStop(resultData));
    }

    private IEnumerator SpinAndStop(WheelNumberData resultData)
    {
        float elapsedTime = 0f;
        float initialSpeed = AngularSpeed;

        while (elapsedTime < SpinDuration)
        {
            elapsedTime += Time.deltaTime;
            AngularSpeed = Mathf.Lerp(initialSpeed, 0, elapsedTime / SpinDuration);
            yield return null;
        }

        _spinning = false;
        transform.SetParent(resultData.NumberTransform);
        transform.localPosition = new Vector3(0f, -0.8f, 0f);
        AngularSpeed = initialSpeed;
    }
}