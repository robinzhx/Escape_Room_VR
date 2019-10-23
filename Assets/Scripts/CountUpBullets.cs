using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountUpBullets : MonoBehaviour {

    public Text countUpText;
    GameObject gun;
    
    // Use this for initialization
    void Start () {
        gun = GameObject.Find("Colt");
	}
	
	// Update is called once per frame
	void Update () {
        countUpText.text = gun.GetComponent<Destroy_Bullet>().bullets_found.ToString("0");
    }
}
