using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardCommand<T> : Command<T> where T : IControlable
{ 
    public override void Execute(T controller, float input)
    {
        controller.Move(input, "MoveMeForward", Vector3.forward);
    }
}
