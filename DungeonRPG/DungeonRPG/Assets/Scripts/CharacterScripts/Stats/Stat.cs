using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class StatRange
{
    [SerializeField]
    private StatTypes statType;
    public StatTypes StatType { get { return statType; } set { statType = value; } }

    [SerializeField]
    private FloatRange range;
    public FloatRange Range { get { return range; } set { range = value; } }
}

[Serializable]
public class Stat
{
    [SerializeField]
    private StatTypes statType;
    public StatTypes StatType { get { return statType; } set { statType = value; } }

    [SerializeField]
    private float value;
    public float Value { get { return value; } set { this.value = value; } }

    public Stat(StatTypes statType, float value)
    {
        this.statType = statType;
        this.value = value;
    }

    public Stat() { }
}