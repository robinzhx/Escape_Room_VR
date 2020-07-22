using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {

    float currTime = 0f;
    float startTime = 600f;
    public Text countdownText;
    bool countdown = false;
    GameObject gun;

	// Use this for initialization
	void Start () {

        currTime = startTime;
        countdownText.color = Color.black;
        countdownText.text = currTime.ToString("0");
        gun = GameObject.Find("Colt Prefab");
    }

    // Change room based on timer
    void Update () {

        // Print time
        if (countdown == true)
        {
            currTime -= 1 * Time.deltaTime;
            countdownText.text = currTime.ToString("0");
        }

        // 1 min left is red
        if(currTime < 60)
        {
            countdownText.color = Color.red;
        }

        // Timer is 0
        if(currTime <= 0)
        {
            currTime = 0;
        }

        // Stop timer when all targets are found
        if(gun.GetComponent<Destroy_Bullet>().bullets_found == 30)
        {
            countdown = false;
        }
	}

    // Start count down when hand grabs gun
    public void StartCountDown()
    {
        Debug.Log("Hand is touching");
        countdown = true;
    }

    // Error check if anything other than hand touches gun first
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Trigger");

        if (other.gameObject.tag == "Hand" && countdown == false)
        {
            Debug.Log("Hand is touching");
            countdown = true;
        }
    }
}
