using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderUpgrade : ISpiderWeapon
{
    private ISpiderWeapon spider;

    public SpiderUpgrade(ISpiderWeapon spiderWeapon)
    {
        spider = spiderWeapon;
    }

    public virtual float GetAttackRange()
    {
        return spider.GetAttackRange();
    }

    public virtual float GetDamage()
    {
        return spider.GetDamage();
    }

    public virtual float GetSpeed()
    {
        return spider.GetSpeed();
    }

    public virtual float GetHealth()
    {
        return spider.GetHealth();
    }

    public virtual bool IsBase()
    {
        return false;
    }

    public ISpiderWeapon RemoveUpgrade()
    {
        return spider;
    }
}
