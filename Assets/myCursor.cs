using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myCursor : MonoBehaviour {

    GameGaze gaze;
    GameObject cursor;
    GameObject lighter;
    float x, y;
	// Use this for initialization
	void Start () {
        cursor = GameObject.Find("Image");
        gaze = GameObject.Find("Gaze").GetComponent<GameGaze>();
        lighter = GameObject.Find("Light");
    }

    // Update is called once per frame
    void Update()
    {
        gaze.GetPos(lighter.gameObject, ref x, ref y);
        cursor.transform.localPosition = new Vector2((x - 0.5f) * 1512, (0.5f - y) * 1680);
    } 
}
