using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class INC_Monitor : EditorWindow
{
    int currLevel;

    int lapsTime;


    List<string> sceneName;
    string[] sc;

    string currGazeObj;
    string gazeStringData;

    float gazeX, gazeY;

    bool stopConfirm, isSurveyEnable;

    LightDir GazeDataProvider;
    GameGaze aGlassDataProvider;

    GameObject panel;

    Texture eyeTex;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/INC Monitor")]

    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(INC_Monitor));
    }

    public void Awake()
    {
        currLevel = 0;
        
        sceneName = new List<string>(2);

        sc = new string[2] { "Tutorial", "Room_1_v3" };
        sceneName.Add(sc[0]);
        sceneName.Add(sc[1]);

        currGazeObj = "";
        gazeStringData = "";

        gazeX = 0.0f;
        gazeY = 0.0f;

        lapsTime = 60;

        stopConfirm = false;
        isSurveyEnable = false;

        eyeTex = new Texture2D(200, 200);
    }

    void OnGUI()
    {
        

        GUILayout.Label("Scene List", EditorStyles.boldLabel);
        
        for (int i = 0; i < sceneName.Count; i++)
        {
            sceneName[i] = EditorGUILayout.DelayedTextField("Scene " + i, sceneName[i]);
        }
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Scene", GUILayout.Width(100)))
        {
            sceneName.Add(string.Empty);
        }
        if (GUILayout.Button("Remove Scene", GUILayout.Width(100)))
        {
            sceneName.RemoveAt(sceneName.Count - 1);
        }
        GUILayout.EndHorizontal();

        //GUILayout.Label("Scene List", EditorStyles.boldLabel);

        currLevel = EditorGUILayout.Popup("Scene Select", currLevel, sceneName.ToArray());
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Load Selected Scene", GUILayout.Width(300)))
        {
            SteamVR_LoadLevel.Begin(sceneName[currLevel], false, 1.0f);
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("Gaze Object Data", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("TimeStamp");
        GUILayout.Label("StartTime");
        GUILayout.Label("ObjectName");
        GUILayout.EndHorizontal();
        GUILayout.TextArea(gazeStringData, GUILayout.Height(200));

        GUILayout.Label("aGlass Data", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("X", GUILayout.Width(15));
        GUILayout.TextField("" + gazeX);
        GUILayout.Label("Y", GUILayout.Width(15));
        GUILayout.TextField("" + gazeY);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Box(Resources.Load("/Textures/eye") as Texture, GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.Box(Resources.Load("/Textures/eye") as Texture, GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Label("Survey Control", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        EditorGUILayout.DelayedTextField("Loop Lap Time(s)", lapsTime.ToString());
        if (GUILayout.Button("Post Survey Now"))
        {
            isSurveyEnable = true;
        }
        GUILayout.EndHorizontal();
    }

    void Update()
    {

        if (EditorApplication.isPlaying && !EditorApplication.isPaused)
        {
            if (stopConfirm)
            {
                stopConfirm = false;
                gazeStringData = "";
            }
            if (isSurveyEnable)
            {
                enableSurvey();
            }
            
            GetGazeData();
            Repaint();
        }
        else
        {
            gazeStringData = "Waiting for Editor to Play";
            if (!stopConfirm)
            {
                Repaint();
                stopConfirm = true;
            }
        }
    }

    private void enableSurvey()
    {
        panel = GameObject.Find("SurveyPanel").gameObject;
        panel.gameObject.SetActive(true);
        panel.transform.position = GameObject.Find("Head").transform.position + new Vector3(0, 0, 1.0f);
    }

    void GetGazeData()
    {
        GazeDataProvider = FindObjectOfType<LightDir>();
        aGlassDataProvider = FindObjectOfType<GameGaze>();
        gazeX = aGlassDataProvider.getCurrX();
        gazeY = aGlassDataProvider.getCurrX();
        string newGazeObj = GazeDataProvider.getCurrGazeObjName();
        if (newGazeObj != currGazeObj)
        {
            currGazeObj = newGazeObj;
            gazeStringData = newGazeObj + "\n" + gazeStringData;
        }
    }
}