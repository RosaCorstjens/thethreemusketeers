using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCommand<T> : Command<T> where T : IControlable
{
    public override void Execute(T controller, float input)
    {
        controller.Turn(input);
    }
}
