using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCaller : MonoBehaviour {

    public GameObject key;
    public EscapeRoom_PuzzleManager unlockManager;
    public int unlockIndex = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (unlockManager != null && key != null && col.gameObject.name == key.name )
        {
            unlockManager.unlock(unlockIndex);
        } else if (unlockManager != null)
        {

            unlockManager.lockback(unlockIndex);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (unlockManager != null && key != null && col.gameObject.name == key.name)
        {
            unlockManager.unlock(unlockIndex);
        }
        else if (unlockManager != null)
        {

            unlockManager.lockback(unlockIndex);
        }
    }
}
