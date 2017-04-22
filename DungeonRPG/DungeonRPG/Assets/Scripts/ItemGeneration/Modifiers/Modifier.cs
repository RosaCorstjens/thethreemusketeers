using UnityEngine;
using System.Collections;
using System;

public enum ModifierType { add, mult }

[Serializable]
public class Modifier
{
    [SerializeField]
    public StatTypes affected;
    [SerializeField]
    public ModifierType modifierType;
    [SerializeField]
    public float value;

    public Modifier() { }

    public Modifier(StatTypes affected, ModifierType modifierType, float value)
    {
        this.affected = affected;
        this.modifierType = modifierType;
        this.value = value;
    }
}