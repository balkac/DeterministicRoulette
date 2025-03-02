using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<Type, object> _pools = new Dictionary<Type, object>();

    public void AddPool<T>(Pool<T> pool) where T : PoolObject
    {
        _pools.Add(typeof(T), pool);
    }
    
    public void RemovePool<T>(Pool<T> pool) where T : PoolObject
    {
        _pools.Remove(typeof(T));
    }

    public GameObject GetObjectFromPool<T>() where T : PoolObject
    {
        if (_pools.TryGetValue(typeof(T), out object pool))
        {
            return ((Pool<T>)pool).GetObject();
        }

        Debug.LogError("No pool for type " + typeof(T));
        return null;
    }
}