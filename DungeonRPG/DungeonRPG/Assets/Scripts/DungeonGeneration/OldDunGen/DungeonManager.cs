/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonManager : MonoBehaviour
{
    // TileRuleParser tileRuleParser
    // Graph missionGraph
    // List<Node> missionInstructions
    // List<TileGrammarRule> tileRecipe

    public Dungeon CurrentDungeon { get; private set; }

    public int columns = 1000;                                 // The number of columns on the board (how wide it will be).
    public int rows = 1000;                                    // The number of rows on the board (how tall it will be).

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
    public GameObject TrapPrefab { get; private set; }

    private GameObject levelParent;
    public GameObject LevelParent { get { return levelParent; } }

    public float WorldScaleX { get { return 3f; } }
    public float WorldScaleZ { get { return 3f; } }

    public Dictionary<DungeonDirection, Vector2> directionValues;

    private int currentLevel;
    public int CurrentLevel { get { return currentLevel; } }

    /// <summary>
    /// Initializes the dungeonmanager.
    /// </summary>
    public void Initialize()
    {
        // create a go to function as level parent
        levelParent = new GameObject("Level Parent");

        // instantiate our 4 directions
        directionValues = new Dictionary<DungeonDirection, Vector2>();
        directionValues.Add(DungeonDirection.North, new Vector2(0, 1));
        directionValues.Add(DungeonDirection.East, new Vector2(1, 0));
        directionValues.Add(DungeonDirection.South, new Vector2(0, -1));
        directionValues.Add(DungeonDirection.West, new Vector2(-1, 0));

        // obtain all prefabs for building the dungeon
        GetPrefabs();

        // start at level 1
        currentLevel = 1;

        // starts a new dungeon to play
        StartNewDungeon();

        // starting up the player
        GameManager.Instance.ActiveCharacterInformation.PlayerController.Initialize(CurrentDungeon.StartPosition);
    }

    /// <summary>
    /// Generates and starts a new dungeon.
    /// </summary>
    public void StartNewDungeon()
    {
        // set the current dungeon to null
        CurrentDungeon = null;

        // generate a new dungeon
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        CurrentDungeon = new Dungeon();
        //CurrentDungeon.GenerateDungeon(GameManager.Instance.ActiveCharacterInformation.Level, columns, rows, numRooms.GetRandomInRange(), minDistStartEnd);
        
        CurrentDungeon.BuildDungeon();
    }

    public void FinishDungeon()
    {
        // Increase floor. 
        currentLevel++;

        // Clear current dungeon.
        ClearDungeon();

        // Start next.
        StartNewDungeon();

        // Respawn player.
        GameManager.Instance.UIManager.NextDungeon();
        GameManager.Instance.StartCoroutine(GameManager.Instance.ActiveCharacterInformation.PlayerController.Respawn(CurrentDungeon.StartPosition));
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
*/
