using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class PoolingManager
{
    public enum TypesPooledObject
    {
        Spider,
        Chest,
        Floor,
        Wall
    }

    private SpiderPool spiderObjectPool = new SpiderPool();
    public SpiderPool SpiderObjectPool { get { return spiderObjectPool; } }

    public void Initialize()
    {
        Debug.Log("Initializing spider pool...");
        spiderObjectPool.Initialize();
    }

    public void Restart()
    {
        Initialize();
    }

    public void ClearPools()
    {
        spiderObjectPool.ClearPool();
    }
}
