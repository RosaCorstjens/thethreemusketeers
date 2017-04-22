using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

// TO DO: also save inventory information.
public class SaveInformation
{
    private SaveDataContainer dataContainer = new SaveDataContainer();
    public SaveDataContainer DataContainer { get { return dataContainer; } }

    public List<PlayerInformation> PlayerInformationList { get; private set; }

    public void Initialize()
    {
        ReadAllSaveDataFiles();
    }

    public bool AddNewSaveData(SaveData dataToSave)
    {
        int id = dataContainer.SaveDataFiles.Count;

        // Cannot save the new data, already 3 save files. 
        if (id >= 3) return false;

        dataToSave.ID = id;

        dataContainer.SaveDataFiles.Add(dataToSave);

        SaveAllInformation();

        return true;
    }

    public void AlterSaveDate(SaveData alteredData)
    {
        SaveData dataToAlter = dataContainer.SaveDataFiles.Find(s => s.ID == alteredData.ID);

        dataToAlter = alteredData;
    }

    public void RemoveSaveData(int id)
    {
        dataContainer.SaveDataFiles.RemoveAt(id);
    }

    public void ReadAllSaveDataFiles()
    {
        Type[] types = { typeof(SaveData), typeof(CharacterStats), typeof(Stat), typeof(Modifier) };

        XmlSerializer serializer = new XmlSerializer(typeof(SaveDataContainer), types);

        TextReader textReader = new StreamReader(Application.streamingAssetsPath + "/SaveData.xml");

        dataContainer = (SaveDataContainer)serializer.Deserialize(textReader);

        textReader.Close();

        PlayerInformationList = new List<PlayerInformation>();

        for (int i = 0; i < dataContainer.SaveDataFiles.Count; i++)
        {
            PlayerInformation tempPlayerInfo = new PlayerInformation(dataContainer.SaveDataFiles[i]);
            PlayerInformationList.Add(tempPlayerInfo);
        }
    }

    public void SaveAllInformation()
    {
        SaveDataContainer tempDataContainer = new SaveDataContainer();

        Type[] types = { typeof(SaveData), typeof(CharacterStats), typeof(Stat), typeof(Modifier) };

        FileStream fs = new FileStream(Application.streamingAssetsPath + "/SaveData.xml", FileMode.Create);

        XmlSerializer serializer = new XmlSerializer(typeof(SaveDataContainer), types);

        for (int i = 0; i < dataContainer.SaveDataFiles.Count; i++)
        {
            tempDataContainer.SaveDataFiles.Add(dataContainer.SaveDataFiles[i]);
        }

        serializer.Serialize(fs, tempDataContainer);

        fs.Close();
    }
}

public class SaveDataContainer
{
    List<SaveData> saveDataFiles = new List<SaveData>();
    public List<SaveData> SaveDataFiles { get { return saveDataFiles; } set { saveDataFiles = value; } }

    public SaveDataContainer() { }
}
