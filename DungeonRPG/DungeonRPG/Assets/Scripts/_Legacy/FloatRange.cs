using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class FloatRange
{
    //min and max values of the total range 
    [SerializeField]
    public float min;
    [SerializeField]
    public float max;

    public FloatRange() { }

    //constructor to assign min and max values
    public FloatRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    //methode to get a random value from the total range, mostly called in start methode of a state to assign new random values for an attack
    public float GetRandomInRange()
    {
        return UnityEngine.Random.Range(min, max);
    }
}
