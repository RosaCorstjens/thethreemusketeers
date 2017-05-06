﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonManager 
{
    private DungeonManager() { }

    private static DungeonManager instance;
    public static DungeonManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DungeonManager();
                instance.Initialize();
            }
            return instance;
        }
    }

    public int columns = 1000;                                 // The number of columns on the board (how wide it will be).
    public int rows = 1000;                                    // The number of rows on the board (how tall it will be).
    public IntRange numRooms = new IntRange(15, 20);          // The range of the number of rooms there can be.
    public IntRange roomWidth = new IntRange(4, 7);          // The range of widths rooms can have.
    public IntRange roomHeight = new IntRange(4, 7);         // The range of heights rooms can have.
    public IntRange corridorLength = new IntRange(3, 10);     // The range of lengths corridors between rooms can have.
    public float minDistStartEnd = 50f;

    public const float TREASURE_ROOM_AMOUNT = 18f;
    public const float MONSTER_ROOM_AMOUNT = 80f;

    public enum TileType { Wall, Floor, }
    Color[] colors = { Color.black, Color.blue, Color.cyan, Color.gray, Color.green, Color.magenta, Color.red };

    // TO DO: Get the prefabs for the dungeon. 
    public GameObject FloorPrefab { get; private set; }
    public GameObject WallPrefab { get; private set; }
    public GameObject PillarPrefab { get; private set; }
    public GameObject CeilingPrefab { get; private set; }
    public GameObject PortalPrefab { get; private set; }

    public GameObject ChestPrefab { get; private set; }
    public GameObject SpiderPrefab { get; private set; }

    private GameObject levelParent;
    public GameObject LevelParent { get { return levelParent; } }

    public float WorldScaleX { get { return 3f; } }
    public float WorldScaleZ { get { return 3f; } }

    public Dictionary<DungeonDirection, Vector2> directionValues;

    // The dungeon we're now working with in generation.
    public Dungeon CurrentDungeon { get; private set; }
    // The dungeon that is active in game.
    public Dungeon ActiveDungeon { get; private set; }

    private int floor;
    public int Floor { get { return floor; } }

    public void Initialize()
    {
        levelParent = new GameObject("Level Parent");

        directionValues = new Dictionary<DungeonDirection, Vector2>();
        directionValues.Add(DungeonDirection.North, new Vector2(0, 1));
        directionValues.Add(DungeonDirection.East, new Vector2(1, 0));
        directionValues.Add(DungeonDirection.South, new Vector2(0, -1));
        directionValues.Add(DungeonDirection.West, new Vector2(-1, 0));

        GetPrefabs();

        floor = 1;

        StartNewDungeon();

        GameManager.Instance.ActiveCharacterInformation.PlayerController.Initialize(CurrentDungeon.StartPosition);
    }

    public void StartNewDungeon()
    {
        // TO DO: Set parameters to match the players level. 

        GetDungeon();

        ActiveDungeon = CurrentDungeon;

        // TO DO: wait till now to do all building stuff and active this shit!
        ActiveDungeon.BuildDungeon();

        //CurrentDungeon.GenerateDungeon(GameManager.Instance.ActiveCharacterInformation.Level, columns, rows, numRooms.GetRandomInRange(), minDistStartEnd);//GenerateDungeon();//GetNewDungeon(dungeonType);
    }

    private void GetDungeon()
    {
        CurrentDungeon = null;

        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        CurrentDungeon = new Dungeon();
        CurrentDungeon.GenerateDungeon(GameManager.Instance.ActiveCharacterInformation.Level, columns, rows, numRooms.GetRandomInRange(), minDistStartEnd);
    }

    private void ClearDungeon()
    {
        CurrentDungeon.ClearDungeon();
    }

    void GetPrefabs()
    {
        FloorPrefab = Resources.Load<GameObject>("Prefabs/Dungeon/Floor");
        WallPrefab = Resources.Load<GameObject>("Prefabs/Dungeon/WallPlane");
        PillarPrefab = Resources.Load<GameObject>("Prefabs/Dungeon/Pillar");
        CeilingPrefab = Resources.Load<GameObject>("Prefabs/Dungeon/Ceiling");
        PortalPrefab = Resources.Load<GameObject>("Prefabs/Dungeon/Portal");
        SpiderPrefab = Resources.Load<GameObject>("Prefabs/Monsters/Spider"); 
        ChestPrefab = Resources.Load<GameObject>("Prefabs/Props/LootChest"); 
    }

    public Vector2 WorldToGridPosition(Vector3 worldPosition)
    {
        Vector2 returnVector = new Vector2();
        returnVector.x = worldPosition.x / WorldScaleX;
        returnVector.y = worldPosition.z / WorldScaleZ;

        return returnVector;
    }

    public Vector3 GridToWorldPosition(Vector2 gridPosition)
    {
        Vector3 returnVector = new Vector3();
        returnVector.x = gridPosition.x * WorldScaleX;
        returnVector.z = gridPosition.y * WorldScaleZ;

        return returnVector;
    }

    public void FinishDungeon()
    {
        // Increase floor. 
        floor++;

        // Clear current dungeon.
        ClearDungeon();

        // Start next.
        StartNewDungeon();

        // Respawn player.
        UIManager.Instance.NextDungeon();
        GameManager.Instance.StartCoroutine(GameManager.Instance.ActiveCharacterInformation.PlayerController.Respawn(ActiveDungeon.StartPosition));
    }

    public static Room SmallestRoomInList(List<Room> rooms)
    {
        Room returnRoom = rooms[0];
        int smallest = rooms[0].Floors.Count;
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].Floors.Count < smallest)
            {
                // New smallest found.
                smallest = rooms[i].Floors.Count;
                returnRoom = rooms[i];
            }
        }

        return returnRoom;
    }

    public static Room BiggestRoomInList(List<Room> rooms)
    {
        Room returnRoom = rooms[0];
        int biggest = rooms[0].Floors.Count;
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].Floors.Count > biggest)
            {
                // New biggest found.
                biggest = rooms[i].Floors.Count;
                returnRoom = rooms[i];
            }
        }

        return returnRoom;
    }
}
