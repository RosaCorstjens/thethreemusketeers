using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    protected Vector3 previousPos;
    public Vector3 PreviousPos { get { return previousPos; } }
    protected Vector3 pos;
    public Vector3 Pos { get { return pos; } }

    public static float Epsilon = 1f;

    public virtual void Update()
    {
        pos = transform.position;
        if (previousPos == Vector3.zero) previousPos = pos;

        if (Mathf.Abs((previousPos - pos).magnitude) > Epsilon)
        {
            UpdateGrid();
            previousPos = pos;
        }
    }

    protected virtual void UpdateGrid()
    {
        DungeonManager.Instance.SpatialPartitionGrid.UpdateObjectInGrid(this);
    }

    protected virtual void AddToGrid()
    {
        DungeonManager.Instance.SpatialPartitionGrid.AddObjectToGrid(this);
    }
}
