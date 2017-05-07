using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRunCommand : Command<PlayerController> 
{
    public override void Execute(PlayerController controller)
    {
        controller.SwitchRun();
    }
}
