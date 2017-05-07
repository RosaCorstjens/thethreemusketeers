using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State <T>
{
    //called once when entering state
    public virtual void Enter(T agent) { }

    //called every frame
    public virtual void Execute(T agent) { }

    //called when leavin stae
    public virtual void Exit(T agent) { }
}
