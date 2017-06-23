using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

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
    private List<GameObject> hudObjects;

    private List<ItemInstance> itemDrops;

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
        hudObjects = new List<GameObject>();
        itemDrops = new List<ItemInstance>();

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
                        GameObject keyObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.KeyPrefab, spawnPos + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
                        keyObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        KeyScript key = keyObject.GetComponent<KeyScript>();
                        key.Initialize(0);

                        keys.Add(key);
                        break;

                    // keymulti
                    case '0':
                        GameObject keyMultiObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.KeyPrefab, spawnPos + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
                        keyMultiObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        KeyScript keyMulti = keyMultiObject.GetComponent<KeyScript>();
                        keyMulti.Initialize(1);

                        keys.Add(keyMulti);
                        break;

                    // keyfinal
                    case 'K':
                        GameObject keyFinalObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.KeyPrefab, spawnPos + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
                        keyFinalObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        KeyScript keyFinal = keyFinalObject.GetComponent<KeyScript>();
                        keyFinal.Initialize(2);

                        keys.Add(keyFinal);
                        break;

                    // lock
                    case 'l':
                        GameObject lockObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.LockPrefab, spawnPos, Quaternion.identity) as GameObject;
                        lockObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);
                        lockObject.transform.localScale = new Vector3(GameManager.Instance.DungeonManager.WorldScaleX, GameManager.Instance.DungeonManager.WorldScaleY, GameManager.Instance.DungeonManager.WorldScaleZ);

                        lookPos = GameManager.Instance.DungeonManager.GridToWorldPosition(new Vector2(GetFloor(i, j).Neighbours[0].xPos, GetFloor(i, j).Neighbours[0].yPos));
                        lockObject.transform.LookAt(lookPos);

                        LockScript lockScript = lockObject.GetComponent<LockScript>();
                        lockScript.Initialize(0);

                        locks.Add(lockScript);
                        break;

                    // lockmulti
                    case '1':
                        GameObject lockMultiObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.MultiLockPrefab, spawnPos, Quaternion.identity) as GameObject;
                        lockMultiObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);
                        lockMultiObject.transform.localScale = new Vector3(GameManager.Instance.DungeonManager.WorldScaleX, GameManager.Instance.DungeonManager.WorldScaleY, GameManager.Instance.DungeonManager.WorldScaleZ);

                        lookPos = GameManager.Instance.DungeonManager.GridToWorldPosition(new Vector2(GetFloor(i, j).Neighbours[0].xPos, GetFloor(i, j).Neighbours[0].yPos));
                        lockMultiObject.transform.LookAt(lookPos);

                        LockScript lockMultiScript = lockMultiObject.GetComponent<LockScript>();
                        lockMultiScript.Initialize(1);

                        locks.Add(lockMultiScript);
                        break;

                    // lockfinal
                    case 'L':
                        GameObject lockFinalObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.LockPrefab, spawnPos, Quaternion.identity) as GameObject;
                        lockFinalObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);
                        lockFinalObject.transform.localScale = new Vector3(GameManager.Instance.DungeonManager.WorldScaleX, GameManager.Instance.DungeonManager.WorldScaleY, GameManager.Instance.DungeonManager.WorldScaleZ);

                        lookPos = GameManager.Instance.DungeonManager.GridToWorldPosition(new Vector2(GetFloor(i, j).Neighbours[0].xPos, GetFloor(i, j).Neighbours[0].yPos));
                        lockFinalObject.transform.LookAt(lookPos);

                        LockScript lockFinalScript = lockFinalObject.GetComponent<LockScript>();
                        lockFinalScript.Initialize(2);

                        locks.Add(lockFinalScript);
                        break;

                    // monster
                    case 'm':
                        GameObject monsterObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.SpiderPrefab, spawnPos + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                        monsterObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        EnemyController monster = monsterObject.GetComponent<EnemyController>();
                        int upgradeAmount = CalculateAmountOfMonsterUpgrades(spawnPos);
                        monster.Initialize(upgradeAmount, GameManager.Instance.ActiveCharacterInformation.Level);

                        enemies.Add(monster);
                        break;

                    // trap
                    case 'p':
                        GameObject trapObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.TrapPrefab, spawnPos + new Vector3(0, 0.29f, 0), Quaternion.identity) as GameObject;
                        trapObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);
                        trapObject.transform.Rotate(Vector3.right, -90);
                        trapObject.transform.localScale = new Vector3(GameManager.Instance.DungeonManager.WorldScaleX, GameManager.Instance.DungeonManager.WorldScaleZ, trapObject.transform.localScale.z);

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

                    case '3':
                        Floor floor = GetFloor(i, j);
                        floor.SetMaterial(Resources.Load<Material>("Materials/Dungeon/Bricks"));
                        if (floor.placement == Floor.Placement.Corner || floor.placement == Floor.Placement.Edge)
                        {
                            GameObject hudProb = GameObject.Instantiate(dm.HubPropPrefabs[UnityEngine.Random.Range(0, dm.HubPropPrefabs.Count)], spawnPos, Quaternion.identity);
                            hudProb.transform.SetParent(GameManager.Instance.DungeonManager.TestParent.transform);

                            hudObjects.Add(hudProb);
                        }

                        break;
                    case '2':
                        GetFloor(i, j).SetMaterial(Resources.Load<Material>("Materials/Dungeon/Bricks"));
                        break;

                    default:
                        Debug.LogError("Undefined type occured in the 2D tile array.");
                        break;
                }
            }
        }
    }

    public void AddItem(ItemInstance toAdd)
    {
        itemDrops.Add(toAdd);
        return;
    }

    private void RemoveContent()
    {
        // clear enemies
        enemies.HandleAction(e => GameObject.Destroy(e.gameObject));
        enemies.Clear();
        Debug.Log("enemies destroyed. " + enemies.Count);

        // clear chests
        lootchests.HandleAction(l => GameObject.Destroy(l.gameObject));
        lootchests.Clear();
        Debug.Log("lootchests destroyed. " + lootchests.Count);

        // clear keys
        keys.HandleAction(k => GameObject.Destroy(k.gameObject));
        keys.Clear();
        Debug.Log("keys destroyed. " + keys.Count);

        // clear locks
        locks.HandleAction(l => GameObject.Destroy(l.gameObject));
        locks.Clear();
        Debug.Log("locks destroyed. " + locks.Count);

        // clear traps
        traps.HandleAction(t => GameObject.Destroy(t.gameObject));
        traps.Clear();
        Debug.Log("traps destroyed. " + traps.Count);

        // clear hud objects
        hudObjects.HandleAction(g => GameObject.Destroy(g.gameObject));
        hudObjects.Clear();
        Debug.Log("hudObjects destroyed. " + hudObjects.Count);

        // clear item drops
        itemDrops.HandleAction(i => GameObject.Destroy(i.gameObject));
        itemDrops.Clear();
        Debug.Log("item drops destroyed. " + itemDrops.Count);
    }

    public void ClearDungeon()
    {
        // remove tiles array
        Array.Clear(tiles, 0, tiles.Length);
        Debug.Log("2D array with tiles cleared. " + tiles.Length);

        // remove floor object 
        floors.HandleAction(f => f.Destroy());
        floors.RemoveRange(0, floors.Count);
        Debug.Log("floor removed. " + floors.Count);

        // remove all content in the dungeon
        RemoveContent();
    }

    public void RestartDungeon()
    {
        // clear enemies
        enemies.HandleAction(e => GameObject.Destroy(e.gameObject));
        enemies.Clear();
        Debug.Log("enemies destroyed. " + enemies.Count);
        
        // clear keys
        keys.HandleAction(k => GameObject.Destroy(k.gameObject));
        keys.Clear();
        Debug.Log("keys destroyed. " + keys.Count);

        // clear chests
        locks.HandleAction(l => GameObject.Destroy(l.gameObject));
        locks.Clear();
        Debug.Log("locks destroyed. " + locks.Count);

        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                // if the tile's content shouldnt be respawned
                if (tiles[i][j] != 'k' && tiles[i][j] != '0' && tiles[i][j] != 'K' && tiles[i][j] != 'l' && tiles[i][j] != '1' && tiles[i][j] != 'L' && tiles[i][j] != 'm') continue;

                Vector3 spawnPos = GameManager.Instance.DungeonManager.GridToWorldPosition(new Vector2(i, j));

                switch (tiles[i][j])
                {
                    // key
                    case 'k':
                        GameObject keyObject =
                            GameObject.Instantiate(GameManager.Instance.DungeonManager.KeyPrefab,
                                spawnPos + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
                        keyObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        KeyScript key = keyObject.GetComponent<KeyScript>();
                        key.Initialize(0);

                        keys.Add(key);
                        break;

                    // keymulti
                    case '0':
                        GameObject keyMultiObject =
                            GameObject.Instantiate(GameManager.Instance.DungeonManager.KeyPrefab,
                                spawnPos + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
                        keyMultiObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        KeyScript keyMulti = keyMultiObject.GetComponent<KeyScript>();
                        keyMulti.Initialize(1);

                        keys.Add(keyMulti);
                        break;

                    // keyfinal
                    case 'K':
                        GameObject keyFinalObject =
                            GameObject.Instantiate(GameManager.Instance.DungeonManager.KeyPrefab,
                                spawnPos + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
                        keyFinalObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        KeyScript keyFinal = keyFinalObject.GetComponent<KeyScript>();
                        keyFinal.Initialize(2);

                        keys.Add(keyFinal);
                        break;

                    // TODO: rotate lock according to neighbours (90 degrees)
                    // lock
                    case 'l':
                        GameObject lockObject =
                            GameObject.Instantiate(GameManager.Instance.DungeonManager.LockPrefab, spawnPos,
                                Quaternion.identity) as GameObject;
                        lockObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        Vector3 lookPos =
                            GameManager.Instance.DungeonManager.GridToWorldPosition(
                                new Vector2(GetFloor(i, j).Neighbours[0].xPos, GetFloor(i, j).Neighbours[0].yPos));
                        lockObject.transform.LookAt(lookPos);

                        LockScript lockScript = lockObject.GetComponent<LockScript>();
                        lockScript.Initialize(0);

                        locks.Add(lockScript);
                        break;

                    // lockmulti
                    case '1':
                        GameObject lockMultiObject =
                            GameObject.Instantiate(GameManager.Instance.DungeonManager.MultiLockPrefab, spawnPos,
                                Quaternion.identity) as GameObject;
                        lockMultiObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        lookPos =
                            GameManager.Instance.DungeonManager.GridToWorldPosition(
                                new Vector2(GetFloor(i, j).Neighbours[0].xPos, GetFloor(i, j).Neighbours[0].yPos));
                        lockMultiObject.transform.LookAt(lookPos);

                        LockScript lockMultiScript = lockMultiObject.GetComponent<LockScript>();
                        lockMultiScript.Initialize(1);

                        locks.Add(lockMultiScript);
                        break;

                    // lockfinal
                    case 'L':
                        GameObject lockFinalObject =
                            GameObject.Instantiate(GameManager.Instance.DungeonManager.LockPrefab, spawnPos,
                                Quaternion.identity) as GameObject;
                        lockFinalObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);

                        lookPos =
                            GameManager.Instance.DungeonManager.GridToWorldPosition(
                                new Vector2(GetFloor(i, j).Neighbours[0].xPos, GetFloor(i, j).Neighbours[0].yPos));
                        lockFinalObject.transform.LookAt(lookPos);

                        LockScript lockFinalScript = lockFinalObject.GetComponent<LockScript>();
                        lockFinalScript.Initialize(2);

                        locks.Add(lockFinalScript);
                        break;

                    // monster
                    case 'm':
                        GameObject monsterObject = GameObject.Instantiate(GameManager.Instance.DungeonManager.SpiderPrefab, spawnPos + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
                        monsterObject.transform.SetParent(GameManager.Instance.DungeonManager.LevelParent.transform);
                        Debug.Log("Spider spawned");
                        EnemyController monster = monsterObject.GetComponent<EnemyController>();

                        int upgradeAmount = CalculateAmountOfMonsterUpgrades(spawnPos);
                        monster.Initialize(upgradeAmount, GameManager.Instance.ActiveCharacterInformation.Level);

                        enemies.Add(monster);
                        break;
                }
            }
        }
    }

    public bool IsOccupied(int xPos, int yPos)
    {
        if(xPos < 0 || xPos >= dm.Rows || yPos < 0 || yPos >= dm.Columns || tiles[xPos][yPos] == 'u') return false;

        return true;
    }

    private int CalculateAmountOfMonsterUpgrades(Vector3 monsterPosition)
    {
        float dist = Mathf.Abs((startPosition - monsterPosition).magnitude);
        if (dist < 50)
        {
            return 1;
        }
        else if (dist < 100)
        {
            return 2;
        }
        else if (dist < 150)
        {
            return 4;
        }
        else
        {
            return 6;
        }
    }

    void PopulateGroup(List<Floor> group, Floor floor)
    {
        group.Add(floor);
        floor.visited = true;

        // Check all four neighbors and recurse on them if needed:
        if (floor.xPos > 0)
        {
            var neighbor = GetFloor(floor.xPos - 1, floor.yPos);
            if (neighbor != null && neighbor.placement != Floor.Placement.Corridor && neighbor.visited == false)
                PopulateGroup(group, neighbor);
        }
        if (floor.xPos < dm.Columns - 1)
        {
            var neighbor = GetFloor(floor.xPos + 1, floor.yPos);
            if (neighbor != null && neighbor.placement != Floor.Placement.Corridor && neighbor.visited == false)
                PopulateGroup(group, neighbor);
        }
        if (floor.yPos > 0)
        {
            var neighbor = GetFloor(floor.xPos, floor.yPos - 1);
            if (neighbor != null && neighbor.placement != Floor.Placement.Corridor && neighbor.visited == false)
                PopulateGroup(group, neighbor);
        }
        if (floor.yPos < dm.Rows - 1)
        {
            var neighbor = GetFloor(floor.xPos, floor.yPos + 1);
            if (neighbor != null && neighbor.placement != Floor.Placement.Corridor && neighbor.visited == false)
                PopulateGroup(group, neighbor);
        }
    }

    public List<Floor> GetFloorsInRoom(Floor floor)
    {
        List<Floor> floorsInRoom = new List<Floor>();

        PopulateGroup(floorsInRoom, floor);

        floorsInRoom.HandleAction(f => f.visited = false);

        return floorsInRoom;
    }

    public List<Floor> GetFloorsInRoom(Vector2 floorCoord)
    {
        return GetFloorsInRoom(GetFloor((int)floorCoord.x, (int)floorCoord.y));
    }
}

