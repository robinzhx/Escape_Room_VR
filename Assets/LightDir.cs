using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDir : MonoBehaviour {

    GameObject lighter;
    public GameObject vrViewer;
	// Use this for initialization
	void Start () {
        lighter = GameObject.Find("Light");
	}
	
	// Update is called once per frame
	void Update () {
        lighter.transform.position = vrViewer.transform.position;

        lighter.transform.LookAt(this.gameObject.transform);
    }
}
