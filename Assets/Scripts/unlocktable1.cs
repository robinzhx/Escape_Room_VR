using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unlocktable1 : MonoBehaviour {


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "candlestick.001")
        {
            GameObject drawer = GameObject.Find("Side_Table_Drawer.001_proxy.001");
            UnlockTableMain drawerLock = (UnlockTableMain)drawer.GetComponent(typeof(UnlockTableMain));
            drawerLock.unlockOne();
        }
    }
}

