using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SurveyController : MonoBehaviour {

    public bool isEnable = false;
    public GameObject surveyObject;
    private StreamWriter _writer;
    private string SurveyResult = "";

    public GameObject mood;
    public GameObject intensity;

    // Use this for initialization
    void Start () {
        //gameObject.SetActive(isEnable);
        string filename = String.Format("{1}_{0:MMddyyyy-HHmmss}{2}", DateTime.Now, "SurveyRecord", ".txt");
        Directory.CreateDirectory(@"C:\EscapeRoomData");
        string path = Path.Combine(@"C:\EscapeRoomData", filename);
        _writer = File.CreateText(path);
        _writer.Write("=============== Game started ================\n\n");
    }
	
	// Update is called once per frame
	void Update () {
        surveyObject.SetActive(isEnable);
        //if (gameObject.activeSelf != isEnable)
        //    gameObject.SetActive(isEnable);
    }

    void OnDestroy()
    {
        print("\n\n=============== Game Ended ================");
        _writer.Close();
    }

    public bool getStatus()
    {
        return surveyObject.activeSelf;
    }

    public void toggle(bool b)
    {
        isEnable = b;
    }

    public void report()
    {
        SurveyResult = String.Format(String.Format("{0:HH:mm:ss.fff}", DateTime.Now)
                                            + "\t" + mood.GetComponent<UnityEngine.UI.Text>().text 
                                            + "\t" + intensity.GetComponent<UnityEngine.UI.Text>().text);
        _writer.Write(SurveyResult);
        isEnable = false;
    }

    public string getSurveyResult()
    {
        return SurveyResult;
    }

}
