using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

public enum StatTypes
{
    Strength,
    Dexterity,
    Intelligence,
    Vitality,

    Armor,
    Damage,
    WeaponDamage,
    Resistance,

    MaxHealth,
    HealthPerSec,
    HealthPerHit,
    HealthPerKill,

    MaxResource,
    ResourceRegen,

    ExperienceBonus,
    MovementSpeed,

    AttackSpeed,
    CritRate,
    CritDamage,
    AreaDamage,
    CoolDownReduction,

    DodgeChance,
    BlockAmount,
    BlockChance,
    Thorns
}

public enum EquipmentSlotType { Belt, Boots, Bracers, Chest, Helmet, Pants, Gloves, Shoulders, Ring, Amulet, MainHand, OffHand }
public enum Equipment { Belt, Boots, Bracers, Chest, Helmet, Pants, Gloves, Shoulders, Ring, Amulet, Weapon, Shield }

public enum Quality { Common, Magic, Rare, Legendary }

public enum GameStates { MainMenu, CharacterCreation, InGame }

public class GameManager : MonoBehaviour
{
    // Singleton to make the gamemanager easy to acces. 
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    private GameStates gameState;
    public GameStates GameState { get { return gameState; } }

    private GameObject warriorPrefab;
    public GameObject WarriorPrefab { get { return warriorPrefab; } }
    private GameObject magePrefab;
    public GameObject MagePrefab { get { return magePrefab; } }
    private GameObject hunterPrefab;
    public GameObject HunterPrefab { get { return hunterPrefab; } }

    private GameObject activeCharacter;
    public GameObject ActiveCharacter { get { return activeCharacter; } }
    private PlayerInformation activeCharacterInformation;
    public PlayerInformation ActiveCharacterInformation { get { return activeCharacterInformation; } }

    // Reference to inventory manager. 
    private UIManager uiManager;
    public UIManager UIManager { get { return uiManager; } }

    // Reference to item manager. 
    [SerializeField]
    private ItemManager itemManager = new ItemManager();
    public ItemManager ItemManager { get { return itemManager; } }

    private CameraManager cameraManager;
    public CameraManager CameraManager { get { return cameraManager; } }

    private TileDungeonManager dungeonManager;
    public TileDungeonManager DungeonManager { get { return dungeonManager; } }

    private CharacterCreation characterCreation;
    public CharacterCreation CharacterCreation { get { return characterCreation; } set { characterCreation = value; } }

    private MainMenu mainMenu;
    public MainMenu MainMenu { get { return mainMenu; } }

    private SaveInformation saveInformation;
    public SaveInformation SaveInformation { get { return saveInformation; } }

    public bool Initialized { get; private set; }

    public void Awake()
    {
        gameState = GameStates.InGame;

        saveInformation = new SaveInformation();
        saveInformation.Initialize();

        uiManager = new UIManager();
        uiManager.Initialize();

        cameraManager = new CameraManager();
        cameraManager.Initialize();

        InstantiateCharacterPrefabs();
        RenderSettings.fog = true;
        //RenderSettings.ambientLight = new Color(0, 0, 255);
        //RenderSettings.ambientIntensity = 0.00001f;
        StartState(gameState);
    }

    private void InstantiateCharacterPrefabs()
    {
        warriorPrefab = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Characters/Players/Warrior"));
        warriorPrefab.transform.localScale = Vector3.one;
        warriorPrefab.gameObject.SetActive(false);

        magePrefab = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Characters/Players/Mage"));
        magePrefab.transform.localScale = Vector3.one;
        magePrefab.gameObject.SetActive(false);

        hunterPrefab = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Characters/Players/Hunter"));
        hunterPrefab.transform.localScale = Vector3.one;
        hunterPrefab.gameObject.SetActive(false);
    }

    public void StartState(GameStates gameState)
    {
        Initialized = false;

        ClearPreviousState();

        switch (gameState)
        {
            case GameStates.MainMenu:
                StartMainMenu();
                break;
            case GameStates.CharacterCreation:
                StartCharacterCreation();
                break;
            case GameStates.InGame:
                StartInGame();
                break;
        }

        Initialized = true;
    }

    private void ClearPreviousState()
    {
        switch (GameState)
        {
            case GameStates.MainMenu:
                ClearMainMenu();
                break;
            case GameStates.CharacterCreation:
                ClearCharacterCreation();
                break;
            case GameStates.InGame:
                ClearInGame();
                break;
        }
    }

    private void StartMainMenu()
    {
        gameState = GameStates.MainMenu;

        mainMenu = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/MainMenu/MainMenuPanel").GetComponent<MainMenu>());
        mainMenu.transform.SetParent(UIManager.UIRoot.transform);
        mainMenu.transform.localScale = Vector3.one;
        mainMenu.transform.localPosition = Vector3.zero;

        mainMenu.Intialize();

        CameraManager.CameraScript.CanReceiveInput = false;
    }

    private void StartCharacterCreation()
    {
        gameState = GameStates.CharacterCreation;

        characterCreation = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/CharacterCreation/CharacterCreationPanel")).GetComponent<CharacterCreation>();
        characterCreation.transform.SetParent(UIManager.UIRoot.transform);
        characterCreation.transform.localScale = Vector3.one;
        characterCreation.transform.localPosition = Vector3.zero;

        characterCreation.Initialize();

        CameraManager.CameraScript.CanReceiveInput = false;
    }

    public void StartInGame()
    {
        gameState = GameStates.InGame;

        itemManager.Initialize();

        if(activeCharacterInformation == null) activeCharacterInformation = GameManager.Instance.SaveInformation.PlayerInformationList[0];

        activeCharacter = warriorPrefab;
        activeCharacter.SetActive(true);
        activeCharacterInformation.PlayerController = activeCharacter.GetComponent<PlayerController>();

        UIManager.InitializeGameUI();

        dungeonManager = new TileDungeonManager();
        dungeonManager.Initialize();

        CameraManager.SetTarget(activeCharacter.transform);
        CameraManager.FocusBack(true);
        CameraManager.CameraScript.CanReceiveInput = true;
        CameraManager.CameraScript.height = 2;

        StartCoroutine(HandleInput());
    }

    private void ClearMainMenu()
    {
        if (mainMenu == null) return;

        mainMenu.Clear();
    }

    private void ClearCharacterCreation()
    {
        if (characterCreation == null) return;

        characterCreation.Clear();
    }

    public void ClearInGame()
    {

    }

    public void SetCurrentCharacter(PlayerInformation playerInformation)
    {
        this.activeCharacterInformation = playerInformation;
        switch (activeCharacterInformation.CharacterClass.CharacterClassType)
        {
            case CharacterClass.Hunter:
                activeCharacter = hunterPrefab;
                break;
            case CharacterClass.Mage:
                activeCharacter = magePrefab;
                break;
            case CharacterClass.Warrior:
                activeCharacter = warriorPrefab;
                break;
        }
    }

    private IEnumerator HandleInput()
    {
        bool inventoryOpen = false;

        while (true)
        {
            // Inventory key. 
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryOpen = !inventoryOpen;
                UIManager.InventoryManager.ToggleMenu(inventoryOpen);
                activeCharacterInformation.PlayerController.InMenu = inventoryOpen;
                cameraManager.CameraScript.CanReceiveInput = !inventoryOpen;
                yield return new WaitForSeconds(0.2f);
            }

            yield return null;
        }

        yield break;
    }


}