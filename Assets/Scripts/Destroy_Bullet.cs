﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Bullet : MonoBehaviour {

    private int win_count;
    public int bullets_found;
    public CountDownTimer timer;

	// Use this for initialization
	void Start () {
        win_count = 10;
        bullets_found = 0;
	}

    // Delete this bullet if collides with pistol
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "target_bullet")
        {
            /*GameObject theGun = GameObject.Find("Colt Prefab");
            Spawn_Bullets_In_Order bulletsArray = theGun.GetComponent<Spawn_Bullets_In_Order>();
            bulletsArray.bullets*/
            Destroy(collision.gameObject);
            bullets_found++;

            if (bullets_found == win_count)
            {
                Debug.Log("You win!");
            }
        }else if (collision.gameObject.tag == "Hand")
        {
            timer.StartCountDown(); 
        }
    }

}
