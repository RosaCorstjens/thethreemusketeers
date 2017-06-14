using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDungeonManager : MonoBehaviour
{
    public TileDungeon CurrentDungeon { get; private set; }
    public int CurrentLevel { get; private set; }

    public int Columns { get; private set; }
    public int Rows { get; private set; }

    public float WorldScaleX
    {
        get { return 3f; }
    }

    public float WorldScaleZ
    {
        get { return 3f; }
    }

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
        char[][] tiles = new char[Columns][];

        // go through all the tile arrays...
        for (int i = 0; i < Columns; i++)
        {
            // ... and set each tile array is the correct height.
            tiles[i] = new char[Rows];

            for (int j = 0; j < Rows; j++)
            {
                tiles[i][j] = 'u';
            }
        }

        tiles[0][0] = 'u';
        tiles[1][0] = 'u';
        tiles[2][0] = 'u';
        tiles[3][0] = 'u';
        tiles[4][0] = 'u';
        tiles[5][0] = 'u';
        tiles[6][0] = 'u';
        tiles[7][0] = 'u';
        tiles[8][0] = 'u';
        tiles[9][0] = 'u';

        tiles[0][1] = 'u';
        tiles[1][1] = 'u';
        tiles[2][1] = 'f';
        tiles[3][1] = 'f';
        tiles[4][1] = 'm';
        tiles[5][1] = 'u';
        tiles[6][1] = 'u';
        tiles[7][1] = 'u';
        tiles[8][1] = 'u';
        tiles[9][1] = 'u';

        tiles[0][2] = 'u';
        tiles[1][2] = 'u';
        tiles[2][2] = 'm';
        tiles[3][2] = 'f';
        tiles[4][2] = 'l';
        tiles[5][2] = 'u';
        tiles[6][2] = 'u';
        tiles[7][2] = 'u';
        tiles[8][2] = 'u';
        tiles[9][2] = 'u';

        tiles[0][3] = 'u';
        tiles[1][3] = 'u';
        tiles[2][3] = 'f';
        tiles[3][3] = 'f';
        tiles[4][3] = 'k';
        tiles[5][3] = 'u';
        tiles[6][3] = 'u';
        tiles[7][3] = 'u';
        tiles[8][3] = 'u';
        tiles[9][3] = 'u';
    
        tiles[0][4] = 'u';
        tiles[1][4] = 'u';
        tiles[2][4] = 'u';
        tiles[3][4] = 'f';
        tiles[4][4] = 'u';
        tiles[5][4] = 'u';
        tiles[6][4] = 'u';
        tiles[7][4] = 'u';
        tiles[8][4] = 'u';
        tiles[9][4] = 'u';

        tiles[0][5] = 'u';
        tiles[1][5] = 'u';
        tiles[2][5] = 'f';
        tiles[3][5] = 'f';
        tiles[4][5] = 'f';
        tiles[5][5] = 'u';
        tiles[6][5] = 'f';
        tiles[7][5] = 'f';
        tiles[8][5] = 'f';
        tiles[9][5] = 'u';

        tiles[0][6] = 'u';
        tiles[1][6] = 'u';
        tiles[2][6] = 'e';
        tiles[3][6] = 'f';
        tiles[4][6] = 'f';
        tiles[5][6] = 'p';
        tiles[6][6] = 't';
        tiles[7][6] = 'f';
        tiles[8][6] = 'f';
        tiles[9][6] = 'u';

        tiles[0][7] = 'u';
        tiles[1][7] = 'u';
        tiles[2][7] = 'f';
        tiles[3][7] = 'f';
        tiles[4][7] = 'P';
        tiles[5][7] = 'u';
        tiles[6][7] = 'f';
        tiles[7][7] = 'f';
        tiles[8][7] = 'f';
        tiles[9][7] = 'u';

        tiles[0][8] = 'u';
        tiles[1][8] = 'u';
        tiles[2][8] = 'u';
        tiles[3][8] = 'u';
        tiles[4][8] = 'u';
        tiles[5][8] = 'u';
        tiles[6][8] = 'u';
        tiles[7][8] = 'u';
        tiles[8][8] = 'u';
        tiles[9][8] = 'u';
    
        tiles[0][9] = 'u';
        tiles[1][9] = 'u';
        tiles[2][9] = 'u';
        tiles[3][9] = 'u';
        tiles[4][9] = 'u';
        tiles[5][9] = 'u';
        tiles[6][9] = 'u';
        tiles[7][9] = 'u';
        tiles[8][9] = 'u';
        tiles[9][9] = 'u';
        
        Debug.Log(Columns);
        Debug.Log(Rows);
        Debug.Log(tiles.Length);
        
        Debug.Log(tiles[0][0]);

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
