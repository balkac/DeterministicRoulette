using UnityEngine;

public class Singleton<TSingleton> : MonoBehaviour
    where TSingleton : MonoBehaviour
{
    private static TSingleton _instance;

    public static TSingleton Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<TSingleton>();

            return _instance;
        }
    }

    protected virtual void AwakeCore()
    {
    }

    protected virtual void OnDestroyCore()
    {
    }

    private void Awake()
    {
        SetInstance();

        AwakeCore();
    }

    private void OnDestroy()
    {
        _instance = null;

        OnDestroyCore();
    }

    protected void SetInstance()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this as TSingleton;
    }
}