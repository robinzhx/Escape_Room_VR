using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class INC_Monitor : EditorWindow
{
    int currLevel;

    int lapsTime;
    float sinceLastAction;


    List<string> sceneName;
    string[] sc = new string[2] { "Tutorial", "Room_1_v3" };

    string currGazeObj;
    string gazeStringData;
    string currSurveyResult;
    string SurveyRecordList;

    float gazeX, gazeY;
    List<Vector2> oldGazePoints;

    bool stopConfirm, isSurveyEnable;

    LightDir GazeDataProvider;
    GameGaze aGlassDataProvider;
    SurveyController SurveyDataProvider;

    GameObject panel;

    Texture eyeTex;

    private Material material;
    private Color backupColor;
    private Color currColor;
    Rect lastRect;

    Vector2 ScrollPos1;
    Vector2 ScrollPos2;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/INC Monitor")]

    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(INC_Monitor));

    }

    public void start()
    {
        currLevel = 0;
        
        sceneName = new List<string>(2);

        sc = new string[2] { "Tutorial", "Room_1_v3" };
        sceneName.Add(sc[0]);
        sceneName.Add(sc[1]);

        currGazeObj = "";
        gazeStringData = "";
        currSurveyResult = "";
        SurveyRecordList = "";

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
        
        ScrollPos1 = EditorGUILayout.BeginScrollView(ScrollPos1, GUILayout.Height(200));
        GUILayout.TextArea(gazeStringData, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();

        GUILayout.Label("aGlass Data", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("X", GUILayout.Width(15));
        GUILayout.TextField("" + gazeX);
        GUILayout.Label("Y", GUILayout.Width(15));
        GUILayout.TextField("" + gazeY);
        GUILayout.EndHorizontal();


        /*GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Box(Resources.Load("/Textures/eye") as Texture, GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.Box(Resources.Load("/Textures/eye") as Texture, GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();*/

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        backupColor = GUI.color;
        GUI.color = currColor;
        GUILayout.Box(Resources.Load("SpectatorRT") as Texture, GUILayout.Width(300), GUILayout.Height(300));
        lastRect = GUILayoutUtility.GetLastRect();
        GUI.color = backupColor;
        foreach (Vector2 g in oldGazePoints)
            if (g.x != 0 && g.y != 0)
                Handles.DrawSolidDisc(new Vector3(g.x * 500 + lastRect.center.x - 250, g.y * 300 + lastRect.center.y - 150, 0), new Vector3(0.0f, 0.0f, 1.0f), 3.0f);

        if (gazeX != 0 && gazeY != 0)
        {
            Handles.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
            Handles.DrawSolidDisc(new Vector3(gazeX * 500 + lastRect.center.x - 250, gazeY * 300 + lastRect.center.y - 150, 0), new Vector3(0.0f, 0.0f, 1.0f), 10.0f);
            Handles.color = Color.red;
            // Handles.DrawLine(new Vector3(0, 0.0f, 0), new Vector3(20.0f, 20.0f, 0));
            Handles.DrawSolidDisc(new Vector3(gazeX * 500 + lastRect.center.x - 250, gazeY * 300 + lastRect.center.y - 150, 0), new Vector3(0.0f, 0.0f, 1.0f), 5.0f);
        }
        

        GUILayout.FlexibleSpace();
        backupColor = GUI.color;
        GUI.color = currColor;
        GUILayout.Box(Resources.Load("SpectatorRT") as Texture, GUILayout.Width(300), GUILayout.Height(300));
        lastRect = GUILayoutUtility.GetLastRect();
        GUI.color = backupColor;
        GUILayout.FlexibleSpace();
        Handles.color = Color.yellow;

        foreach (Vector2 g in oldGazePoints)
            if (g.x != 0 && g.y != 0)
                Handles.DrawSolidDisc(new Vector3(g.x * 500 + lastRect.center.x - 250, g.y * 300 + lastRect.center.y - 150, 0), new Vector3(0.0f, 0.0f, 1.0f), 3.0f);

        if (gazeX != 0 && gazeY != 0)
        {
            Handles.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
            Handles.DrawSolidDisc(new Vector3(gazeX * 500 + lastRect.center.x - 250, gazeY * 300 + lastRect.center.y - 150, 0), new Vector3(0.0f, 0.0f, 1.0f), 10.0f);
            Handles.color = Color.red;
            // Handles.DrawLine(new Vector3(0, 0.0f, 0), new Vector3(20.0f, 20.0f, 0));
            Handles.DrawSolidDisc(new Vector3(gazeX * 500 + lastRect.center.x - 250, gazeY * 300 + lastRect.center.y - 150, 0), new Vector3(0.0f, 0.0f, 1.0f), 5.0f);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Survey Control", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Lapse Time(s)", GUILayout.Width(100));
        lapsTime = EditorGUILayout.DelayedIntField(lapsTime, GUILayout.Width(50));
        GUILayout.FlexibleSpace();
        GUILayout.Label("Countdown: " + (int)sinceLastAction + " s");
        GUILayout.FlexibleSpace();
        GUILayout.Label((isSurveyEnable ? "Status: On" : "Status: Off"));
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Post Survey Now"))
        {
            GameObject tmp;
            if (tmp = GameObject.Find("HeadMenus"))
            {
                tmp.GetComponent<SurveyController>().toggle(true);
                
            }
        }
        if (GUILayout.Button("Turn Off Survey"))
        {
            GameObject tmp;
            if (tmp = GameObject.Find("HeadMenus"))
            {
                tmp.GetComponent<SurveyController>().toggle(false);
            }
        }
        GUILayout.EndHorizontal();
        ScrollPos2 = EditorGUILayout.BeginScrollView(ScrollPos2, GUILayout.Height(80));
        GUILayout.TextArea(SurveyRecordList, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();

    }

    void Update()
    {
        currColor = Color.grey;
        if (EditorApplication.isPlaying && !EditorApplication.isPaused)
        {
            if (stopConfirm)
            {
                stopConfirm = false;
                gazeStringData = "";
                SurveyRecordList = "";
            }
            /*if (isSurveyEnable)
            {
                panel = Instantiate(Resources.Load("SurveyPanel") as GameObject) as GameObject;
                enableSurvey();
                isSurveyEnable = false;
            }*/

            GameObject tmp;
            if (tmp = GameObject.Find("HeadMenus"))
            {
                isSurveyEnable = tmp.GetComponent<SurveyController>().getStatus();
            }

            GetGazeData();

            if (lapsTime != 0 && !isSurveyEnable)
            {
                sinceLastAction += Time.deltaTime;

                if (sinceLastAction > lapsTime)
                {
                    if (tmp)
                    {
                        tmp.GetComponent<SurveyController>().toggle(true);
                        isSurveyEnable = tmp.GetComponent<SurveyController>().getStatus();
                    }
                    sinceLastAction -= lapsTime;
                }
            }

            oldGazePoints.Add(new Vector2(gazeX, gazeY));

            if (oldGazePoints.Count > 150)
            {
                oldGazePoints.Remove(oldGazePoints[0]);
            }

            if (gazeX == 0 || gazeY == 0)
                currColor = new Color(0.96f,0.76f,0.05f);
            Repaint();
        }
        else
        {
            gazeStringData = (gazeStringData == "" ? "Waiting for Editor to Play" : gazeStringData);
            SurveyRecordList = (SurveyRecordList == "" ? "Waiting for Editor to Play" : SurveyRecordList);
            sinceLastAction = 0;
            if (!stopConfirm)
            {
                Repaint();
                stopConfirm = true;
            }
        }
    }

    private void enableSurvey()
    {
        //panel = GameObject.Find("SurveyPanel").gameObject;
        panel.gameObject.SetActive(true);
        panel.transform.parent = GameObject.Find("Head").transform;
    }

    void GetGazeData()
    {
        GazeDataProvider = FindObjectOfType<LightDir>();
        aGlassDataProvider = FindObjectOfType<GameGaze>();
        SurveyDataProvider = FindObjectOfType<SurveyController>();
        if (aGlassDataProvider && GazeDataProvider && SurveyDataProvider)
        {
            gazeX = aGlassDataProvider.getCurrX();
            gazeY = aGlassDataProvider.getCurrY();
            string newGazeObj = GazeDataProvider.getCurrGazeObjName();
            string newSurveyResult = SurveyDataProvider.getSurveyResult();
            if (newGazeObj != currGazeObj)
            {
                currGazeObj = newGazeObj;
                gazeStringData = newGazeObj + "\n" + gazeStringData;
            }
            if (newSurveyResult != currSurveyResult)
            {
                currSurveyResult = newSurveyResult;
                gazeStringData = "*******Survey Mark******\n" + gazeStringData;
                SurveyRecordList = currSurveyResult + "\n" + SurveyRecordList;
            }
        }
    }

    void glcreate()
    {
        GUILayout.BeginHorizontal(EditorStyles.helpBox);

        // Reserve GUI space with a width from 10 to 10000, and a fixed height of 200, and 
        // cache it as a rectangle.
        Rect layoutRectangle = GUILayoutUtility.GetRect(10, 10000, 200, 200);

        if (Event.current.type == EventType.Repaint)
        {
            // If we are currently in the Repaint event, begin to draw a clip of the size of 
            // previously reserved rectangle, and push the current matrix for drawing.
            GUI.BeginClip(layoutRectangle);
            GL.PushMatrix();

            // Clear the current render buffer, setting a new background colour, and set our
            // material for rendering.
            GL.Clear(true, false, Color.black);
            if (material = new Material(Shader.Find("Hidden/Internal-Colored")))
                material.SetPass(0);
            else
                Debug.Log("error set pass");


            // Start drawing in OpenGL Quads, to draw the background canvas. Set the
            // colour black as the current OpenGL drawing colour, and draw a quad covering
            // the dimensions of the layoutRectangle.
            GL.Begin(GL.QUADS);
            GL.Color(Color.black);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(layoutRectangle.width, 0, 0);
            GL.Vertex3(layoutRectangle.width, layoutRectangle.height, 0);
            GL.Vertex3(0, layoutRectangle.height, 0);
            GL.End();

            // Start drawing in OpenGL Lines, to draw the lines of the grid.
            GL.Begin(GL.LINES);

            // Store measurement values to determine the offset, for scrolling animation,
            // and the line count, for drawing the grid.
            int offset = (Time.frameCount * 2) % 50;
            int count = (int)(layoutRectangle.width / 10) + 20;

            for (int i = 0; i < count; i++)
            {
                // For every line being drawn in the grid, create a colour placeholder; if the
                // current index is divisible by 5, we are at a major segment line; set this
                // colour to a dark grey. If the current index is not divisible by 5, we are
                // at a minor segment line; set this colour to a lighter grey. Set the derived
                // colour as the current OpenGL drawing colour.
                Color lineColour = (i % 5 == 0
                    ? new Color(0.5f, 0.5f, 0.5f) : new Color(0.2f, 0.2f, 0.2f));
                GL.Color(lineColour);

                // Derive a new x co-ordinate from the initial index, converting it straight
                // into line positions, and move it back to adjust for the animation offset.
                float x = i * 10 - offset;

                if (x >= 0 && x < layoutRectangle.width)
                {
                    // If the current derived x position is within the bounds of the
                    // rectangle, draw another vertical line.
                    GL.Vertex3(x, 0, 0);
                    GL.Vertex3(x, layoutRectangle.height, 0);
                }

                if (i < layoutRectangle.height / 10)
                {
                    // Convert the current index value into a y position, and if it is within
                    // the bounds of the rectangle, draw another horizontal line.
                    GL.Vertex3(0, i * 10, 0);
                    GL.Vertex3(layoutRectangle.width, i * 10, 0);
                }
            }

            // End lines drawing.
            GL.End();

            // Pop the current matrix for rendering, and end the drawing clip.
            GL.PopMatrix();
            GUI.EndClip();
        }

        // End our horizontal 
        GUILayout.EndHorizontal();
    }
}