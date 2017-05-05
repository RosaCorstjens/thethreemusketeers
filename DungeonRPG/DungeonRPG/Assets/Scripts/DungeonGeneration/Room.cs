using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room
{
    public enum RoomType { Start, End, Treasure, Shop, Miniboss, Default }

    private RoomType type;
    public RoomType Type { get { return type; } set { type = value; } }

    private List<Floor> floors;
    public List<Floor> Floors { get { return floors; } set { floors = value; } }

    private List<Corridor> connectedCorridors;
    public List<Corridor> ConnectedCorridors { get { return connectedCorridors; } }

    public void SetConnectedCorridors(List<Corridor> connectedCorridors)
    {
        this.connectedCorridors = connectedCorridors;
        FindOuterTiles();
    }

    public void BuildRoom()
    {
        if (type != RoomType.Miniboss) type = RoomType.Default;

        FindOuterTiles().HandleAction(f => f.PlaceWalls());
    }

    private List<Floor> FindOuterTiles()
    {
        List<Floor> outerTiles = new List<Floor>();

        outerTiles.AddRange(floors.FindAll(f => f.placement == Floor.Placement.Corner));
        outerTiles.AddRange(floors.FindAll(f => f.placement == Floor.Placement.Edge));

        return outerTiles;
    }

    public Floor RandomFloorInRoom()
    {
        int randomFloor = Random.Range(0, floors.Count);

        return floors[randomFloor];
    }

    public float SmallestDistBetween(Room otherRoom)
    {
        float shortestDistance = 100;

        for (int i = 0; i < otherRoom.Floors.Count; i++)
        {
            for (int j = 0; j < floors.Count; j++)
            {
                Vector2 otherPos = new Vector2(otherRoom.Floors[i].xPos, otherRoom.Floors[i].yPos);
                Vector2 myPos = new Vector2(floors[j].xPos, floors[j].yPos);

                if ((otherPos - myPos).magnitude < shortestDistance)
                {
                    shortestDistance = (GameManager.Instance.DungeonManager.GridToWorldPosition(new Vector2(otherRoom.Floors[i].xPos, otherRoom.Floors[i].yPos)) - GameManager.Instance.DungeonManager.GridToWorldPosition(new Vector2(floors[j].xPos, floors[j].yPos))).magnitude;
                }
            }
        }

        return shortestDistance;
    }

    public void Destroy()
    {
        floors.HandleAction(f => f.Destroy());
        floors.Clear();

        connectedCorridors.Clear();
    }
}

