using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonManager : MonoBehaviour
{
    public int columns = 1000;                                 // The number of columns on the board (how wide it will be).
    public int rows = 1000;                                    // The number of rows on the board (how tall it will be).
    public IntRange numRooms = new IntRange(15, 20);          // The range of the number of rooms there can be.
    public IntRange roomWidth = new IntRange(4, 7);          // The range of widths rooms can have.
    public IntRange roomHeight = new IntRange(4, 7);         // The range of heights rooms can have.
    public IntRange corridorLength = new IntRange(3, 10);     // The range of lengths corridors between rooms can have.
    public float minDistStartEnd = 50f;

    public const int MIN_SIZE_BOSS_ROOM = 60;
    public const int MINIBOSS_INTERVAL = 5;
    public const float TREASURE_ROOM_AMOUNT = 20f;
    public const float MONSTER_ROOM_AMOUNT = 75f;

    public enum TileType { Wall, Floor, }
    Color[] colors = { Color.black, Color.blue, Color.cyan, Color.gray, Color.green, Color.magenta, Color.red };

    // TO DO: Get the prefabs for the dungeon. 
    public GameObject FloorPrefab { get; private set; }
    public GameObject WallPrefab { get; private set; }
    public GameObject PillarPrefab { get; private set; }
    public GameObject CeilingPrefab { get; private set; }
    public GameObject PortalPrefab { get; private set; }

    public float WorldScaleX { get { return 3f; } }
    public float WorldScaleZ { get { return 3f; } }

    public Dictionary<DungeonDirection, Vector2> directionValues;

    // The dungeon we're now working with in generation.
    public Dungeon CurrentDungeon { get; private set; }
    // The dungeon that is active in game.
    public Dungeon ActiveDungeon { get; private set; }
    private DungeonPool dungeonPool;

    private int floor;
    public int Floor { get { return floor; } }

    public void Initialize()
    {       
        directionValues = new Dictionary<DungeonDirection, Vector2>();
        directionValues.Add(DungeonDirection.North, new Vector2(0,1));
        directionValues.Add(DungeonDirection.East, new Vector2(1,0));
        directionValues.Add(DungeonDirection.South, new Vector2(0,-1));
        directionValues.Add(DungeonDirection.West, new Vector2(-1,0));

        dungeonPool = new DungeonPool();
        dungeonPool.Initialize();

        GetPrefabs();

        floor = 1;

        StartNewDungeon();
    }

    public void StartNewDungeon()
    {
        // TO DO: Set parameters to match the players level. 

        // What type of dungeon do we want?
        Dungeon.DungeonType dungeonType = floor % MINIBOSS_INTERVAL == 0 ? Dungeon.DungeonType.MiniBoss : Dungeon.DungeonType.Normal;

        GetDungeon(dungeonType);

        ActiveDungeon = CurrentDungeon;

        // TO DO: wait till now to do all building stuff and active this shit!
        ActiveDungeon.BuildDungeon();

        //CurrentDungeon.GenerateDungeon(GameManager.Instance.ActiveCharacterInformation.Level, columns, rows, numRooms.GetRandomInRange(), minDistStartEnd);//GenerateDungeon();//GetNewDungeon(dungeonType);
        Debug.Log("Stored miniboss dungeons: " +dungeonPool.NumMiniBossDungeons + " --- Stored normal dungeons: "+ dungeonPool.NumNormalDungeons);
    }

    private void GetDungeon(Dungeon.DungeonType type)
    {
        CurrentDungeon = null;

        // Does the pool has a dungeon we can use?
        if (type == Dungeon.DungeonType.Normal)
        {
            if (dungeonPool.HasNormalDungeons) CurrentDungeon = dungeonPool.GetNormalDungeon();
        }
        else if (type == Dungeon.DungeonType.Normal)
        {
            if (dungeonPool.HasMiniBossDungeons) CurrentDungeon = dungeonPool.GetMiniBossDungeon();
        }

        // If the pool didn't have a dungeon for us..
        if(CurrentDungeon == null)
        {
            Debug.Log("No dungeon in pools found");
            // Generate a dungeon.
            GenerateDungeon();

            // TO DO: replace with 'while the pool wants to store our dungeon. aka he doens't have enough dungeons yet.'
            // While the dungeon doesn't fit the floor requirments.
            while (dungeonPool.CanStore(CurrentDungeon.Type) && CurrentDungeon.Type != type)
            {
                // Store the old dungeon.
                if(CurrentDungeon.Type == Dungeon.DungeonType.MiniBoss)
                {
                    dungeonPool.AddMiniBossDungeon(CurrentDungeon);
                }
                else
                {
                    dungeonPool.AddNormalDungeon(CurrentDungeon);
                }

                // Generate a new dungeon.
                GenerateDungeon();
            }
        }
    }

    private void GenerateDungeon()
    {
        CurrentDungeon = new Dungeon();
        CurrentDungeon.GenerateDungeon(GameManager.Instance.ActiveCharacterInformation.Level, columns, rows, numRooms.GetRandomInRange(), minDistStartEnd);
    }

    void GetPrefabs()
    {
        FloorPrefab = Resources.Load<GameObject>("Prefabs/Dungeon/Floor");
        WallPrefab = Resources.Load<GameObject>("Prefabs/Dungeon/Wall");
        PillarPrefab = Resources.Load<GameObject>("Prefabs/Dungeon/Pillar");
        CeilingPrefab = Resources.Load<GameObject>("Prefabs/Dungeon/Ceiling");
        PortalPrefab = Resources.Load<GameObject>("Prefabs/Dungeon/Portal");
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

    /// <summary>
    /// Returns false if no mini boss room is possible in this dungeon. 
    /// </summary>
    /// <returns></returns>
    private bool DetermineMinibossRoom()
    {
        return true;
    }

    public void FinishDungeon()
    {
        // Determine score. 

        // Increase floor. 
        floor++;

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
