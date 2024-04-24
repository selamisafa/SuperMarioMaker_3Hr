using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveLevelScript : MonoBehaviour
{
    public SaveableObject_Data[] saveableObjects;
    string selectedPath;

    void SaveTheLevel()
    {
        SaveableObjectScript[] saveableObjectScripts = GetComponentsInChildren<SaveableObjectScript>();
        saveableObjects = new SaveableObject_Data[saveableObjectScripts.Length];

        for (int i = 0; i < saveableObjectScripts.Length; i++)
        {
            saveableObjects[i] = saveableObjectScripts[i].GetData();
        }

        WriteToFile(JsonHelper.ToJson(saveableObjects, true));
    }

    void LoadTheLevel()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 1; i < transform.GetChild(0).childCount; i++)
        {
            Destroy(transform.GetChild(0).GetChild(i).gameObject);
        }

        saveableObjects = JsonHelper.FromJson<SaveableObject_Data>(ReadFromFile());

        for (int i = 0; i < saveableObjects.Length; i++)
        {
            GameObject GO = Instantiate(Resources.Load<GameObject>("SaveableObjects" + "/" + saveableObjects[i].objectID), transform);

            GO.GetComponent<SaveableObjectScript>().data.args = saveableObjects[i].args;

            switch (saveableObjects[i].objectType)
            {
                case SaveableObjectType.Ground:
                    GO.transform.SetParent(transform.GetChild(0));
                    break;
                case SaveableObjectType.Win:
                    break;
                case SaveableObjectType.Lose:
                    break;
                case SaveableObjectType.Player:
                    break;
                default:
                    break;
            }

            GO.transform.position = saveableObjects[i].position;
            GO.transform.rotation = saveableObjects[i].rotation;
            GO.transform.localScale = saveableObjects[i].scale;
        }
    }

    void WriteToFile(string text)
    {
        File.WriteAllText(selectedPath, text);
    }

    string ReadFromFile()
    {
        return File.ReadAllText(selectedPath);
    }

    public void LoadButton()
    {
        selectedPath = EditorUtility.OpenFilePanel("Pick the level file...", Application.dataPath, "json");

        if (selectedPath.Length != 0)
        {
            LoadTheLevel();
        }
        else
        {
            LoadButton();
        }
    }

    public void SaveButton()
    {
        selectedPath = EditorUtility.SaveFilePanel("Where to save the JSON...", Application.dataPath, "LevelJSON", "json");

        if (selectedPath.Length != 0)
        {
            SaveTheLevel();
        }
        else
        {
            SaveButton();
        }
    }

#region In_Game
    public void InGame_SaveTheLevel()
    {
        SaveableObjectScript[] saveableObjectScripts = GetComponentsInChildren<SaveableObjectScript>();
        saveableObjects = new SaveableObject_Data[saveableObjectScripts.Length];

        for (int i = 0; i < saveableObjectScripts.Length; i++)
        {
            saveableObjects[i] = saveableObjectScripts[i].GetData();
        }

        InGame_WriteToFile(JsonHelper.ToJson(saveableObjects, true));
    }

    public void InGame_LoadTheLevel()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 1; i < transform.GetChild(0).childCount; i++)
        {
            Destroy(transform.GetChild(0).GetChild(i).gameObject);
        }

        saveableObjects = JsonHelper.FromJson<SaveableObject_Data>(InGame_ReadFromFile());

        for (int i = 0; i < saveableObjects.Length; i++)
        {
            GameObject GO = Instantiate(Resources.Load<GameObject>("SaveableObjects" + "/" + saveableObjects[i].objectID), transform);

            switch (saveableObjects[i].objectType)
            {
                case SaveableObjectType.Ground:
                    GO.transform.SetParent(transform.GetChild(0));
                    break;
                case SaveableObjectType.Win:
                    break;
                case SaveableObjectType.Lose:
                    break;
                case SaveableObjectType.Player:
                    break;
                default:
                    break;
            }

            GO.transform.position = saveableObjects[i].position;
            GO.transform.rotation = saveableObjects[i].rotation;
            GO.transform.localScale = saveableObjects[i].scale;
        }
    }

    void InGame_WriteToFile(string text)
    {
        File.WriteAllText(Application.persistentDataPath + "/TestGame.JSON", text);
    }

    string InGame_ReadFromFile()
    {
        return File.ReadAllText(Application.persistentDataPath + "/TestGame.JSON");
    }

#endregion
}
