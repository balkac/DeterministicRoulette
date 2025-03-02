using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool<T> : MonoBehaviour where T : PoolObject
{
    [SerializeField] private T _prefab;

    public T Prefab => _prefab;

    [SerializeField] private int _poolSize;

    private List<T> _pool;

    private void Awake()
    {
        InitializePool();
        PoolManager.Instance.AddPool(this);
    }

    private void OnDestroy()
    {
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.RemovePool(this);
        }

        foreach (var poolObject in _pool.Where(poolObject => poolObject != null))
        {
            Destroy(poolObject.gameObject);
        }
    }

    private void InitializePool()
    {
        _pool = new List<T>();

        for (int i = 0; i < _poolSize; i++)
        {
            T poolObject = Instantiate(_prefab, PoolManager.Instance.transform);
            poolObject.Deactivate();
            _pool.Add(poolObject);
        }
    }

    public GameObject GetObject()
    {
        foreach (T poolObject in _pool)
        {
            if (!poolObject.IsActive)
            {
                poolObject.Activate();
                return poolObject.gameObject;
            }
        }

        return null;
    }

    public bool Contains(T poolObject)
    {
        return _pool.Contains(poolObject);
    }

    public void ReturnObject(T poolObject)
    {
        poolObject.Deactivate();
    }
}