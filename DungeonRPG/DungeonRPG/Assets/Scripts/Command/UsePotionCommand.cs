using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePotionCommand : Command
{
    public override void Execute(PlayerController player)
    {
        player.UsePotion();
    }
}
