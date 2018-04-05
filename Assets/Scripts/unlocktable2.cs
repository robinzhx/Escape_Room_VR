using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unlocktable2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "mug")
        {
            GameObject drawer = GameObject.Find("Side_Table_Drawer.001_proxy.001");
            UnlockTableMain drawerLock = (UnlockTableMain)drawer.GetComponent(typeof(UnlockTableMain));
            drawerLock.unlockTwo();
        }
    }
}

