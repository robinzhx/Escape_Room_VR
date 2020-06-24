using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer_shorttime : MonoBehaviour {

    float currTime = 0f;
    float startTime = 480f;
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

    // Update is called once per frame
    void Update () {

        if (countdown == true)
        {
            currTime -= 1 * Time.deltaTime;
            countdownText.text = currTime.ToString("0");
        }
        if(currTime < 60)
        {
            countdownText.color = Color.red;
        }
        if(currTime <= 0)
        {
            currTime = 0;
        }
        if(gun.GetComponent<Destroy_Bullet>().bullets_found == 30)
        {
            countdown = false;
        }
	}

    public void StartCountDown()
    {
        Debug.Log("Hand is touching");
        countdown = true;
    }

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
