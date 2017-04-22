using UnityEngine;
using System.Collections;

public class LevelManager
{
    private DungeonManager dungeonManager;
    public DungeonManager DungeonManager { get { return dungeonManager; } }

    private GameObject levelParent;
    public GameObject LevelParent { get { return levelParent; } }

    public void Initialize()
    {
        levelParent = new GameObject("Level Parent");

        dungeonManager = new DungeonManager();
        dungeonManager.Initialize();
        Vector3 spawnPosition = dungeonManager.CurrentDungeon.StartPosition;

        GameObject monsterObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Monsters/Spider"), new Vector3(spawnPosition.x + 10, spawnPosition.y, spawnPosition.z), Quaternion.identity) as GameObject;
        EnemyController monster = monsterObject.GetComponent<EnemyController>();
        monster.Initialize();

        Vector3 chestPosition = dungeonManager.CurrentDungeon.Rooms[0].RandomPositionInRoom();
        GameObject chestObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Props/LootChest"), new Vector3(spawnPosition.x, spawnPosition.y + 0.2f, spawnPosition.z + 10), Quaternion.identity) as GameObject;
        LootChest chest = chestObject.GetComponent<LootChest>();
        chest.Initialize();

        GameManager.Instance.ActiveCharacterInformation.PlayerController.Initialize(spawnPosition);
    }
}