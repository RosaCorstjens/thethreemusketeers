using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using Object = UnityEngine.Object;

public class Cell
{
    static List<Vector2> directions = new List<Vector2>();

    public int X { get; private set; }
    public int Y { get; private set; }

    private List<Cell> neighbors;
    public List<Cell> Neighbors { get { return neighbors; } }

    private List<WorldObject> objects;
    public List<WorldObject> Objects { get { return objects; } }

    public bool Occupied { get { return objects.Count == 0 ? false : true; } }

    public Cell()
    {
        X = Y = 0;
        objects = new List<WorldObject>();
    }

    public Cell(int x, int y)
    {
        X = x;
        Y = y;
        neighbors = new List<Cell>();
        objects = new List<WorldObject>();

        GameObject.Instantiate(ItemManager.Instance.ItemGenerator.DropPrefab, DungeonManager.Instance.SpatialPartitionGrid.GridToWorld(X, Y), Quaternion.identity);
    }

    public void AddObject(WorldObject obj)
    {
        objects.Add(obj);
    }

    public void RemoveObject(WorldObject obj)
    {
        objects.Remove(obj);
    }

    public void FindNeighbors()
    {
        if(directions.Count == 0)
        {
            directions.Add(new Vector2(1, 0));
            directions.Add(new Vector2(-1, 0));
            directions.Add(new Vector2(0, 1));
            directions.Add(new Vector2(0, -1));
            directions.Add(new Vector2(1, 1));
            directions.Add(new Vector2(-1, -1));
            directions.Add(new Vector2(-1, 1));
            directions.Add(new Vector2(1, -1));
        }

        for (int i = 0; i < directions.Count; i++)
        {
            Cell possNeighbour = DungeonManager.Instance.SpatialPartitionGrid.GetCellAt(X + Mathf.RoundToInt(directions[i].x),
                    Y + Mathf.RoundToInt(directions[i].y));
            if (possNeighbour != null)
            {
                neighbors.Add(possNeighbour);
            }
        }

        neighbors.Add(this);
    }
}
