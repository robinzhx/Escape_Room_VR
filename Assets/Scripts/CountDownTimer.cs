using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {

    float currTime = 0f;
    float startTime = 300f;
    public Text countdownText;

	// Use this for initialization
	void Start () {
        currTime = startTime;
	}
	
	// Update is called once per frame
	void Update () {
        countdownText.color = Color.red;
        currTime -= 1 * Time.deltaTime;
        countdownText.text = currTime.ToString("0");
        if(currTime <= 0)
        {
            currTime = 0;
        }
	}
}
