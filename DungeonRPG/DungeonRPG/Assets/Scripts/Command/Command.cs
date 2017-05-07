using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command<T> where T : IControlable
{
    public virtual void Execute(T controller, float input = 0)
    {
        return;
    }

    public virtual void Execute(T controller)
    {
        return;
    }
}
