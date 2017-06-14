using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDungeonManager : MonoBehaviour
{
    // vars for current dungeon/level
    public TileDungeon CurrentDungeon { get; private set; }
    public int CurrentLevel { get; private set; }
    public int Columns { get; private set; }
    public int Rows { get; private set; }


    // vars for dungeon generation
    public TileRuleParser Parser { get; private set; }
    public RecipeCreator RecipeCreator { get; private set; }
    public Graph Graph { get; private set; }
    public TileGrammarHandler TileGrammarHandler { get; private set; }



    public float WorldScaleX { get { return 3f; } }
    public float WorldScaleZ { get { return 3f; } }

    public Dictionary<DungeonDirection, Vector2> directionValues;

    public GameObject LevelParent { get; private set; }

    public GameObject FloorPrefab { get; private set; }
    public GameObject WallPrefab { get; private set; }
    public GameObject PillarPrefab { get; private set; }
    public GameObject CeilingPrefab { get; private set; }
    public GameObject PortalPrefab { get; private set; }
    public GameObject ChestPrefab { get; private set; }
    public GameObject SpiderPrefab { get; private set; }
    public GameObject TrapPrefab { get; private set; }
    public GameObject KeyPrefab { get; private set; }
    public GameObject LockPrefab { get; private set; }
    public GameObject MultiLockPrefab { get; private set; }

    public void Initialize()
    {
        Columns = 10;
        Rows = 10;

        // create a go to function as level parent
        LevelParent = new GameObject("Level Parent");

        // instantiate our 4 directions
        directionValues = new Dictionary<DungeonDirection, Vector2>();
        directionValues.Add(DungeonDirection.North, new Vector2(0, 1));
        directionValues.Add(DungeonDirection.East, new Vector2(1, 0));
        directionValues.Add(DungeonDirection.South, new Vector2(0, -1));
        directionValues.Add(DungeonDirection.West, new Vector2(-1, 0));

        // obtain all prefabs for building the dungeon
        GetPrefabs();

        // start at level 1
        CurrentLevel = 1;

        // starts a new dungeon to play
        Parser = new TileRuleParser();
        Graph = new Graph();                    // TODO: graph class should get a string with the correct file

        RecipeCreator = new RecipeCreator(Graph);
        TileGrammarHandler = new TileGrammarHandler(RecipeCreator.getRoomList());

        StartNewDungeon();

        // starting up the player
        GameManager.Instance.ActiveCharacterInformation.PlayerController.Initialize(CurrentDungeon.StartPosition);
    }

    void StartNewDungeon()
    {
        // 1. set the current dungeon to null
        CurrentDungeon = null;

        // 2. read all rules and the amount of mission graphs

        // 3. chose a mission graph based on the set preference

        // 4. obtain the ordered graph

        // 5. get the tile grammar rules

        // 6. set up the tile array
        char[][] tiles = new char[TileGrammarHandler.grid.Width][];

        // go through all the tile arrays...
        for (int i = 0; i < TileGrammarHandler.grid.Width; i++)
        {
            // ... and set each tile array is the correct height.
            tiles[i] = new char[TileGrammarHandler.grid.Height];

            for (int j = 0; j < TileGrammarHandler.grid.Height; j++)
            {
                Debug.Log(i + " " + j);
                tiles[i][j] = TileGrammarHandler.grid.GetTile(i, j);
            }
        }

        Columns = TileGrammarHandler.grid.Height;
        Rows = TileGrammarHandler.grid.Width;

        // 7. apply all rules to a 2d array

        // 8. create a new dungeon
        CurrentDungeon = new TileDungeon();

        // 9. and initialize it!
        CurrentDungeon.Initialize(tiles);
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

        TrapPrefab = Resources.Load<GameObject>("Prefabs/Dungeon/Trap");
        KeyPrefab = Resources.Load<GameObject>("Prefabs/Dungeon/Key");
        LockPrefab = Resources.Load<GameObject>("Prefabs/Dungeon/LockedDoor");
        MultiLockPrefab = Resources.Load<GameObject>("Prefabs/Dungeon/MultiLockedDoor");
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

    private void ClearDungeon()
    {
        CurrentDungeon.ClearDungeon();
    }

    public void FinishDungeon()
    {
        // Increase floor. 
        CurrentLevel++;

        // Clear current dungeon.
        ClearDungeon();

        // Start next.
        StartNewDungeon();

        // Respawn player.
        GameManager.Instance.UIManager.NextDungeon();
        GameManager.Instance.StartCoroutine(GameManager.Instance.ActiveCharacterInformation.PlayerController.Respawn(CurrentDungeon.StartPosition));
    }


}
