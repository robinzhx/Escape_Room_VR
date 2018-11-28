using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SurveyController : MonoBehaviour {

    
    public int whichToEnable = 0;
    public GameObject[] surveyObject = new GameObject[3];
    bool[] isEnable = {false, false, false};
    private StreamWriter _writer;
    private string SurveyResult = "";

    public GameObject mood;
    public GameObject intensity;
    public GameObject experience;
    public GameObject flow;

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

        surveyObject[0].SetActive(isEnable[0]);
        surveyObject[1].SetActive(isEnable[1]);
        surveyObject[2].SetActive(isEnable[2]);
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
        bool isActive = false;
        foreach (GameObject obj in surveyObject) {
            isActive = isActive || obj.activeSelf;
        }
        return isActive;
    }

    public void toggle(bool b, int choice)
    {
        isEnable[choice] = b;
    }

    public void report()
    {
        SurveyResult = String.Format(String.Format("{0:HH:mm:ss.fff}", DateTime.Now)
                                            + "\tm:" + mood.GetComponent<UnityEngine.UI.Text>().text 
                                            + " " + intensity.GetComponent<UnityEngine.UI.Text>().text
                                            + "\te:" + experience.GetComponent<UnityEngine.UI.Text>().text
                                            + "\tf:" + flow.GetComponent<UnityEngine.UI.Text>().text);
        _writer.Write(SurveyResult);
        isEnable[0] = false;
        isEnable[1] = false;
        isEnable[2] = false;
    }

    public void nextpanel()
    {
        int i = 0;
        while (i < isEnable.Length)
        {
            if (isEnable[i] == true)
            {
                toggle(false, i);
                i++;
                break;
            }
            i++;
        }
        if (i < isEnable.Length)
        {
            isEnable[i] = true;
        }
        else
        {
            i = 0;
            report();
        }
    }

    public string getSurveyResult()
    {
        return SurveyResult;
    }

}
