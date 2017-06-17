using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpiderWeapon
{
    bool IsBase();
    ISpiderWeapon RemoveUpgrade();

    float GetAttackRange();
    float GetSpeed();
    float GetDamage();
    float GetHealth();
}
