using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.UI;
using Valve.VR;
using VRTK;

public class ReplayManager : MonoBehaviour
{
    #region Serialization

    [System.Serializable]
    public class Frame : System.Object
    {
        // 4 + 4 + 4 * 3 + 4 * 4 = 36
        [SerializeField] public string uniqueID;
        [SerializeField] public float timestamp;
        [SerializeField] public Vector3 position;
        [SerializeField] public Quaternion rotation;
    }

    private BinaryFormatter bf;
    private SurrogateSelector surrogateSelector;
    private Vector3Surrogate vector3Surrogate;
    private QuaternionSurrogate quaternionSurrogate;

    #endregion

    #region Enum

    public enum Mode
    {
        Replay,
        Record,
        ModeCount
    }

    public enum State
    {
        Play,
        Pause,
        Stop,
        StateCount
    }

    #endregion
    
    //[HideInInspector]
    #region Filename Variable

    private string replayFileName = "";
    private string defaultFileName = "Replay.dem";
    private string currentFileName = "";

    #endregion

    #region State Variable

    private Mode currentMode = Mode.Replay;
    private State currentState = State.Stop;

    #endregion

    #region Debug Variable

    private string lastError = "Success";
    private bool isDebug = true;

    #endregion

    #region Progess Variable
    
    private float currentTime;
    private float startTime = -1;
    private string uniqueID;
    private float minTime;
    private float maxTime;

    private bool needRelocate = false;

    #endregion

    #region Replay Frame Variable

    private int currIndex = -1;
    
    private float speed = 1.0f;
    private float minSpeed = 0.01f;
    private float maxSpeed = 10.0f;

    #endregion

    #region Old Garbage
    public bool isRecording = true;
    public bool isReplay = false;
    public bool isReplayInitialized = false;

    public float deltaTime = -1;

    public int skip = 1;

    public bool forcedUpdate = false;
    public bool timeSliderEnabled = false;
    public bool useScale = false;
    
    public Slider slider;

    // to take over camera
    public GameObject camera;

    // to take over left Controller
    public GameObject leftController;

    // to take over right Controller
    public GameObject rightController;

    public SteamVR_RenderModel rightHand;

    [ReadOnly] public bool initialized = false;
    [ReadOnly] public bool saved = true;
    
    [ReadOnly] private List<UniqueId> gameObjectList;

    [SerializeField] public List<List<Frame>> frameList;
    private List<bool> isChanged;

    [ReadOnly] private List<int> indices;


    private int count = 0;

    
   

    private int skipFirstFewFrame = 15;
    private int currentSkip = 0;

    #endregion

    #region Public Interface

    public string GetLastError(bool reset = true)
    {
        string temp = lastError;
        if (reset)
        {
            lastError = "Success";
        }
        return temp;
    }

    #endregion

    #region Private Helper Function

    void DebugPrint(string s)
    {
        if (isDebug)
        {
           Debug.Log(s);
        }
    }

    void LoadReplay(string filename)
    {
        FileStream file;
        try
        {
            file = File.Open(Application.dataPath + "/Scripts/Replay/" + filename, FileMode.Open);

            Debug.Log("Load from: " + Application.dataPath + "/Scripts/Replay/" + filename);
            frameList = (List<List<Frame>>) bf.Deserialize(file);
            minTime = frameList[0][0].timestamp;
            maxTime = frameList[0][frameList[0].Count - 1].timestamp;
            foreach (var frames in frameList)
            {
                minTime = Mathf.Min(minTime, frames[0].timestamp);
                maxTime = Mathf.Max(maxTime, frames[frames.Count - 1].timestamp);
            }

            currentFileName = filename;
            file.Close();
        }
        catch (IOException)
        {
            lastError = "LoadReplay: IOException - fail to load file " + filename;
        }
    }

    void ReLocate()
    {


        needRelocate = false;
    }

    #endregion

    #region UI Function

    public void SetProgress(Single progress)
    {
        if (currentMode == Mode.Replay)
        {
            if (progress > 100 || progress < 0)
            {
                lastError = "SetProgress: Invalid progress";
            }
            else
            {
                float factor = progress / 100.0f;
                currentTime = minTime + factor * (maxTime - minTime);
                needRelocate = true;
            }
        }
    }

    public void SetState(Int32 state)
    {
        DebugPrint("SetState to " + state);
        if (state >= 0 && state < (int) State.StateCount)
        {
            currentState = (State) state;
        }
        else
        {
            lastError = "SetState: Invalid state";
        }
    }

    public void SetMode(Int32 mode)
    {
        DebugPrint("SetMode to " + mode);
        if (mode >= 0 && mode < (int) Mode.ModeCount)
        {
            currentMode = (Mode) mode;
        }
        else
        {
            lastError = "SetMode: Invalid mode";
        }
    }

    public void SetFileName(string s)
    {
        DebugPrint("SetFileName to " + s);
        replayFileName = s;
    }

    public void SaveCurrentDemo()
    {
    }

    public void LoadDemo()
    {
        if (replayFileName == "")
        {
            LoadReplay("Replay.dem");
        }
        else
        {
            LoadReplay(replayFileName);
        }
    }

    #endregion

    #region Unity Function

    // Use this for initialization
    void Start()
    {
        if (!GetComponent<UniqueId>())
        {
            UniqueId u = this.gameObject.AddComponent<UniqueId>();
            if (string.IsNullOrEmpty(u.guid))
            {
                Guid guid = Guid.NewGuid();
                u.guid = guid.ToString();
            }

            Debug.Log("Add UniqueID " + u.guid + " to: " + this.name);
        }

        saved = true;
        Debug.Log("My instance ID:" + this.gameObject.GetComponent<UniqueId>().guid);
        uniqueID = this.gameObject.GetComponent<UniqueId>().guid;
        gameObjectList = new List<UniqueId>();
        frameList = new List<List<Frame>>();

        // serializer 
        bf = new BinaryFormatter();
        surrogateSelector = new SurrogateSelector();
        vector3Surrogate = new Vector3Surrogate();
        quaternionSurrogate = new QuaternionSurrogate();
        surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All),
            vector3Surrogate);
        surrogateSelector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All),
            quaternionSurrogate);
        bf.SurrogateSelector = surrogateSelector;

        //if (isReplay)
        //{
        //    LoadReplay("Replay.dem");
        //}

        count = skip;
    }
    
    void FixedUpdate()
    {
        if (currentMode == Mode.Record)
        {
            if (currentState == State.Play)
            {
                currentTime += Time.fixedDeltaTime;
            }
        }

        //Record();
        //Replay();
    }

    void LateUpdate()
    {
        if (currentMode == Mode.Replay)
        {
            if (currentState == State.Play)
            {
                currentTime += Time.fixedDeltaTime;
            }
        }

        //Replay();
    }

    #endregion

    void UpdateScene(bool isReplay)
    {
        UniqueId[] gameObjectList = Resources.FindObjectsOfTypeAll<UniqueId>();
        foreach (var gameObject in gameObjectList)
        {
            if (gameObject.guid == "")
            {
                Debug.LogWarning("This object has problematic guid: " + gameObject.name + ": " + gameObject.guid);
            }

            if (gameObject.guid != uniqueID && gameObject.tag != "ignore" && gameObject.guid != "")
            {
                if (!this.gameObjectList.Contains(gameObject))
                {
                    Debug.Log("Adding " + gameObject.name + ": " + gameObject.guid);
                    this.gameObjectList.Add(gameObject);
                    gameObject.transform.hasChanged = false;
                    if (isReplay)
                    {
                    }
                    else
                    {
                        this.frameList.Add(new List<Frame>());
                    }
                }
            }
        }
    }


    void Record()
    {
        if (isRecording)
        {
            if (count > 0)
            {
                count--;
                return;
            }
            else
            {
                count = skip;
            }

            if (saved)
            {
                saved = false;
            }

            // timestamp
            currentTime = Time.fixedUnscaledTime;

            if (!initialized || forcedUpdate)
            {
                UpdateScene(false);
                if (!initialized)
                {
                    isChanged = Enumerable.Repeat(false, frameList.Count).ToList();
                    saved = false;
                    startTime = currentTime;
                    initialized = true;
                }
            }

            for (int i = 0; i < gameObjectList.Count; i++)
            {
                GameObject gameObject = gameObjectList[i].gameObject;
                if (gameObject.transform.hasChanged)
                {
                    if (!isChanged[i] && frameList[i].Count > 1)
                    {
                        var diff = gameObject.transform.position - frameList[i][0].position;
                        if (diff.magnitude >= 0.05)
                        {
                            isChanged[i] = true;
                        }
                    }

                    Frame frame = new Frame
                    {
                        timestamp = currentTime - startTime,
                        uniqueID = gameObject.GetComponent<UniqueId>().guid,
                        position = gameObject.transform.position,
                        rotation = gameObject.transform.rotation
                    };
                    frameList[i].Add(frame);

                    gameObject.transform.hasChanged = false;
                }
            }
        }
        else
        {
            if (!saved)
            {
                Debug.Log("The size of replay is: " + frameList.Count * frameList[0].Count);

                var list = new List<List<Frame>>();
                for (int i = 0; i < frameList.Count; i++)
                {
                    if (isChanged[i])
                    {
                        list.Add(frameList[i]);
                    }
                }

                FileStream file = File.Create(Application.dataPath + "/Scripts/Replay/Replay.dem");
                Debug.Log("Save to: " + Application.dataPath + "/Scripts/Replay/Replay.dem");
                if (bf != null) bf.Serialize(file, list);
                file.Close();

                saved = true;
            }
        }
    }

    void Replay()
    {
        if (isReplay && !isRecording)
        {
            currentTime = Time.unscaledTime;

            // Hacky Code to deal with steamvr tracking
            if (currentSkip > skipFirstFewFrame)
            {
                var c = camera.GetComponent<VRTK_TransformFollow>();
                if (c != null)
                {
                    c.enabled = false;
                }

                var lc = leftController.GetComponent<SteamVR_TrackedObject>();
                if (lc != null)
                {
                    lc.enabled = false;
                }

                var rc = rightController.GetComponent<SteamVR_TrackedObject>();
                if (rc != null)
                {
                    rc.enabled = false;
                }

                //rightHand.renderModelName = "vr_controller_vive_1_5";
                //rightHand.SetDeviceIndex(3);
                //rightController.inputSource = SteamVR_Input_Sources.Waist;
            }
            else
            {
                currentSkip++;
            }

            if (!initialized)
            {
                indices = Enumerable.Repeat(0, frameList.Count).ToList();
                UpdateScene(true);
                initialized = true;
            }

            if (!isReplayInitialized)
            {
                startTime = currentTime;
                for (int i = 0; i < indices.Count; i++)
                {
                    indices[i] = 0;
                }

                isReplayInitialized = true;
            }

            if (!timeSliderEnabled)
            {
                deltaTime = currentTime - startTime;
                //slider.value = deltaTime;
            }
            else
            {
                //deltaTime = slider.value;
            }

            for (int i = 0; i < frameList.Count; i++)
            {
                string guid = frameList[i][0].uniqueID;
                UniqueId currId = gameObjectList.Find(x => x.guid == guid);
                if (currId == null)
                {
                    // the gameObject corresponding to current frameList is no longer exist
                    continue;
                }

                GameObject curr = currId.gameObject;

                Rigidbody r = curr.GetComponent<Rigidbody>();
                if (r)
                {
                    r.isKinematic = true;
                }

                // Start from index
                List<Frame> frames = this.frameList[i];
                for (int j = indices[i]; j < frames.Count; j++)
                {
                    Frame currFrame = frames[j];
                    if (currFrame.timestamp < deltaTime)
                    {
                        continue;
                    }

                    // Correct Frame (j and j - 1)
                    if (j > 0)
                    {
                        // Interpolate j and j - 1 frame
                        Frame lastFrame = frames[j - 1];

                        // Calculate (s)lerp factor
                        float factor = (deltaTime - lastFrame.timestamp) / (currFrame.timestamp - lastFrame.timestamp);

                        // (s)lerp
                        curr.transform.position = Vector3.LerpUnclamped(lastFrame.position, currFrame.position, factor);
                        curr.transform.rotation =
                            Quaternion.LerpUnclamped(lastFrame.rotation, currFrame.rotation, factor);
                    }
                    else
                    {
                        // first key (no interpolation needed)
                        curr.transform.position = currFrame.position;
                        curr.transform.rotation = currFrame.rotation;
                    }

                    indices[i] = j;
                    break;
                }
            }
        }
        else
        {
            isReplayInitialized = false;
            if (indices != null)
            {
                for (int i = 0; i < indices.Count; i++)
                {
                    indices[i] = 0;
                }
            }
        }
    }
}