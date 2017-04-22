using UnityEngine;
using System.Collections;
using System;

public class MainMenu : MonoBehaviour
{
    private UILabel resume;
    private UILabel create;
    private UILabel select;
    private UILabel options;
    private UILabel exit;

    private UIGrid buttonGrid;

    private GameObject cylinderCreator;
    private GameObject warriorPrefab;
    private GameObject magePrefab;
    private GameObject hunterPrefab;

    private PlayerInformation selectedCharacter;
    private bool selectingCharacter = false;
    private UIWidget selectedCharInfo;
    private UILabel nameSelected;
    private UILabel classNameSelected;
    private UILabel levelSelected;

    private UIGrid selectCharGrid;
    private GameObject selectCharButton;
    private bool loadedSaveFiles;

    public void Intialize()
    {
        buttonGrid = transform.FindChild("Anchor_MidLeft/MenuButtons/Grid").GetComponent<UIGrid>();

        resume = buttonGrid.transform.FindChild("Resume Game").GetComponent<UILabel>();
        create = buttonGrid.transform.FindChild("Create Character").GetComponent<UILabel>();
        select = buttonGrid.transform.FindChild("Select Character").GetComponent<UILabel>();
        options = buttonGrid.transform.FindChild("Options").GetComponent<UILabel>();
        exit = buttonGrid.transform.FindChild("Exit").GetComponent<UILabel>();

        selectedCharInfo = transform.FindChild("Anchor_MidRight/SelectedCharacterInformation").GetComponent<UIWidget>();

        nameSelected = selectedCharInfo.transform.FindChild("Name").GetComponent<UILabel>();
        classNameSelected = selectedCharInfo.transform.FindChild("Class").GetComponent<UILabel>();
        levelSelected = selectedCharInfo.transform.FindChild("Level").GetComponent<UILabel>();

        // TO DO: this should not need to search for the object by himself! .....
        cylinderCreator = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/MenuObjects/CylinderCreator"), Vector3.zero, Quaternion.identity) as GameObject;
        cylinderCreator.transform.localScale = new Vector3(2, 0.1f, 2);

        warriorPrefab = GameManager.Instance.WarriorPrefab;
        magePrefab = GameManager.Instance.MagePrefab;
        hunterPrefab = GameManager.Instance.HunterPrefab;

        warriorPrefab.transform.localPosition = cylinderCreator.transform.FindChild("SpawnPoint").transform.position;
        magePrefab.transform.localPosition = cylinderCreator.transform.FindChild("SpawnPoint").transform.position;
        hunterPrefab.transform.localPosition = cylinderCreator.transform.FindChild("SpawnPoint").transform.position;

        selectCharGrid = transform.FindChild("Anchor_Mid/Container/Grid").GetComponent<UIGrid>();
        selectCharButton = Resources.Load("Prefabs/UI/MainMenu/SelectedSavedButton") as GameObject;

        // Set the parent and visibilty of the buttons according to save data etc. 
        // No saved games found. 
        if (GameManager.Instance.SaveInformation.DataContainer.SaveDataFiles.Count == 0)
        {
            // Deactivate resume and select. 
            resume.transform.SetParent(this.transform);
            select.transform.SetParent(this.transform);

            resume.gameObject.SetActive(false);
            select.gameObject.SetActive(false);

            buttonGrid.Reposition();

            // No character information to show yet. 
            selectedCharInfo.gameObject.SetActive(false);
        }
        else
        {
            // Select first character in the list as default character. 
            selectedCharacter = GameManager.Instance.SaveInformation.PlayerInformationList[0];

            // Set model. 
            switch (selectedCharacter.CharacterClass.CharacterClassType)
            {
                case CharacterClass.Hunter:
                    magePrefab.SetActive(false);
                    warriorPrefab.SetActive(false);
                    hunterPrefab.SetActive(true);
                    break;
                case CharacterClass.Mage:
                    magePrefab.SetActive(true);
                    warriorPrefab.SetActive(false);
                    hunterPrefab.SetActive(false);
                    break;
                case CharacterClass.Warrior:
                    magePrefab.SetActive(false);
                    warriorPrefab.SetActive(true);
                    hunterPrefab.SetActive(false);
                    break;
            }

            SetCharacterInformation();
        }

        GameManager.Instance.CameraManager.SetTarget(warriorPrefab.transform);
        GameManager.Instance.CameraManager.FocusBack(false);
    }

    // After Resume, the game starts. 
    public void ResumeGame()
    {
        GameManager.Instance.SetCurrentCharacter(selectedCharacter);
        GameManager.Instance.StartState(GameStates.InGame);
    }

    // After creation, the game starts. 
    public void CreateCharacter()
    {
        GameManager.Instance.StartState(GameStates.CharacterCreation);
    }

    // After select character, you go back to the 'normal' main menu. 
    public void SelectCharacter()
    {
        ToggleCharacterSelection(true);
        ToggleMainMenu(false);

        if (loadedSaveFiles) return;

        int i = GameManager.Instance.SaveInformation.DataContainer.SaveDataFiles.Count;

        // Just checking, but shouldn't be possible. 
        if (i == 0) return;
    
        for (int j = 0; j < i; j++)
        {
            GameObject tempButton = GameObject.Instantiate(selectCharButton);
            tempButton.transform.SetParent(selectCharGrid.transform);
            tempButton.transform.localScale = Vector3.one;
            tempButton.transform.localPosition = Vector3.zero;

            SelectCharacterButton tempBttnScript = tempButton.GetComponent<SelectCharacterButton>();
            tempBttnScript.Initialize();
            tempBttnScript.id = j;

            tempButton.transform.FindChild("Name").GetComponent<UILabel>().text = GameManager.Instance.SaveInformation.PlayerInformationList[j].Name;
            tempButton.transform.FindChild("Class").GetComponent<UILabel>().text = GameManager.Instance.SaveInformation.PlayerInformationList[j].CharacterClass.CharacterClassType.ToString();
            tempButton.transform.FindChild("Level").GetComponent<UILabel>().text = "Level " + GameManager.Instance.SaveInformation.PlayerInformationList[j].Level;
        }
        loadedSaveFiles = true;
        selectCharGrid.Reposition();
    }

    // Go back to main menu. 
    public void OnSelectCharacterButton(int id)
    {
        ToggleCharacterSelection(false);

        switch (selectedCharacter.CharacterClass.CharacterClassType)
        {
            case CharacterClass.Hunter:
                hunterPrefab.SetActive(false);
                break;
            case CharacterClass.Mage:
                magePrefab.SetActive(false);
                break;
            case CharacterClass.Warrior:
                warriorPrefab.SetActive(false);
                break;
        }

        selectedCharacter = GameManager.Instance.SaveInformation.PlayerInformationList[id];
        SetCharacterInformation();

        ToggleMainMenu(true);
    }

    public void Options()
    {
        throw new NotImplementedException();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetCharacterInformation()
    {
        // Set character information.
        nameSelected.text = selectedCharacter.Name;
        levelSelected.text = "Level " + selectedCharacter.Level;
        classNameSelected.text = selectedCharacter.CharacterClass.CharacterClassType.ToString();
    }

    public void Clear()
    {
        Debug.Log("Clearing 'MainMenu'");
        gameObject.SetActive(false);

        Destroy(cylinderCreator);
        cylinderCreator = null;

        warriorPrefab = null;
        hunterPrefab = null;
        magePrefab = null;

        selectedCharacter = null;
    }

    public void ToggleCharacterSelection(bool on)
    {
        selectCharGrid.gameObject.SetActive(on);
    }

    public void ToggleMainMenu(bool on)
    {
        selectedCharInfo.gameObject.SetActive(on);
        buttonGrid.gameObject.SetActive(on);

        cylinderCreator.SetActive(on);

        switch (selectedCharacter.CharacterClass.CharacterClassType)
        {
            case CharacterClass.Hunter:
                hunterPrefab.SetActive(on);
                break;
            case CharacterClass.Mage:
                magePrefab.SetActive(on);
                break;
            case CharacterClass.Warrior:
                warriorPrefab.SetActive(on);
                break;
        }
    }
}
