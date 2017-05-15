using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEquipment : ISpiderWeapon
{
    private float moveSpeed = 1f;
    private int damage = 5;
    private float attackRange = 1.5f;
    private float health = 20f;

    public SpiderEquipment() { }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetSpeed()
    {
        return moveSpeed;
    }

    public virtual float GetHealth()
    {
        return health;
    }

    public bool IsBase()
    {
        return true;
    }

    public ISpiderWeapon RemoveUpgrade()
    {
        return this;
    }
}
