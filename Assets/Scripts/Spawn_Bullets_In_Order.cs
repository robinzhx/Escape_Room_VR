using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Bullets_In_Order : MonoBehaviour {

    public GameObject[] bullets;
    public GameObject visible_bullet_1;
    public GameObject visible_bullet_2;
    public GameObject visible_bullet_3;

    // Initialize first three bullets in room, deactivate remaining
    void Start () {
        for (int i = 3; i < 30; i++)
        {
            bullets[i].SetActive(false);
        }
        visible_bullet_1 = bullets[0];
        visible_bullet_1 = bullets[1];
        visible_bullet_3 = bullets[2];
    }
	
	// Update is called once per frame
	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "target_bullet" && bullets.Length > 22)
        {
            if (visible_bullet_1 == null)
            {
                int nextbullet = Random.Range(3, 30);
                while (bullets[nextbullet] == visible_bullet_2 || bullets[nextbullet] == visible_bullet_3 || bullets[nextbullet] == null)
                {
                    nextbullet = Random.Range(3, 30);
                }
                bullets[nextbullet].SetActive(true);
                visible_bullet_1 = bullets[nextbullet];
            }
            if (visible_bullet_2 == null)
            {
                int nextbullet = Random.Range(3, 30);
                while (bullets[nextbullet] == visible_bullet_1 || bullets[nextbullet] == visible_bullet_3 || bullets[nextbullet] == null)
                {
                    nextbullet = Random.Range(3, 30);
                }
                bullets[nextbullet].SetActive(true);
                visible_bullet_2 = bullets[nextbullet];
            }
            if (visible_bullet_3 == null)
            {
                int nextbullet = Random.Range(3, 30);
                while (bullets[nextbullet] == visible_bullet_2 || bullets[nextbullet] == visible_bullet_1 || bullets[nextbullet] == null)
                {
                    nextbullet = Random.Range(3, 30);
                }
                bullets[nextbullet].SetActive(true);
                visible_bullet_3 = bullets[nextbullet];
            }
            Debug.Log("Current bullets in the room are " + visible_bullet_1 + ", " + visible_bullet_2 + ", and " + visible_bullet_3);
        }
	}
}
