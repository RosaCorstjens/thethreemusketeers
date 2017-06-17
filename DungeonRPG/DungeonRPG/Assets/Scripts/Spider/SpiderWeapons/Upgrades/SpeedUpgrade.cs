using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpgrade : SpiderUpgrade
{
    private float moveSpeed = 0;

    public SpeedUpgrade(ISpiderWeapon spiderWeapon, float moveSpeed) : base(spiderWeapon)
    {
        this.moveSpeed = moveSpeed;
    }

    public override float GetSpeed()
    {
        return base.GetSpeed() + moveSpeed;
    }
}
