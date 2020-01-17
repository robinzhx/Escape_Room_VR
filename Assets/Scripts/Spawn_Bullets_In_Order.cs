using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Bullets_In_Order : MonoBehaviour {

    public GameObject[] bullets;
    public int count = 0;
    
	// Use this for initialization
	void Start () {
        bullets[1].SetActive(false);
        bullets[2].SetActive(false);
        bullets[3].SetActive(false);
        bullets[4].SetActive(false);
        bullets[5].SetActive(false);
        bullets[6].SetActive(false);
        bullets[7].SetActive(false);
        bullets[8].SetActive(false);
        bullets[9].SetActive(false);
    }
	
	// Update is called once per frame
	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "target_bullet")
        {
            bullets[count + 1].SetActive(true);
            count++;
            Debug.Log("Next target revealed is bullet " + count);
        }
	}
}
