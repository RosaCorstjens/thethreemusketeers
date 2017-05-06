using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRunCommand : Command
{
    public override void Execute(PlayerController player)
    {
        player.SwitchRun();
    }
}
