using System;
using System.Collections.Generic;
using UnityEngine;

public interface IControlable
{
    void Update();
    void Turn(float input);
    void Move(float input, string animation, Vector3 direction);
    void Attack();
}

