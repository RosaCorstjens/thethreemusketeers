using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor 
{
    public int xPos, yPos;

    public enum Placement { Corner, Corridor, Edge, Mid }
    public Placement placement = Placement.Mid;

    public enum RoomType { Room, Corridor }
    public RoomType roomType = RoomType.Room;

    public List<Floor> Neighbours { get; private set; }

    List<DungeonDirection> wallPlaces;

    public bool visited = false;

    // TO DO: fix this. a floor can only belong to one of them, but you need to inherit them so you can ref here to a basic space class. 
    public Room myRoom;
    public Corridor myCorridor;

    public bool IsOccupied;

    public GameObject myGO;

    public void Initialize(int xPos, int yPos)
    {
        this.xPos = xPos;
        this.yPos = yPos;
    }

    public void DetermineNeighbours()
    {
        List<DungeonDirection> directions = GetDirectionsWithOrWithoutNeighbour(true);
        Neighbours = new List<Floor>();
        for (int i = 0; i < directions.Count; i++)
        {
            Neighbours.Add(GameManager.Instance.DungeonManager.CurrentDungeon.GetFloor((int)(xPos + GameManager.Instance.DungeonManager.directionValues[directions[i]].x), (int)(yPos + GameManager.Instance.DungeonManager.directionValues[directions[i]].y)));
        }

        // First check for being a corridorfloor, cuz of special case. 
        // A corridor usually has 2 neigbours, but not if he is the start or end. He can even have 4 neighbours!
        // One thing we do know is that a corridorfloor doesn't have only 1 neighbour, but nobody has.  
        if ((!CheckForNeighbourNorth() && !CheckForNeighbourSouth()) || (!CheckForNeighbourEast() && !CheckForNeighbourWest()))
        {
            placement = Placement.Corridor;
        }
        else if (Neighbours.Count == 3)
        {
            placement = Placement.Edge;
        }
        else if (Neighbours.Count == 2)
        {
            placement = Placement.Corner;
        }
    }

    public void PlaceWalls()
    {
        GameObject wallPrefab = GameManager.Instance.DungeonManager.WallPrefab;
        wallPlaces = GetDirectionsWithOrWithoutNeighbour(false);

        for (int i = 0; i < wallPlaces.Count; i++)
        {
            Vector3 position = GameManager.Instance.DungeonManager.GridToWorldPosition(new Vector2(xPos, yPos));
            position.y = 1.75f;
            float yRot = 0;

            switch (wallPlaces[i])
            {
                case DungeonDirection.North:
                    position.z += (GameManager.Instance.DungeonManager.WorldScaleZ / 2);
                    yRot = 180;
                    break;
                case DungeonDirection.East:
                    position.x += (GameManager.Instance.DungeonManager.WorldScaleX / 2);
                    yRot = -90;
                    break;
                case DungeonDirection.South:
                    position.z -= (GameManager.Instance.DungeonManager.WorldScaleZ / 2);
                    yRot = 0;
                    break;
                case DungeonDirection.West:
                    position.x -= (GameManager.Instance.DungeonManager.WorldScaleX / 2);
                    yRot = 90;
                    break;
            }

            GameObject go = GameObject.Instantiate(wallPrefab, position, Quaternion.Euler(0, yRot, 0)) as GameObject;
            go.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);
        }
    }

    private List<DungeonDirection> GetDirectionsWithOrWithoutNeighbour(bool with)
    {
        List<DungeonDirection> directions = new List<DungeonDirection>();

        if (CheckForNeighbourNorth() == with) directions.Add(DungeonDirection.North);

        if (CheckForNeighbourSouth() == with) directions.Add(DungeonDirection.South);

        if (CheckForNeighbourEast() == with) directions.Add(DungeonDirection.East);

        if (CheckForNeighbourWest() == with) directions.Add(DungeonDirection.West);

        return directions;
    }

    private bool CheckForNeighbourNorth()
    {
        return GameManager.Instance.DungeonManager.CurrentDungeon.IsOccupied(xPos, yPos + 1);
    }

    private bool CheckForNeighbourEast()
    {
        return GameManager.Instance.DungeonManager.CurrentDungeon.IsOccupied(xPos + 1, yPos);
    }

    private bool CheckForNeighbourSouth()
    {
        return GameManager.Instance.DungeonManager.CurrentDungeon.IsOccupied(xPos, yPos - 1);
    }

    private bool CheckForNeighbourWest()
    {
        return GameManager.Instance.DungeonManager.CurrentDungeon.IsOccupied(xPos - 1, yPos);
    }

    public Vector3 ObjectRotation()
    {
        float yRotation = 0;

        if (wallPlaces.Contains(DungeonDirection.East))
        {
            yRotation = -90;
        }
        else if (wallPlaces.Contains(DungeonDirection.North))
        {
            yRotation = 0;
        }
        else if (wallPlaces.Contains(DungeonDirection.South))
        {
            yRotation = 180;
        }
        else if (wallPlaces.Contains(DungeonDirection.West))
        {
            yRotation = 90;
        }
        else
        {
            yRotation = 0;
        }

        return new Vector3(0, yRotation, 0);
    }
}
