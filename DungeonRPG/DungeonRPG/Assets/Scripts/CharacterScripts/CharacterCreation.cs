using UnityEngine;
using System.Collections;

public class CharacterCreation : MonoBehaviour
{
    private BaseCharacterClass selectedClass;
    private GameObject cylinderCreator;

    private BaseWarriorClass warriorClass;
    private GameObject warriorPrefab;
    private BaseMageClass mageClass;
    private GameObject magePrefab;
    private BaseHunterClass hunterClass;
    private GameObject hunterPrefab;

    private HooverButton warrior;
    private HooverButton mage;
    private HooverButton hunter;
    private Color normalColor;
    private Color selectedColor = Color.white;

    private UILabel description;
    private UIInput nameInput;

    public void Initialize()
    {
        cylinderCreator = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/MenuObjects/CylinderCreator"), Vector3.zero, Quaternion.identity) as GameObject;
        cylinderCreator.transform.localScale = new Vector3(2, 0.1f, 2);

        warriorClass = new BaseWarriorClass();
        mageClass = new BaseMageClass();
        hunterClass = new BaseHunterClass();

        warriorPrefab = GameManager.Instance.WarriorPrefab;
        magePrefab = GameManager.Instance.MagePrefab;
        hunterPrefab = GameManager.Instance.HunterPrefab;

        warriorPrefab.transform.localPosition = cylinderCreator.transform.FindChild("SpawnPoint").transform.position;
        magePrefab.transform.localPosition = cylinderCreator.transform.FindChild("SpawnPoint").transform.position;
        hunterPrefab.transform.localPosition = cylinderCreator.transform.FindChild("SpawnPoint").transform.position;

        warrior = transform.FindChild("Anchor_MidLeft/ClassSelection/Grid/Warrior").GetComponent<HooverButton>();
        mage = transform.FindChild("Anchor_MidLeft/ClassSelection/Grid/Mage").GetComponent<HooverButton>();
        hunter = transform.FindChild("Anchor_MidLeft/ClassSelection/Grid/Hunter").GetComponent<HooverButton>();
        normalColor = warrior.GetComponent<UILabel>().color;

        warrior.Initlialize();
        mage.Initlialize();
        hunter.Initlialize();

        description = transform.FindChild("Anchor_MidRight/ClassExplenation/ClassDescription").GetComponent<UILabel>();
        nameInput = transform.FindChild("Anchor_Buttom/NameCreation/Input Field").GetComponent<UIInput>();

        GameManager.Instance.CameraManager.SetTarget(warriorPrefab.transform);
        GameManager.Instance.CameraManager.FocusBack(false);

        SelectWarriorClass();
    }

    public void SelectMageClass()
    {
        this.selectedClass = mageClass;

        warrior.Selected = false;
        mage.Selected = true;
        hunter.Selected = false;

        magePrefab.SetActive(true);
        warriorPrefab.SetActive(false);
        hunterPrefab.SetActive(false);

        description.text = this.selectedClass.ClassDescription;
    }

    public void SelectWarriorClass()
    {
        this.selectedClass = warriorClass;

        warrior.Selected = true;
        mage.Selected = false;
        hunter.Selected = false;

        warriorPrefab.SetActive(true);
        magePrefab.SetActive(false);
        hunterPrefab.SetActive(false);
        description.text = this.selectedClass.ClassDescription;

    }
    public void SelectHunterClass()
    {
        this.selectedClass = hunterClass;

        warrior.Selected = false;
        mage.Selected = false;
        hunter.Selected = true;

        hunterPrefab.SetActive(true);
        magePrefab.SetActive(false);
        warriorPrefab.SetActive(false);

        description.text = this.selectedClass.ClassDescription;
    }

    public void CreateCharacter()
    {
        Debug.Log("Creating character...");

        // Create the character with current information.
        PlayerInformation newPlayer = new PlayerInformation(nameInput.label.text, selectedClass);

        // Set CurrentPlayer in GM to the newly created player. 
        GameManager.Instance.SetCurrentCharacter(newPlayer);

        GameManager.Instance.SaveInformation.AddNewSaveData(newPlayer.savedata);

        // Make the GameManager load ingame state. 
        GameManager.Instance.StartState(GameStates.InGame);
    }

    public void Clear()
    {
        Debug.Log("Clearing 'PlayerCreator'");
        gameObject.SetActive(false);

        selectedClass = null;

        Destroy(cylinderCreator);
        cylinderCreator = null;

        warriorClass = null;
        mageClass = null;
        hunterClass = null;

        warriorPrefab = null;
        hunterPrefab = null;
        magePrefab = null;

        Destroy(this.gameObject);
    }
}
