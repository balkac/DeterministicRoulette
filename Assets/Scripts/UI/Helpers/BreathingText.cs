using UnityEngine;

public class BreathingText : MonoBehaviour
{
    public float MinScale = 0.95f;
    public float MaxScale = 1.05f;
    public float Speed = 1.5f;

    private void Update()
    {
        float scale = Mathf.Lerp(MinScale, MaxScale, (Mathf.Sin(Time.time * Speed) + 1) / 2);
        transform.localScale = new Vector3(scale, scale, 1);
    }
}