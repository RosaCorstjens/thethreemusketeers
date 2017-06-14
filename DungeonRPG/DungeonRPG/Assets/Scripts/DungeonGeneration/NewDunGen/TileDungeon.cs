using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDungeon
{
    private TileDungeonManager dm; // ref to dungeon manager

    private char[][] tiles;
    private List<Floor> floors;

    // TODO: empty and destory these lists properly
    private List<EnemyController> enemies;
    public List<EnemyController> Enemies { get { return enemies; } }

    private List<LootChest> lootchests;

    private List<KeyScript> keys;
    private List<LockScript> locks;
    private List<TrapScript> traps;
    private PortalScript portal;

    private Vector3 startPosition;

    public Vector3 StartPosition
    {
        get { return startPosition; }
    }

    private int level;

    public Floor GetFloor(int xPos, int yPos)
    {
        return floors.Find(f => f.xPos == xPos && f.yPos == yPos);
    }

    public void Initialize(char[][] tiles)
    {
        dm = GameManager.Instance.DungeonManager;
        this.tiles = tiles;

        enemies = new List<EnemyController>();
        lootchests = new List<LootChest>();
        keys = new List<KeyScript>();
        locks = new List<LockScript>();
        traps = new List<TrapScript>();

        // instantiates all floor tiles
        InstantiateFloors();

        // spawns content in the dungeon
        SpawnContent();
    }

    private void InstantiateFloors()
    {
        floors = new List<Floor>();

        // Go through all the tiles in the jagged array...
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                // if the tile is undefined
                if (tiles[i][j] == 'u') continue;

                // create a new floor object
                Floor newFloor = new Floor();
                newFloor.Initialize(i, j);
                floors.Add(newFloor);
            }
        }

        floors.HandleAction(f => f.DetermineNeighbours());
        floors.HandleAction(f => f.PlaceFloor());
        floors.HandleAction(f => f.PlaceWalls());
    }

    private void SpawnContent()
    {
        //TODO: key and lock are new content after you have been merged
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                // if the tile is undefined
                if (tiles[i][j] == 'u' || tiles[i][j] == 'f' || tiles[i][j] == 'd') continue;

                Vector3 spawnPos = GameManager.Instance.DungeonManager.GridToWorldPosition(new Vector2(i, j));

                switch (tiles[i][j]) {
                    case 'r': // room
                    case 'w': // wall
                    case 'h':
                    case 'H':
                        Debug.LogError(tiles[i][j].ToString() + " shouldn't occur in the generated dungeon.");
                        break;

                    // treasure
                    case 't':
                        GameObject chestObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.ChestPrefab, spawnPos + new Vector3(0, 0.2f, 0), Quaternion.identity) as GameObject;
                        chestObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        int randomNeighbour = Random.Range(0, GetFloor(i, j).Neighbours.Count);
                        Vector3 lookPos = GameManager.Instance.DungeonManager.GridToWorldPosition(new Vector2(GetFloor(i, j).Neighbours[randomNeighbour].xPos, GetFloor(i, j).Neighbours[randomNeighbour].yPos));
                        chestObject.transform.LookAt(lookPos);
                        LootChest chest = chestObject.GetComponent<LootChest>();
                        chest.Initialize();

                        lootchests.Add(chest);
                        break;

                    // key
                    case 'k':
                        GameObject keyObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.KeyPrefab, spawnPos, Quaternion.identity) as GameObject;
                        keyObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        KeyScript key = keyObject.GetComponent<KeyScript>();
                        key.Initialize(0);

                        keys.Add(key);
                        break;

                    // keymulti
                    case '0':
                        GameObject keyMultiObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.KeyPrefab, spawnPos, Quaternion.identity) as GameObject;
                        keyMultiObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        KeyScript keyMulti = keyMultiObject.GetComponent<KeyScript>();
                        keyMulti.Initialize(1);

                        keys.Add(keyMulti);
                        break;

                    // keyfinal
                    case 'K':
                        GameObject keyFinalObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.KeyPrefab, spawnPos, Quaternion.identity) as GameObject;
                        keyFinalObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        KeyScript keyFinal = keyFinalObject.GetComponent<KeyScript>();
                        keyFinal.Initialize(2);

                        keys.Add(keyFinal);
                        break;

                    // TODO: rotate lock according to neighbours (90 degrees)
                    // lock
                    case 'l':
                        GameObject lockObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.LockPrefab, spawnPos, Quaternion.identity) as GameObject;
                        lockObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        LockScript lockScript = lockObject.GetComponent<LockScript>();
                        lockScript.Initialize(0);

                        locks.Add(lockScript);
                        break;

                    // lockmulti
                    case '1':
                        GameObject lockMultiObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.LockPrefab, spawnPos, Quaternion.identity) as GameObject;
                        lockMultiObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        LockScript lockMultiScript = lockMultiObject.GetComponent<LockScript>();
                        lockMultiScript.Initialize(1);

                        locks.Add(lockMultiScript);
                        break;

                    // lockfinal
                    case 'L':
                        GameObject lockFinalObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.LockPrefab, spawnPos, Quaternion.identity) as GameObject;
                        lockFinalObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        LockScript lockFinalScript = lockFinalObject.GetComponent<LockScript>();
                        lockFinalScript.Initialize(2);

                        locks.Add(lockFinalScript);
                        break;

                    // monster
                    case 'm':
                        GameObject monsterObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.SpiderPrefab, spawnPos + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                        monsterObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        EnemyController monster = monsterObject.GetComponent<EnemyController>();
                        monster.Initialize();

                        enemies.Add(monster);
                        break;

                    // trap
                    case 'p':
                        GameObject trapObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.TrapPrefab, spawnPos + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                        trapObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);
                        trapObject.transform.Rotate(Vector3.right, -90);

                        TrapScript trap = trapObject.GetComponent<TrapScript>();
                        trap.Initialize();

                        traps.Add(trap);
                        break;

                    // entrance
                    case 'e':
                        startPosition = dm.GridToWorldPosition(new Vector2(i, j));
                        break;

                    // portal
                    case 'P':
                        GameObject portal = GameObject.Instantiate(dm.PortalPrefab, spawnPos + new Vector3(0, 1.75f, 0), Quaternion.identity);
                        portal.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);
                        portal.transform.localEulerAngles = GetFloor(i, j).ObjectRotation();

                        PortalScript portalScript = portal.GetComponent<PortalScript>();
                        portalScript.Initialze();
                        break;

                    default:
                        Debug.LogError("Undefined type occured in the 2D tile array.");
                        break;
                }
            }
        }
    }

    // TODO
    public void ClearDungeon()
    {
    }

    // TODO
    public void RestartDungeon()
    {
    }

    public bool IsOccupied(int xPos, int yPos)
    {
        if(xPos < 0 || xPos >= dm.Rows || yPos < 0 || yPos >= dm.Columns || tiles[xPos][yPos] == 'u') return false;

        return true;
    }
}

