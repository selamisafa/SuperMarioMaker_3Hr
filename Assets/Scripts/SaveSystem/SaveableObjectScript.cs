using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableObjectScript : MonoBehaviour
{
    public SaveableObject_Data data;

    public SaveableObject_Data GetData()
    {
        data.position = transform.position;
        data.rotation = transform.rotation;
        data.scale = transform.localScale;

        return data;
    }
}

[Serializable]
public class SaveableObject_Data
{
    public string objectID = string.Empty;
    public SaveableObjectType objectType;
    public string args = string.Empty;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

[Serializable]
public enum SaveableObjectType
{
    Ground,
    Win,
    Lose,
    Player,
}
