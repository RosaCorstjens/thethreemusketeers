using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class ObjectPool <T> where T : class, IPoolable, new()
{
    protected T prefabObject;
    protected Queue<T> pooledObjects;

    [SerializeField]
    protected int initialBufferSize = 1;
    public int InitialBufferSize { get { return initialBufferSize; } }

    public virtual void Initialize()
    {
    }

    public virtual T New()
    {
        return null;
    }

    public void Store(T obj)
    {
        obj.Deactivate();
        pooledObjects.Enqueue(obj);
    }

    public void ClearPool()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            T unitToKill = pooledObjects.Dequeue();
            unitToKill.Destroy();
            i--;
        }

        pooledObjects.Clear();
        pooledObjects = null;
    }
}
