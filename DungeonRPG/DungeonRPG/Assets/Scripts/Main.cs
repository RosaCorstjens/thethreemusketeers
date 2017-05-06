using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private static Main instance;
    public static Main Instance {
        get
        {
            if (instance == null) instance = FindObjectOfType<Main>();
            return instance;
        }
    }

	// Use this for initialization
    void Awake()
    {
        GameManager.Instance.Initialize();
    }
}
