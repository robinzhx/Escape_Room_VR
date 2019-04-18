using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UniqueIDMenu {

    
    [MenuItem("Plugins/UniqueID/Generate UniqueIDs for all gameObjects")]
    static void GenerateUniqueID()
    {
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if (!g.GetComponent<UniqueID>())
            {
                UniqueID u = g.AddComponent<UniqueID>();
                if (string.IsNullOrEmpty(u.guid))
                {
                    Guid guid = Guid.NewGuid();
                    u.guid = guid.ToString();
                }
                Debug.Log("Add UniqueID " + u.guid + " to: " + g.name);
            }
        }
    }
    
    [MenuItem("Plugins/UniqueID/Generate UniqueIDs for selected gameObjects")]
    static void GenerateUniqueIDSelection()
    {
        foreach (object o in Selection.gameObjects)
        {
            GameObject g = (GameObject)o;
            if (!g.GetComponent<UniqueID>())
            {
                UniqueID u = g.AddComponent<UniqueID>();
                if (string.IsNullOrEmpty(u.guid))
                {
                    Guid guid = Guid.NewGuid();
                    u.guid = guid.ToString();
                }
                Debug.Log("Add UniqueID " + u.guid + " to: " + g.name);
            }
        }
    }
    
    [MenuItem("Plugins/UniqueID/Re-Generate UniqueIDs (Dangerous! This will break old replay!)")]
    static void ReGenerateUniqueID()
    {
        RemoveUniqueID();
        GenerateUniqueID();
    }
    
    [MenuItem("Plugins/UniqueID/Remove UniqueIDs (Dangerous! This will break old replay!)")]
    static void RemoveUniqueID()
    {
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            UniqueID u = g.GetComponent<UniqueID>();
            if (u)
            {
                Debug.Log("Remove UniqueID " + u.guid + " from: " + g.name);
                UnityEngine.Object.DestroyImmediate(u);
            }
        }
    }
}
