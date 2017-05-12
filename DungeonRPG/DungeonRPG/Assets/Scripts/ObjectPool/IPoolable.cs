using UnityEngine;
using System.Collections;

public interface IPoolable
{ 
    void Initialize();
    void Activate();
    void Deactivate();
    void Destroy();
}
