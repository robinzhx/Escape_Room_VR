using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCameraManager : MonoBehaviour {
    
    public GameObject FOV_Obj;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setFOVvisible (bool b)
    {
        if (FOV_Obj)
            FOV_Obj.SetActive(b);
    }
}
