using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LightDir : MonoBehaviour {

    public GameObject lighter = GameObject.Find("Light");
    public GameObject vrViewer;

    private RaycastHit hit;
    private StreamWriter _writer;
    private string currObjLookAtStr = "";

    private float secondsCount = 0.0f;

    // Use this for initialization
    void Start () {
        string filename = String.Format("{1}_{0:MMddyyyy-HHmmss}{2}", DateTime.Now, "GazeRecord", ".txt");
        string path = Path.Combine(@"C:\", filename);
        _writer = File.CreateText(path);
        _writer.Write("\n\n=============== Game started ================\n\n");
    }
	
	// Update is called once per frame
	void Update () {
        lighter.transform.position = vrViewer.transform.position;

        lighter.transform.LookAt(this.gameObject.transform);

        if ( Physics.Raycast(lighter.transform.position, lighter.transform.forward, out hit) )
        {
            if (currObjLookAtStr != hit.collider.gameObject.name)
            {
                if (currObjLookAtStr != "")
                {
                    _writer.WriteLine(" : " + secondsCount);
                    secondsCount = 0.0f;
                }
                
                currObjLookAtStr = hit.collider.gameObject.name;
                print(currObjLookAtStr + " : " + secondsCount);
                _writer.Write(String.Format("{0:HH:mm:ss.fff}", DateTime.Now) + " - " + currObjLookAtStr);
            }
            secondsCount += Time.deltaTime;
            print(currObjLookAtStr + " : " + secondsCount);
        }
    }

    void OnDestroy()
    {
        _writer.WriteLine(" : " + secondsCount);
        _writer.Close();
    }
}
