using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Singleton<T> where T : ISingletonInstance, new()
{
    protected Singleton() { }

    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}

public interface ISingletonInstance
{
    void Initialize();
}
