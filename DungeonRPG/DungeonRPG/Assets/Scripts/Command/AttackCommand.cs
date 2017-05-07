using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand<T> : Command<T> where T : IControlable
{
    public override void Execute(T controller)
    {
        controller.Attack();
    }
}
