using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    public virtual void Execute(PlayerController player, float input = 0)
    {
        return;
    }

    public virtual void Execute(PlayerController player)
    {
        return;
    }
}
