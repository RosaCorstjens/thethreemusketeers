using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : SpiderUpgrade
{
    private float health = 0;

    public HealthUpgrade(ISpiderWeapon spiderWeapon, float health) : base(spiderWeapon)
    {
        this.health = health;
    }

    public override float GetHealth()
    {
        return base.GetHealth() + health;
    }
}
