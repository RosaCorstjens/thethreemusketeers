using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCommand : Command
{
    public override void Execute(PlayerController player, float input)
    {
        player.Turn(input);
    }
}
