using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardCommand : Command {

    public override void Execute(PlayerController player, float input)
    {
        player.Move(input, "MoveMeForward", Vector3.forward);
    }
}
