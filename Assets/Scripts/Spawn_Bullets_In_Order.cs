using System.Collections;
using System.Collections.Generic;
using LSL;
using UnityEngine;

public class Spawn_Bullets_In_Order : MonoBehaviour {

    public GameObject[] bullets;
    public GameObject visible_bullet_1;
    public GameObject visible_bullet_2;
    public GameObject visible_bullet_3;
    private int spawn_thirty_bullets;
    private liblsl.StreamOutlet markerStream;

    // Initialize first three bullets in room, deactivate remaining
    void Start () {

        liblsl.StreamInfo inf =
            new liblsl.StreamInfo("BulletMarker", "Markers", 1, 0, liblsl.channel_format_t.cf_string, "controller");
        markerStream = new liblsl.StreamOutlet(inf);

        for (int i = 3; i < 31; i++)
        {
            bullets[i].SetActive(false);
        }

        //bullets[0] is a freebie to acclimate users to target bullet
        visible_bullet_1 = bullets[0];
        visible_bullet_2 = bullets[1];
        visible_bullet_3 = bullets[2];

        string markerString = "Spawn Bullets at:" + visible_bullet_1.gameObject.transform.position;
        Debug.Log(markerString);
        string[] tempSample = { markerString };
        markerStream.push_sample(tempSample);

        markerString = "Spawn Bullets at:" + visible_bullet_2.gameObject.transform.position;
        Debug.Log(markerString);
        tempSample = new string[]{ markerString };
        markerStream.push_sample(tempSample);

        markerString = "Spawn Bullets at:" + visible_bullet_3.gameObject.transform.position;
        Debug.Log(markerString);
        tempSample = new string[] { markerString };
        markerStream.push_sample(tempSample);

        // Two other actual bullets are already in the room, so run 28 more times
        spawn_thirty_bullets = 28;
    }
	
	// Update is called once per frame
	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "target_bullet" && spawn_thirty_bullets > 0)
        {
            //Select next bullet 1
            if (visible_bullet_1 == null)
            {
                int nextbullet = Random.Range(3, 31);

                //Reroll if bullet is already active
                while (bullets[nextbullet] == visible_bullet_2 || bullets[nextbullet] == visible_bullet_3 || bullets[nextbullet] == null)
                {
                    nextbullet = Random.Range(3, 31);
                }
                bullets[nextbullet].SetActive(true);
                visible_bullet_1 = bullets[nextbullet];

                string markerString = "Spawn Bullets at:" + visible_bullet_1.gameObject.transform.position;
                Debug.Log(markerString);
                string[] tempSample = { markerString };
                markerStream.push_sample(tempSample);

                spawn_thirty_bullets--;
            }
            //Select next bullet 2
            if (visible_bullet_2 == null)
            {
                int nextbullet = Random.Range(3, 31);

                //Reroll if bullet is already active
                while (bullets[nextbullet] == visible_bullet_1 || bullets[nextbullet] == visible_bullet_3 || bullets[nextbullet] == null)
                {
                    nextbullet = Random.Range(3, 31);
                }
                bullets[nextbullet].SetActive(true);
                visible_bullet_2 = bullets[nextbullet];

                string markerString = "Spawn Bullets at:" + visible_bullet_2.gameObject.transform.position;
                Debug.Log(markerString);
                string[] tempSample = { markerString };
                markerStream.push_sample(tempSample);

                spawn_thirty_bullets--;
            }
            //Select next bullet 3
            if (visible_bullet_3 == null)
            {
                int nextbullet = Random.Range(3, 31);

                //Reroll if bullet is already active
                while (bullets[nextbullet] == visible_bullet_2 || bullets[nextbullet] == visible_bullet_1 || bullets[nextbullet] == null)
                {
                    nextbullet = Random.Range(3, 31);
                }
                bullets[nextbullet].SetActive(true);
                visible_bullet_3 = bullets[nextbullet];

                string markerString = "Spawn Bullets at:" + visible_bullet_3.gameObject.transform.position;
                Debug.Log(markerString);
                string[] tempSample = { markerString };
                markerStream.push_sample(tempSample);

                spawn_thirty_bullets--;
            }
            Debug.Log("Current bullets in the room are " + visible_bullet_1 + ", " + visible_bullet_2 + ", and " + visible_bullet_3);
        }
	}
}
