using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public bool IsActive { get; private set; }

    public void Activate()
    {
        IsActive = true;
        gameObject.SetActive(true);
        ActivateCustomActions();
    }

    public void Deactivate()
    {
        IsActive = false;
        gameObject.SetActive(false);
        if (PoolManager.Instance != null)
        {
            transform.SetParent(PoolManager.Instance.transform);
        }
        DeactivateCustomActions();
    }
    
    protected virtual void DeactivateCustomActions()
    {
    }
    
    protected virtual void ActivateCustomActions()
    {
    }
}