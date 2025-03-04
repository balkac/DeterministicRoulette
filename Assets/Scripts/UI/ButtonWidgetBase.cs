using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ButtonWidgetBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private bool _canAnimate = true;
    [SerializeField] private float _animationDuration = 0.125f;
    [SerializeField] private Vector3 _targetScale = new Vector3(0.95f, 0.95f, 0.95f);

    public abstract void OnPointerDown(PointerEventData eventData);
    public abstract void OnPointerUp(PointerEventData eventData);

    private Vector3 _initialScale;
    protected bool _isPointerDown = false;
    
    protected virtual void Awake()
    {
        _initialScale = transform.localScale;
        AwakeCustomActions();
    }

    protected virtual void AwakeCustomActions()
    {
    }

    protected virtual void Update()
    {
        if (!_canAnimate)
        {
            return;
        }

        if (_isPointerDown)
        {
            transform.localScale =
                Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime / _animationDuration);
        }
        else
        {
            transform.localScale =
                Vector3.Lerp(transform.localScale, _initialScale, Time.deltaTime / _animationDuration);
        }
    }
}