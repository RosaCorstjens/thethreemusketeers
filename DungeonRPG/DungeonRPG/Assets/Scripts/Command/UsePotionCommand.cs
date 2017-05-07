using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePotionCommand : Command<PlayerController>
{
    public override void Execute(PlayerController controller)
    {
        controller.UsePotion();
    }
}
