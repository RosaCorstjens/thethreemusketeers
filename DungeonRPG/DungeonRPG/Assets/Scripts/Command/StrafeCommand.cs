using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrafeCommand : Command {

    public override void Execute(PlayerController player, float input)
    {
        player.Move(input, "MoveMeSideways", Vector3.right);
    }
}
