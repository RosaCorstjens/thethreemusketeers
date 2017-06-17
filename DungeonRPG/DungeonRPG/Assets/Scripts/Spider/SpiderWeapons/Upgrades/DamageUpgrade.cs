using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpgrade : SpiderUpgrade
{
    private float damage = 0;

    public DamageUpgrade(ISpiderWeapon spiderWeapon, float damage) : base(spiderWeapon)
    {
        this.damage = damage;
    }

    public override float GetDamage()
    {
        return base.GetDamage() + damage;
    }
}
