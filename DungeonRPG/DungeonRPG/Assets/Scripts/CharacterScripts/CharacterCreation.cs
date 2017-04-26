using UnityEngine;
using System.Collections;

public class CharacterCreation : MonoBehaviour
{
    private BaseCharacterClass selectedClass;
    private GameObject cylinderCreator;

    private BaseWarriorClass warriorClass;
    private GameObject warriorPrefab;

    private HooverButton warrior;

    private UILabel description;
    private UIInput nameInput;

    public void Initialize()
    {
        cylinderCreator = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/MenuObjects/CylinderCreator"), Vector3.zero, Quaternion.identity) as GameObject;
        cylinderCreator.transform.localScale = new Vector3(2, 0.1f, 2);

        warriorClass = new BaseWarriorClass();

        warriorPrefab = GameManager.Instance.WarriorPrefab;

        warriorPrefab.transform.localPosition = cylinderCreator.transform.FindChild("SpawnPoint").transform.position;

        warrior = transform.FindChild("Anchor_MidLeft/ClassSelection/Grid/Warrior").GetComponent<HooverButton>();

        warrior.Initlialize();

        description = transform.FindChild("Anchor_MidRight/ClassExplenation/ClassDescription").GetComponent<UILabel>();
        nameInput = transform.FindChild("Anchor_Buttom/NameCreation/Input Field").GetComponent<UIInput>();

        GameManager.Instance.CameraManager.SetTarget(warriorPrefab.transform);
        GameManager.Instance.CameraManager.FocusBack(false);

        SelectWarriorClass();
    }

    public void SelectWarriorClass()
    {
        this.selectedClass = warriorClass;

        warrior.Selected = true;

        warriorPrefab.SetActive(true);
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

        warriorPrefab = null;

        Destroy(this.gameObject);
    }
}
