using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Bullets_In_Order : MonoBehaviour {

    public GameObject[] bullets;
    public GameObject visible_bullet_1;
    public GameObject visible_bullet_2;
    public GameObject visible_bullet_3;
    private int spawn_thirty_bullets;

    // Initialize first three bullets in room, deactivate remaining
    void Start () {

        //bullets[0] is a freebie to acclimate users to target bullet
        visible_bullet_1 = bullets[0];
        visible_bullet_2 = bullets[1];
        visible_bullet_3 = bullets[2];

        // Two other actual bullets are already in the room, so run 28 more times
        spawn_thirty_bullets = 28;
    }
	
	// Spawn the next target bullet
	void OnCollisionEnter (Collision col) {

        if (col.gameObject.tag == "target_bullet" && spawn_thirty_bullets > 0)
        {
            //Select next bullet 1
            if (visible_bullet_1 == null)
            {
                int nextbullet = Random.Range(3, 33);

                //Reroll if bullet is already active
                while (bullets[nextbullet] == visible_bullet_2 || bullets[nextbullet] == visible_bullet_3 || bullets[nextbullet] == null)
                {
                    nextbullet = Random.Range(3, 33);
                }
                bullets[nextbullet].SetActive(true);
                visible_bullet_1 = bullets[nextbullet];
                spawn_thirty_bullets--;
            }
            //Select next bullet 2
            if (visible_bullet_2 == null)
            {
                int nextbullet = Random.Range(3, 33);

                //Reroll if bullet is already active
                while (bullets[nextbullet] == visible_bullet_1 || bullets[nextbullet] == visible_bullet_3 || bullets[nextbullet] == null)
                {
                    nextbullet = Random.Range(3, 33);
                }
                bullets[nextbullet].SetActive(true);
                visible_bullet_2 = bullets[nextbullet];
                spawn_thirty_bullets--;
            }
            //Select next bullet 3
            if (visible_bullet_3 == null)
            {
                int nextbullet = Random.Range(3, 33);

                //Reroll if bullet is already active
                while (bullets[nextbullet] == visible_bullet_2 || bullets[nextbullet] == visible_bullet_1 || bullets[nextbullet] == null)
                {
                    nextbullet = Random.Range(3, 33);
                }
                bullets[nextbullet].SetActive(true);
                visible_bullet_3 = bullets[nextbullet];
                spawn_thirty_bullets--;
            }
            Debug.Log("Current bullets in the room are " + visible_bullet_1 + ", " + visible_bullet_2 + ", and " + visible_bullet_3);
        }
	}
}
