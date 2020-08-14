using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCaller : MonoBehaviour {

    public GameObject key;
    public GameObject[] nonKeys;
    public EscapeRoom_PuzzleManager unlockManager;
    public int unlockIndex = 0;

    private List<string> nonKeyNames = new List<string>();

    // Use this for initialization
    void Start () {
		foreach(var nonKey in nonKeys)
        {
            nonKeyNames.Add(nonKey.name);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //void OnCollisionEnter(Collision col)
    //{
    //    if (unlockManager != null && key != null && col.gameObject.name == key.name )
    //    {
    //        unlockManager.unlock(unlockIndex);
    //    } else if (unlockManager != null)
    //    {

    //        unlockManager.lockback(unlockIndex);
    //    }
    //}

    void OnTriggerEnter(Collider col)
    {
        
        if (unlockManager != null && key != null && col.gameObject.name == key.name)
        {
            unlockManager.unlock(unlockIndex);
        }
        else if (unlockManager != null)
        {
            if (nonKeyNames.Exists(x => x == col.name))
            {
                Debug.Log(col.name);
                unlockManager.lockback(unlockIndex);
            }
        }
    }
}
