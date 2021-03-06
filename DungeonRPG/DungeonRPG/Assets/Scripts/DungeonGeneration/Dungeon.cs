﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dungeon
{
    public enum DungeonType { MiniBoss, Normal, }

    private DungeonType type;
    public DungeonType Type { get { return type; } }

    private DungeonManager dm;

    private DungeonManager.TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private BaseRoom[] baseRooms;                                     // All the rooms that are created for this board.
    private BaseCorridor[] baseCorridors;                             // All the corridors that connect the rooms.
    private List<Floor> floors;
    private List<GameObject> floorGos;

    private List<Room> rooms;
    public List<Room> Rooms { get { return rooms; } }
    private List<Corridor> corridors;

    private Vector3 startPosition;
    public Vector3 StartPosition { get { return startPosition; } }
    public Room StartRoom { get; private set; }
    public Room EndRoom { get; private set; }
    public List<Room> treasureRooms;
    public List<Room> monsterRooms;
    public Room bossRoom;

    private int dLevel;
    private int columns;
    private int rows;
    private int numRooms;
    private float minDistStartEnd;

    public void GenerateDungeon(int dLevel, int columns, int rows, int numRooms, float minDistStartEnd)
    {
        dm = GameManager.Instance.LevelManager.DungeonManager;

        this.dLevel = dLevel;
        this.columns = columns;
        this.rows = rows;
        this.numRooms = numRooms;
        this.minDistStartEnd = minDistStartEnd;

        SetupTilesArray();
        CreateRoomsAndCorridors();
        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();

        InstantiateFloors();
        RedefineRoomsAndCorridors();
        EvaluateDungeon();
    }

    public void BuildDungeon()
    {
        floorGos = new List<GameObject>();
        InstantiateFloorGameobjects();

        rooms.HandleAction(r => r.BuildRoom());
        corridors.HandleAction(c => c.BuildCorridor());

        DetermineRoomTypes();
    }

    private void SetupTilesArray()
    {
        // Set the tiles jagged array to the correct width.
        tiles = new DungeonManager.TileType[columns][];

        // Go through all the tile arrays...
        for (int i = 0; i < tiles.Length; i++)
        {
            // ... and set each tile array is the correct height.
            tiles[i] = new DungeonManager.TileType[rows];
        }
    }

    private void CreateRoomsAndCorridors()
    {
        // Create the rooms array with a random size.
        baseRooms = new BaseRoom[numRooms];

        // There should be one less corridor than there is rooms.
        baseCorridors = new BaseCorridor[baseRooms.Length - 1];

        // Create the first room and corridor.
        baseRooms[0] = new BaseRoom();
        baseCorridors[0] = new BaseCorridor();

        // Setup the first room, there is no previous corridor so we do not use one.
        baseRooms[0].SetupRoom(dm.roomWidth, dm.roomHeight, columns, rows);

        // Setup the first corridor using the first room.
        baseCorridors[0].SetupCorridor(baseRooms[0], dm.corridorLength, dm.roomWidth, dm.roomHeight, columns, rows, true);

        for (int i = 1; i < baseRooms.Length; i++)
        {
            // Create a room.
            baseRooms[i] = new BaseRoom();

            // Setup the room based on the previous corridor.
            baseRooms[i].SetupRoom(dm.roomWidth, dm.roomHeight, columns, rows, baseCorridors[i - 1]);

            // If we haven't reached the end of the corridors array...
            if (i < baseCorridors.Length)
            {
                // ... create a corridor.
                baseCorridors[i] = new BaseCorridor();

                // Setup the corridor based on the room that was just created.
                baseCorridors[i].SetupCorridor(baseRooms[i], dm.corridorLength, dm.roomWidth, dm.roomHeight, columns, rows, false);
            }
        }

    }

    void SetTilesValuesForRooms()
    {
        // Go through all the rooms...
        for (int i = 0; i < baseRooms.Length; i++)
        {
            BaseRoom currentRoom = baseRooms[i];

            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;

                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                    tiles[xCoord][yCoord] = DungeonManager.TileType.Floor;
                }
            }
        }
    }

    void SetTilesValuesForCorridors()
    {
        // Go through every corridor...
        for (int i = 0; i < baseCorridors.Length; i++)
        {
            BaseCorridor currentCorridor = baseCorridors[i];

            // and go through it's length.
            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {
                // Start the coordinates at the start of the corridor.
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                // Depending on the direction, add or subtract from the appropriate
                // coordinate based on how far through the length the loop is.
                switch (currentCorridor.direction)
                {
                    case DungeonDirection.North:
                        yCoord += j;
                        break;
                    case DungeonDirection.East:
                        xCoord += j;
                        break;
                    case DungeonDirection.South:
                        yCoord -= j;
                        break;
                    case DungeonDirection.West:
                        xCoord -= j;
                        break;
                }

                // Set the tile at these coordinates to Floor.
                tiles[xCoord][yCoord] = DungeonManager.TileType.Floor;
            }
        }
    }

    void InstantiateFloors()
    {
        floors = new List<Floor>();

        // Go through all the tiles in the jagged array...
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                if (tiles[i][j] == DungeonManager.TileType.Wall) continue;

                Floor newFloor = new Floor();
                newFloor.Initialize(i, j);
                floors.Add(newFloor);
            }
        }

        floors.HandleAction(f => f.DetermineNeighbours());
    }

    void InstantiateFloorGameobjects()
    {
        floors.HandleAction(f => floorGos.Add(InstantiateFromArray(dm.FloorPrefab, f.xPos, f.yPos)));
    }

    GameObject InstantiateFromArray(GameObject prefab, float xCoord, float yCoord)
    {
        // The position to be instantiated at is based on the coordinates.
        Vector3 position = dm.GridToWorldPosition(new Vector2(xCoord, yCoord));

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = GameObject.Instantiate(prefab, position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = GameManager.Instance.LevelManager.LevelParent.transform;

        return tileInstance;
    }

    void RedefineRoomsAndCorridors()
    {
        corridors = new List<Corridor>();
        rooms = new List<Room>();

        floors.FindAll(f => f.placement == Floor.Placement.Corridor).HandleAction(f => f.roomType = Floor.RoomType.Corridor);

        // Find corridors. 
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                // Get the floor to evaluate. 
                Floor floor = GetFloor(x, y);

                // If there is no floor at this spot, continue. 
                if (floor == null) continue;

                // Skip floors that are already grouped.
                if (floor.visited) continue;

                // Check for being the correct floortype. 
                if (floor.placement == Floor.Placement.Corridor)
                {
                    var corridor = new Corridor();
                    corridor.Floors = new List<Floor>();
                    PopulateGroup(corridor.Floors, floor, Floor.RoomType.Corridor);
                    corridor.Floors.HandleAction(f => f.myCorridor = corridor);

                    corridors.Add(corridor);
                }
            }
        }

        // Find rooms. 
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                // Get the floor to evaluate. 
                Floor floor = GetFloor(x, y);

                if (floor == null) continue;

                // Skip floors that are already grouped.
                if (floor.visited) continue;

                // Check for being the correct floortype. 
                if (floor.roomType == Floor.RoomType.Room)
                {
                    if (x > 0)
                    {
                        var neighbor = GetFloor(x - 1, y);
                        if (neighbor != null && neighbor.roomType == Floor.RoomType.Room)
                        {
                            rooms.Add(CreateRoom(floor));
                            continue;
                        }
                    }
                    if (x < columns)
                    {
                        var neighbor = GetFloor(x + 1, y);
                        if (neighbor != null && neighbor.roomType == Floor.RoomType.Room)
                        {
                            rooms.Add(CreateRoom(floor));
                            continue;
                        }
                    }
                    if (y > 0)
                    {
                        var neighbor = GetFloor(x, y - 1);
                        if (neighbor != null && neighbor.roomType == Floor.RoomType.Room)
                        {
                            rooms.Add(CreateRoom(floor));
                        }
                    }
                    if (y < rows)
                    {
                        var neighbor = GetFloor(x, y + 1);
                        if (neighbor != null && neighbor.roomType == Floor.RoomType.Room)
                        {
                            rooms.Add(CreateRoom(floor));
                        }
                    }
                }
            }
        }
        rooms.HandleAction(r => RedefineConnections(r));
    }

    // Recursively find connected floors (depth-first search)
    void PopulateGroup(List<Floor> group, Floor floor, Floor.RoomType roomType)
    {
        group.Add(floor);
        floor.visited = true;

        // Check all four neighbors and recurse on them if needed:
        if (floor.xPos > 0)
        {
            var neighbor = GetFloor(floor.xPos - 1, floor.yPos);
            if (neighbor != null && neighbor.roomType == floor.roomType && neighbor.visited == false)
                PopulateGroup(group, neighbor, roomType);
        }
        if (floor.xPos < columns - 1)
        {
            var neighbor = GetFloor(floor.xPos + 1, floor.yPos);
            if (neighbor != null && neighbor.roomType == floor.roomType && neighbor.visited == false)
                PopulateGroup(group, neighbor, roomType);
        }
        if (floor.yPos > 0)
        {
            var neighbor = GetFloor(floor.xPos, floor.yPos - 1);
            if (neighbor != null && neighbor.roomType == floor.roomType && neighbor.visited == false)
                PopulateGroup(group, neighbor, roomType);
        }
        if (floor.yPos < rows - 1)
        {
            var neighbor = GetFloor(floor.xPos, floor.yPos + 1);
            if (neighbor != null && neighbor.roomType == floor.roomType && neighbor.visited == false)
                PopulateGroup(group, neighbor, roomType);
        }
    }

    private Room CreateRoom(Floor floor)
    {
        var room = new Room();
        room.Floors = new List<Floor>();

        PopulateGroup(room.Floors, floor, Floor.RoomType.Room);

        room.Floors.HandleAction(f => { f.myRoom = room; });

        return room;
    }

    private void RedefineConnections(Room room)
    {
        List<Floor> corridorNeighbourFloors = new List<Floor>();
        List<Corridor> connectedCorridors = new List<Corridor>();

        // Find the connected corridor tiles.
        for (int i = 0; i < room.Floors.Count; i++)
        {
            corridorNeighbourFloors.AddRange(room.Floors[i].Neighbours.FindAll(n => n.placement == Floor.Placement.Corridor));
        }

        // Find the corridors that own the corridor tiles. 
        for (int i = 0; i < corridorNeighbourFloors.Count; i++)
        {
            if (connectedCorridors.Contains(corridorNeighbourFloors[i].myCorridor)) continue;
            connectedCorridors.Add(corridorNeighbourFloors[i].myCorridor);
        }

        room.SetConnectedCorridors(connectedCorridors);
    }

    private bool ApproveCorridor(Corridor corridor)
    {
        if (corridor.Floors.Count <= 1)
        {
            corridor.Floors.HandleAction(f => { f.roomType = Floor.RoomType.Room; f.visited = false; });

            return false;
        }
        return true;
    }

    private void DetermineStartAndEnd()
    {
        // Ideally a start room only has one corridor, check for this possibility. 
        List<Room> options = rooms.FindAll(r => r.ConnectedCorridors.Count <= 1);

        // There can be a start room with only one connection. 
        if (options.Count > 0)
        {
            StartRoom = options[0];
            options.RemoveAt(0);

            // If the option list is still not empty, evaluate the fitness of the room to serve as end room. 
            if (options.Count > 0)
            {
                if (options[0].SmallestDistBetween(StartRoom) > minDistStartEnd) EndRoom = options[0];
            }
        }
        // If not, the start room must be a small room with no more than two connections. 
        else
        {
            options.Clear();
            options = rooms.FindAll(r => r.ConnectedCorridors.Count <= 2);

            // Find the smallest from the new options
            StartRoom = DungeonManager.SmallestRoomInList(options);

            options.Remove(StartRoom);

            // Find the rooms far away enough from the start room. 
            options = options.FindAll(r => r.SmallestDistBetween(StartRoom) > minDistStartEnd);

            // If there are no rooms far away enough... We have a problem
            if (options.Count == 0) Debug.LogError("No rooms far away enough from start to function as end room!");

            EndRoom = DungeonManager.SmallestRoomInList(options);
        }

        // In case the start room only has one connection, but there can't be a endroom with one. 
        if (EndRoom == null)
        {
            options = rooms.FindAll(r => r.ConnectedCorridors.Count <= 2);
            options = options.FindAll(r => r.SmallestDistBetween(StartRoom) > minDistStartEnd);
            options.Remove(StartRoom);

            EndRoom = DungeonManager.SmallestRoomInList(options);
        }

        StartRoom.Type = Room.RoomType.Start;
        EndRoom.Type = Room.RoomType.End;

        List<Floor> cornerFloors = StartRoom.Floors.FindAll(f => f.placement == Floor.Placement.Corner);
        Floor startFloor = cornerFloors[Random.Range(0, cornerFloors.Count)];
        startPosition = dm.GridToWorldPosition(new Vector2(startFloor.xPos, startFloor.yPos));

        List<Floor> edgeFloors = EndRoom.Floors.FindAll(f => f.placement == Floor.Placement.Edge);
        Floor endFloor = edgeFloors[Random.Range(0, edgeFloors.Count)];
        GameObject portal = GameObject.Instantiate(dm.PortalPrefab);
        portal.transform.SetParent(GameManager.Instance.LevelManager.LevelParent.transform);
        portal.transform.position = dm.GridToWorldPosition(new Vector2(endFloor.xPos, endFloor.yPos));
        portal.transform.position += new Vector3(0, 1.75f, 0);
        portal.transform.localEulerAngles = endFloor.ObjectRotation();

        PortalScript portalScript = portal.GetComponent<PortalScript>();
        portalScript.Initialze();

        StartRoom.Floors.HandleAction(f => SetTestColor(f.xPos, f.yPos, Color.red));
        EndRoom.Floors.HandleAction(f => SetTestColor(f.xPos, f.yPos, Color.blue));
    }

    private void DetermineRoomTypes()
    {
        // Sneaky, sneaky, I already defined the boss room.. 

        DetermineStartAndEnd();

        List<Room> availableRooms = rooms.FindAll(r => r.Type == Room.RoomType.Default);

        // Smallest x % will be treasure rooms.
        int amountTreasureRooms = (int)((availableRooms.Count / 100f) * DungeonManager.TREASURE_ROOM_AMOUNT);

        treasureRooms = new List<Room>();

        while (treasureRooms.Count < amountTreasureRooms)
        {
            treasureRooms.Add(DungeonManager.SmallestRoomInList(availableRooms));
            availableRooms.Remove(treasureRooms.Last());
            treasureRooms.Last().Floors.HandleAction(f => SetTestColor(f.xPos, f.yPos, Color.yellow));
        }

        // Do the same for monster rooms.
        int amountMonsterRooms = (int)((availableRooms.Count / 100f) * DungeonManager.MONSTER_ROOM_AMOUNT);

        monsterRooms = new List<Room>();

        while (monsterRooms.Count < amountMonsterRooms)
        {
            monsterRooms.Add(DungeonManager.BiggestRoomInList(availableRooms));
            availableRooms.Remove(monsterRooms.Last());
            monsterRooms.Last().Floors.HandleAction(f => SetTestColor(f.xPos, f.yPos, Color.green));
        }

    }

    private void EvaluateDungeon()
    {
        // Check for being a possible miniboss dungeon. 
        Room biggestRoom = DungeonManager.BiggestRoomInList(rooms);

        if(biggestRoom.Floors.Count > DungeonManager.MIN_SIZE_BOSS_ROOM)
        {
            bossRoom = biggestRoom;
            bossRoom.Type = Room.RoomType.Miniboss;
            type = DungeonType.MiniBoss;
        }
        else
        {
            type = DungeonType.Normal;
        }
    }

    public Floor GetFloor(int xPos, int yPos)
    {
        return floors.Find(f => f.xPos == xPos && f.yPos == yPos);
    }

    public bool IsOccupied(int xPos, int yPos)
    {
        if (tiles[xPos][yPos] == DungeonManager.TileType.Wall) return false;

        return true;
    }

    public void SetTestColor(int xPos, int yPos, Color color)
    {
        Vector3 worldPos = dm.GridToWorldPosition(new Vector2(xPos, yPos));

        floorGos.Find(go => go.transform.position.x == worldPos.x && go.transform.position.z == worldPos.z).GetComponent<Renderer>().material.color = color;
    }
}
