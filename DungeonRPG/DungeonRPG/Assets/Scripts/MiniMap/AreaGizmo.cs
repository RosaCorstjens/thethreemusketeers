using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaGizmo : TriggerGizmo
{

    private GameObject gizmoObject;

    public override void Update()
    {
        base.Update();
        if (Triggered && !CheckTrigger())
        {
            hideGizmo();
        }
    }

    protected override GameObject ShowGizmo()
    {
        gizmoObject = base.ShowGizmo();
        return gizmoObject;
    }

    protected void hideGizmo()
    {
        Destroy(gizmoObject);
        Triggered = false;
    }
}
