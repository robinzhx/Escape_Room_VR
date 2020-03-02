using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Bullets_In_Order : MonoBehaviour {

    public GameObject[] bullets;
    public GameObject i1;
    public GameObject i2;
    public GameObject i3;

    // Initialize first three bullets in room, deactivate remaining
    void Start () {
        for (int i = 3; i < 10; i++)
        {
            bullets[i].SetActive(false);
        }
        i1 = bullets[0];
        i2 = bullets[1];
        i3 = bullets[2];
    }
	
	// Update is called once per frame
	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "target_bullet" && bullets.Length > 2)
        {
            if (i1 == null)
            {
                int nextbullet = Random.Range(3, 10);
                while (bullets[nextbullet] == i2 || bullets[nextbullet] == i3 || bullets[nextbullet] == null)
                {
                    nextbullet = Random.Range(3, 10);
                }
                bullets[nextbullet].SetActive(true);
                i1 = bullets[nextbullet];
            }
            if (i2 == null)
            {
                int nextbullet = Random.Range(3, 10);
                while (bullets[nextbullet] == i1 || bullets[nextbullet] == i3 || bullets[nextbullet] == null)
                {
                    nextbullet = Random.Range(3, 10);
                }
                bullets[nextbullet].SetActive(true);
                i2 = bullets[nextbullet];
            }
            if (i3 == null)
            {
                int nextbullet = Random.Range(3, 10);
                while (bullets[nextbullet] == i2 || bullets[nextbullet] == i1 || bullets[nextbullet] == null)
                {
                    nextbullet = Random.Range(3, 10);
                }
                bullets[nextbullet].SetActive(true);
                i3 = bullets[nextbullet];
            }
            Debug.Log("Current bullets in the room are " + i1 + ", " + i2 + ", and " + i3);
        }
	}
}
