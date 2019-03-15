using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class INC_inGameControl : MonoBehaviour {

    public InputField surveyTimerSetInput;
    public Text surveyCountdownText;
    public Text onoffStatusText;
    public Text surveyResultText;
    public SurveyController SurveyDataProvider;

    public int lapsTime;
    public bool isSurveyEnable;

    float sinceLastAction;
    string SurveyRecordList = "";
    string currSurveyResult = "";

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        
        if (SurveyDataProvider)
        {
            isSurveyEnable = SurveyDataProvider.getStatus();

            if (lapsTime != 0 && !isSurveyEnable)
            {
                sinceLastAction += Time.deltaTime;

                if (sinceLastAction > lapsTime)
                {

                    SurveyDataProvider.GetComponent<SurveyController>().toggle(true, 0);
                    isSurveyEnable = SurveyDataProvider.GetComponent<SurveyController>().getStatus();
                    sinceLastAction -= lapsTime;
                }
            }

            string newSurveyResult = SurveyDataProvider.getSurveyResult();
            if (newSurveyResult != currSurveyResult)
            {
                currSurveyResult = newSurveyResult;
                SurveyRecordList = currSurveyResult + SurveyRecordList;
            }
        }

        SurveyRecordList = (SurveyRecordList == "" ? "Waiting for data" : SurveyRecordList);
    }

    private void LateUpdate()
    {
        surveyCountdownText.text = ((int)sinceLastAction).ToString();
        surveyResultText.text = SurveyRecordList;
        onoffStatusText.text = (isSurveyEnable ? "On" : "Off");
        onoffStatusText.color = (isSurveyEnable ? Color.green : Color.red);
    }

    public void toggleHeadSurvey(bool b)
    {
        SurveyDataProvider.toggle(b);
    }
}
