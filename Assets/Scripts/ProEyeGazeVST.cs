﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LSL;
using UnityEngine;
using UnityEngine.SceneManagement;
using ViveSR.anipal.Eye;
using VRTK;

public class ProEyeGazeVST : MonoBehaviour
{
    public GameObject lighter;
    public GameObject headset;
    public SteamVR_TrackedObject chestIMU;
    int count = 0;
    private StreamWriter _writer;
    Vector2 pupilPos_L;
    Vector2 pupilPos_R;
    private liblsl.StreamOutlet markerStream;
    public string folderName = "EscapeRoomData";

    public TrackingMap trackingMap;
    public Heatmap heatMapManager;
    public float heatMapValue;
    public float heatMapThreshold = 0.0f;

    // Use this for initialization
    void Start()
    {
        if (!SRanipal_Eye_Framework.Instance.EnableEye)
        {
            enabled = false;
            return;
        }

        if (!lighter)
        {
            lighter = GameObject.Find("LightEye");
        }

        if (!headset)
        {
            headset = Camera.main.gameObject;
        }

        //print(aGlass.Instance.aGlassStart());
        string filename = String.Format("{1}_{0:MMddyyyy-HHmmss}{2}", DateTime.Now, "ProEyeData", ".txt");
        string path = Path.Combine(@"C:\" + folderName, filename);
        _writer = File.CreateText(path);
        _writer.Write("\n\n=============== Game started ================\n\n");

        // 0 - 2d coordinate of left eye
        // 2 - 2d coordinate of right eye
        // 4 - 3d direction of left eye
        // 7 - 3d direction of right eye
        // 10 - 3d position of combined hit spot
        // 13 - 3d position of head
        // 16 - 3d forward direction of head
        // 19 - 3d velocity of head
        // 22 - 3d angular velocity of head
        // 25 - left eye openness
        // 26 - right eye openness
        // 27 - 3d position of chest IMU
        // 30 - 3d forward direction of chest IMU
        // 33 - 3d velocity of chest IMU
        // 36 - Exploration percentage 
        // 37 - Pupil diameter of left eye in mm
        // 38 - Pupil diameter of right eye

        liblsl.StreamInfo inf =
            new liblsl.StreamInfo("ProEyeGaze", "Gaze", 39, 50, liblsl.channel_format_t.cf_float32,
                "ProEye");
        markerStream = new liblsl.StreamOutlet(inf);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (SceneManager.GetActiveScene().buildIndex == 0 ||
                                                 SceneManager.GetActiveScene().buildIndex == 1))
        {
            Application.Quit();
        }

        //if (heatMapManager)
        //{
        //    RaycastHit hittest;
        //    Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hittest);

        //    heatMapValue = heatMapManager.GazeAt(hittest.point, Time.fixedDeltaTime);

        //    if (heatMapThreshold > 0)
        //    {
        //        heatMapValue /= heatMapThreshold;
        //        if (heatMapValue > 1.0f)
        //        {
        //            heatMapValue = 1.0f;
        //        }
        //    }
        //    //Debug.Log(Time.fixedDeltaTime);
        //}

        if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
            SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;
        Vector3 GazeOriginCombinedLocal, GazeDirectionCombinedLocal;
        Vector3 GazeOriginLeftLocal, GazeDirectionLeftLocal;
        Vector3 GazeOriginRightLocal, GazeDirectionRightLocal;
        float leftOpenness;
        float rightOpenness;
        float leftDiameter = 0;
        float rightDiameter = 0;

        SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal);
        if (SRanipal_Eye.GetGazeRay(GazeIndex.LEFT, out GazeOriginLeftLocal, out GazeDirectionLeftLocal))
        {
            SRanipal_Eye.GetEyeOpenness(EyeIndex.LEFT, out leftOpenness);
        }
        else
        {
            leftOpenness = -1;
        }

        if (SRanipal_Eye.GetGazeRay(GazeIndex.RIGHT, out GazeOriginRightLocal, out GazeDirectionRightLocal))
        {
            SRanipal_Eye.GetEyeOpenness(EyeIndex.RIGHT, out rightOpenness);
        }
        else
        {
            rightOpenness = -1;
        }

        if (!SRanipal_Eye.GetPupilPosition(EyeIndex.LEFT, out pupilPos_L))
        {
            leftOpenness = -1;
        }

        if (!SRanipal_Eye.GetPupilPosition(EyeIndex.RIGHT, out pupilPos_R))
        {
            rightOpenness = -1;
        }

        EyeData eyeData = new EyeData();
        SRanipal_Eye.GetEyeData(ref eyeData);

        if (eyeData.verbose_data.combined.convergence_distance_validity)
        {
            Debug.Log(eyeData.verbose_data.combined.convergence_distance_mm);
        }
        leftDiameter = eyeData.verbose_data.left.pupil_diameter_mm;
        rightDiameter = eyeData.verbose_data.right.pupil_diameter_mm;

        Vector3 GazeDirectionCombined = Camera.main.transform.TransformDirection(GazeDirectionCombinedLocal);
        Vector3 camPos = Camera.main.transform.position - Camera.main.transform.up * 0.05f;
        lighter.transform.SetPositionAndRotation(camPos, Quaternion.identity);
        lighter.transform.LookAt(Camera.main.transform.position + GazeDirectionCombined * 25);

        // Camera position: lighter.transform.position
        // Gaze direction: lighter.transform.position

        // print(Time.time.ToString() + " -- Eye X: " + aGlass.Instance.GetGazePoint().x + "  Y: " + aGlass.Instance.GetGazePoint().y);

        // Debug.Log(VRTK_SDK_Bridge.GetHeadsetVelocity());
        // Debug.Log(VRTK_SDK_Bridge.GetHeadsetAngularVelocity());

        Vector3 headsetVelocity = VRTK_SDK_Bridge.GetHeadsetVelocity();
        Vector3 headsetAngVelocity = VRTK_SDK_Bridge.GetHeadsetAngularVelocity();
        Vector3 chestPosition = Vector3.zero;
        Vector3 chestForward = Vector3.forward;
        Vector3 chestVelocity = Vector3.zero;
        if (chestIMU)
        {
          chestPosition = chestIMU.transform.position;
          chestForward = chestIMU.transform.forward;
          chestVelocity = SteamVR_Controller.Input((int) chestIMU.index).velocity;
        }
        else
        {
            for (int i = 0; trackingMap && i < trackingMap.viveTrackers.Count; i++)
            {
                if (trackingMap.viveTrackers[i]?.GetComponent<SteamVR_TrackedObject>()?.index !=
                    SteamVR_TrackedObject.EIndex.None)
                {
                    chestIMU = trackingMap.viveTrackers[i].GetComponent<SteamVR_TrackedObject>();
                    break;
                }
            }
        }


        // 0 - 2d coordinate of left eye
        // 2 - 2d coordinate of right eye
        // 4 - 3d direction of left eye
        // 7 - 3d direction of right eye
        // 10 - 3d position of combined hit spot
        // 13 - 3d position of head
        // 16 - 3d forward direction of head
        // 19 - 3d velocity of head
        // 22 - 3d angular velocity of head
        // 25 - left eye openness
        // 26 - right eye openness
        // 27 - 3d position of chest IMU
        // 30 - 3d forward direction of chest IMU
        // 33 - 3d velocity of chest IMU
        // 36 - Exploration percentage 

        RaycastHit hit;
        Physics.Raycast(lighter.transform.position, lighter.transform.forward, out hit);

        if (heatMapManager)
        {
            //RaycastHit hittest;
            //Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hittest);

            heatMapValue = heatMapManager.GazeAt(hit.point, Time.fixedDeltaTime);

            if (heatMapThreshold > 0)
            {
                heatMapValue /= heatMapThreshold;
                if (heatMapValue > 1.0f)
                {
                    heatMapValue = 1.0f;
                }
            }
            //Debug.Log(Time.fixedDeltaTime);
        }

        _writer.WriteLine(String.Format("{0:HH:mm:ss.fff}", DateTime.Now) + " - " + Time.time.ToString() +
                          ": 2DEyeL " + pupilPos_L +
                          ", 2DEyeR: " + pupilPos_R +
                          ", 3DEyeL: " + GazeDirectionLeftLocal +
                          ", 3DEyeR: " + GazeDirectionRightLocal +
                          ", 3DHit: " + hit.point +
                          ", 3DHead: " + lighter.transform.position +
                          ", 3DHeadForward: " + headset.transform.forward +
                          ", 3DHeadVelocity: " + headsetVelocity +
                          ", 3DHeadAngVelocity: " + headsetAngVelocity +
                          ", LeftEyeOpenness: " + leftOpenness +
                          ", RightEyeOpenness: " + rightOpenness +
                          ", ChestPosition:" + chestPosition +
                          ", ChestForward:" + chestForward +
                          ", ChestVelocity:" + chestVelocity +
                          ", ExplorationPercentage:" + heatMapValue +
                          ", LeftPupilDiameter:" + leftDiameter + 
                          ", RightPupilDiameter:" + rightDiameter);
        float[] tempSample =
        {
            pupilPos_L.x,
            pupilPos_L.y,
            pupilPos_R.x,
            pupilPos_R.y,
            GazeDirectionLeftLocal.x,
            GazeDirectionLeftLocal.y,
            GazeDirectionLeftLocal.z,
            GazeDirectionRightLocal.x,
            GazeDirectionRightLocal.y,
            GazeDirectionRightLocal.z,
            hit.point.x,
            hit.point.y,
            hit.point.z,
            lighter.transform.position.x,
            lighter.transform.position.y,
            lighter.transform.position.z,
            headset.transform.forward.x,
            headset.transform.forward.y,
            headset.transform.forward.z,
            headsetVelocity.x,
            headsetVelocity.y,
            headsetVelocity.z,
            headsetAngVelocity.x,
            headsetAngVelocity.y,
            headsetAngVelocity.z,
            leftOpenness,
            rightOpenness,
            chestPosition.x,
            chestPosition.y,
            chestPosition.z,
            chestForward.x,
            chestForward.y,
            chestForward.z,
            chestVelocity.x,
            chestVelocity.y,
            chestVelocity.z,
            heatMapValue,
            leftDiameter,
            rightDiameter
        };
        markerStream.push_sample(tempSample);
    }

    void OnDestroy()
    {
        _writer.Close();
    }

    public void GetPos(GameObject c, ref float cx, ref float cy)
    {
    }
}