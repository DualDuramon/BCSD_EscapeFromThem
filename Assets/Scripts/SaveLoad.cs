using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "SaveFile.txt";

    private void Awake()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/SaveData/";
    }
    private void Start()
    {
        
        if(!Directory.Exists(SAVE_DATA_DIRECTORY))
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(GameManager.Instance.CurrentSaveData);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);
    }

    public string LoadSaveData()
    {
        if(isThereData())
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            
            return loadJson;
        }
        else
        {
            return "";
        }
    }

    public void InsertSaveData(StatusSaveData jsonSaveData)
    {
        GameManager.Instance.CurrentSaveData = jsonSaveData;
        GameManager.Instance.nowStageIndex = jsonSaveData.nowStageIndex;
    }

    public bool isThereData()
    {
        return File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
    }
}
