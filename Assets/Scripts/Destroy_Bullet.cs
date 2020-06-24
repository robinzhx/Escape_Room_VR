using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Destroy_Bullet : MonoBehaviour {

    private int win_count;
    public int bullets_found;
    public CountDownTimer_shorttime timer1;
    public CountDownTimer_longtime timer2;
    Scene currentScene;
    string sceneName;

	// Use this for initialization
	void Start () {
        win_count = 30;
        bullets_found = -1;

        currentScene = SceneManager.GetActiveScene();

        sceneName = currentScene.name;
	}

    // Delete this bullet if collides with pistol
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "target_bullet")
        {

            Destroy(collision.gameObject);
            bullets_found++;

            if (bullets_found == win_count)
            {
                Debug.Log("You win!");
            }

        // Start timer when hand picks up gun
        }else if (collision.gameObject.tag == "Hand")
        {
            if (sceneName == "Room_3_no_puzzle_short_time")
            {
                timer1.StartCountDown();
            }
            else if (sceneName == "Room_3_no_puzzle_long_time")
            {
                timer2.StartCountDown();
            }
        }
    }

}
