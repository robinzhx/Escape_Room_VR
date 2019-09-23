﻿using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using LSL;

namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class ProEyeGameGaze : MonoBehaviour
            {
                public GameObject lighter;
                int count = 0;
                private StreamWriter _writer;
                Vector2 pupilPos_L;
                Vector2 pupilPos_R;
                private liblsl.StreamOutlet markerStream;

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

                    //print(aGlass.Instance.aGlassStart());
                    string filename = String.Format("{1}_{0:MMddyyyy-HHmmss}{2}", DateTime.Now, "ProEyeData", ".txt");
                    string path = Path.Combine(@"C:\EscapeRoomData", filename);
                    _writer = File.CreateText(path);
                    _writer.Write("\n\n=============== Game started ================\n\n");

                    liblsl.StreamInfo inf = new liblsl.StreamInfo("ProEyeGaze", "Gaze", 4, 90, liblsl.channel_format_t.cf_float32, "sddsfsdf");
                    markerStream = new liblsl.StreamOutlet(inf);
                }

                // Update is called once per frame
                void FixedUpdate()
                {
                    if (Input.GetKeyDown(KeyCode.Escape) && (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1))
                    {
                        Application.Quit();
                    }

                    if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                        SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;
                    if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                        SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;
                    Vector3 GazeOriginCombinedLocal, GazeDirectionCombinedLocal;
                    if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                    else if (SRanipal_Eye.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                    else if (SRanipal_Eye.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                    else return;
                    Vector3 GazeDirectionCombined = Camera.main.transform.TransformDirection(GazeDirectionCombinedLocal);
                    Vector3 camPos = Camera.main.transform.position - Camera.main.transform.up * 0.05f;
                    lighter.transform.SetPositionAndRotation(camPos, Quaternion.identity);
                    lighter.transform.LookAt(Camera.main.transform.position + GazeDirectionCombined * 25);
                    
                    // Camera position: lighter.transform.position
                    // Gaze direction: lighter.transform.position

                    // print(Time.time.ToString() + " -- Eye X: " + aGlass.Instance.GetGazePoint().x + "  Y: " + aGlass.Instance.GetGazePoint().y);
                    
                    if (SRanipal_Eye.GetPupilPosition(EyeIndex.LEFT, out pupilPos_L) && SRanipal_Eye.GetPupilPosition(EyeIndex.RIGHT, out pupilPos_R))
                    {
                        _writer.WriteLine(String.Format("{0:HH:mm:ss.fff}", DateTime.Now) + " - " + Time.time.ToString() + ": EyeL " + pupilPos_L + ", EyeR: " + pupilPos_R);
                        float[] tempSample = { pupilPos_L.x, pupilPos_L.y, pupilPos_R.x, pupilPos_R.y, };
                        markerStream.push_sample(tempSample);
                        
                    }
                    else
                    {
                        Debug.Log("GetPupilPosition Failed");
                    }
                        
                    
                }

                void OnDestroy()
                {
                    _writer.Close();
                }

                public void GetPos(GameObject c, ref float cx, ref float cy)
                {
                    
                }
            }
        }
    }
}
