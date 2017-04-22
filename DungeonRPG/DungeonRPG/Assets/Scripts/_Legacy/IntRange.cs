using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class IntRange
{
    //min and max values of the total range 
    [SerializeField]
    public int min;
    [SerializeField]
    public int max;

    public IntRange() { }

    //constructor to assign min and max values
    public IntRange(int _min, int _max)
    {
        min = _min;
        max = _max;
    }

    //methode to get a random value from the total range, mostly called in start methode of a state to assign new random values for an attack
    public int GetRandomInRange()
    {
        return UnityEngine.Random.Range(min, max);
    }
}
