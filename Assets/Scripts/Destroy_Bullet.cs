using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Bullet : MonoBehaviour {

    private int win_count;
    public int bullets_found;
    public CountDownTimer timer;

	// Use this for initialization
	void Start () {
        win_count = 30;
        bullets_found = -1;
	}

    // Delete target bullet if collides with pistol
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "target_bullet")
        {
            Destroy(collision.gameObject);
            bullets_found++;
        }
    }

}
