using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class AutoTagAttachScripts : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.FindGameObjectsWithTag("PickUpObj");
        GameObject[] allObjects = (GameObject[])Editor.FindObjectsOfType(typeof(GameObject));
        foreach (GameObject obj in allObjects)
            if (obj.CompareTag("PickUpObj"))
            {
                //print("found one");
                Interactable ds = obj.GetComponent(typeof(Interactable)) as Interactable;
                if (!ds)
                    obj.AddComponent(typeof(Interactable));

                Throwable th = obj.GetComponent(typeof(Throwable)) as Throwable;
                if (!th)
                    obj.AddComponent(typeof(Throwable));
            }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
