using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorScript : MonoBehaviour
{
    /*
     * 41 = player controller
     * 1:47:10.09 = save/load system
     * 2:35:27.32
     * 3:00:15.60
     */

    /*
     * https://prasetion.medium.com/saving-data-as-json-in-unity-4419042d1334
     * 
     * https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity
     * 
     * Block type enum for every object + special args string so we can store data like position/rotation/lifetime and so on
     * 
     * ----------------------
     * choose where to save and load from a file (only in editor)
     * start/test button (Test ending)
     * a way to control the sizes of blocks (no click go up get down)
     * ----------------------
     * 
     */

    [SerializeField] Transform levelParent;
    [SerializeField] Transform currentObj = null;

    [SerializeField] SaveLevelScript saveLevelScript;

    private void Start()
    {
        Time.timeScale = 0.0f;
    }

    public void InstantiateObject(string objectID)
    {
        currentObj = Instantiate(Resources.Load<GameObject>("SaveableObjects" + "/" + objectID), levelParent).transform;

        if (currentObj.GetComponent<SaveableObjectScript>().data.objectType == SaveableObjectType.Ground)
        {
            currentObj.SetParent(levelParent.GetChild(0));
        }
    }

    private void Update()
    {
        if (currentObj != null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            currentObj.position = pos;

            if (Input.GetMouseButtonUp(1))
            {
                currentObj = null;
            }
        }
    }

    public void StartTestingButton()
    {
        saveLevelScript.InGame_SaveTheLevel();

        Time.timeScale = 1.0f;
    }

    public void EndTesting()
    {
        saveLevelScript.InGame_LoadTheLevel();

        Time.timeScale = 0.0f;
    }
}
