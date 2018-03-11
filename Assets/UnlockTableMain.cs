using NewtonVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockTableMain : MonoBehaviour {

    bool unlocked1 = false;
    bool unlocked2 = false;
    bool unlocked3 = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void unlockOne()
    {
        print("sucess1");
        unlocked1 = true;
        checkUnlockAll();
    }

    public void unlockTwo()
    {
        print("sucess2");
        unlocked2 = true;
        checkUnlockAll();
    }

    public void unlockThree()
    {
        print("sucess3");
        unlocked3 = true;
        checkUnlockAll();
    }

     void checkUnlockAll()
    {
        if (unlocked1 && unlocked2 && unlocked3)
        {
            NVRInteractableItem driveScript = GetComponent<NVRInteractableItem>();
            driveScript.CanAttach = true;
        }
    }
}
