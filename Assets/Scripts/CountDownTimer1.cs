using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer1 : MonoBehaviour {

    float currTime = 0f;
    float startTime = 300f;
    public Text countdownText;
    public AudioClip clockNormal;
    public AudioClip clockFaster;
    public AudioClip clockFail;
    public AudioClip clockSuccess;
    private AudioManager theAM;
    bool countdown = false;
    bool gunFound = false;
    GameObject gun;

	// Use this for initialization
	void Start () {

        currTime = startTime;
        countdownText.color = Color.black;
        countdownText.text = currTime.ToString("0");
        gun = GameObject.Find("Colt Prefab");
        theAM = FindObjectOfType<AudioManager>();
    }

    // Change room based on timer
    void Update () {

        // Counting down time
        if (countdown == true)
        {
            currTime -= 1 * Time.deltaTime;
            countdownText.text = currTime.ToString("0");
        }

        // 1 min left
        if(currTime < 60)
        {
            countdownText.color = Color.red;
            if (clockFaster != null)
            {
                theAM.ChangeClockLoop(clockFaster);
            }
        }

        // 0 sec left
        if(currTime <= 0)
        {
            currTime = 0;

            if (clockFail != null)
            {
                theAM.ChangeClock(clockFail);
            }

            enabled = false;
        }

        // Stop timer when all targets are found
        if(gun.GetComponent<Destroy_Bullet>().bullets_found == 30)
        {
            countdown = false;
            if (clockSuccess != null)
            {
                theAM.ChangeClock(clockSuccess);
            }

            enabled = false;
        }
	}

    // Start count down when hand grabs gun
    public void StartCountDown()
    {
        Debug.Log("Hand is touching");
        if (gunFound == false)
        {
            countdown = true;
            if (clockNormal != null)
            {
                theAM.ChangeClockLoop(clockNormal);
            }
            gunFound = true;
        }
    }

    // Start count down if anything other than hand touches gun first
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Trigger");

        if (other.gameObject.tag == "Hand" && countdown == false)
        {
            Debug.Log("Hand is touching");
            if (gunFound == false)
            {
                countdown = true;
                if (clockNormal != null)
                {
                    theAM.ChangeClockLoop(clockNormal);
                }
                gunFound = true;
            }
        }
    }
}
