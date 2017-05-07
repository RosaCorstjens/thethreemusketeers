using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrafeCommand<T> : Command<T> where T : IControlable
{
    public override void Execute(T controller, float input)
    {
        controller.Move(input, "MoveMeSideways", Vector3.right);
    }
}
