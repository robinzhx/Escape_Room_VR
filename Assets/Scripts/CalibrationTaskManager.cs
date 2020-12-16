using System.Collections;
using System.Collections.Generic;
using LSL;
using NewtonVR;
using UnityEngine;

public class CalibrationTaskManager : MonoBehaviour
{
    public enum State
    {
        Stop,
        Center,
        ShowTarget
    }

    public int trial_count = 120;
    public float odd = 0.2f;

    public float duration = 1.0f;

    public float rest_duration = 1.5f;

    public float starting_time = 0;

    public bool started = false;

    public bool r1Enable = true;
    public List<GameObject> r1Cubes = new List<GameObject>();
    public bool r2Enable = true;
    public List<GameObject> r2Cubes = new List<GameObject>();
    public bool r3Enable = true;
    public List<GameObject> r3Cubes = new List<GameObject>();

    public GameObject targetSign;
    public GameObject deviantSign;

    private Random r_index;
    private Random r_odd;

    private liblsl.StreamOutlet markerStream;

    private State currentState = State.Stop;
    private bool switching = false;
    public float nextSwitchTime = 0;
    private int ring_index = 0;
    private int trial = 0;

    private int cube_index = 0;
    private bool isTarget = false;

    public bool paused = false;

    public NVRHand left;
    public NVRHand right;

    private List<List<GameObject>> rings = new List<List<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        liblsl.StreamInfo inf = new liblsl.StreamInfo("EventMarker", "Markers", 1, 0, liblsl.channel_format_t.cf_string, "giu4569");
        markerStream = new liblsl.StreamOutlet(inf);
        r_index = new Random();
        r_odd = new Random();
        targetSign.SetActive(false);
        deviantSign.SetActive(false);
        if (r1Enable)
        {
            rings.Add(r1Cubes);
        }

        if (r2Enable)
        {
            rings.Add(r2Cubes);
        }

        if (r3Enable)
        {
            rings.Add(r3Cubes);
        }
    }

    public void StartCalibration()
    {
        started = true;
        switching = true;
        paused = false;
        currentState = State.Stop;
        ring_index = 0;
        trial = 0;
        starting_time = Time.time;
        string[] tempSample;
        tempSample = new string[] { "Calibration Start" };
        markerStream.push_sample(tempSample);
    }

    public void SetPause(bool pause)
    {
        paused = pause;

        string[] tempSample;
        if (paused)
        {
            tempSample = new string[] { "Paused" };
            markerStream.push_sample(tempSample);
        }
        else
        {
            tempSample = new string[] { "Resumed" };
            markerStream.push_sample(tempSample);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Send trigger event
        if (left)
        {
            if (left.HoldButtonDown)
            {
                string[] tempSample;
                tempSample = new string[] { "Left Trigger is holded" };
                Debug.Log("left holded");
                markerStream.push_sample(tempSample);
            }
        }

        if (right)
        {
            if (right.HoldButtonDown)
            {
                string[] tempSample;
                tempSample = new string[] { "Right Trigger is holded" };
                markerStream.push_sample(tempSample);
            }
        }


        // Logic
        if (started)
        {
            if (paused)
            {
                nextSwitchTime += Time.deltaTime;
                return;
            }

            switch (currentState)
            {
                case State.Stop:
                    if (switching)
                    {
                        nextSwitchTime = Time.time + rest_duration;
                        switching = false;
                    }

                    if (Time.time > nextSwitchTime)
                    {
                        currentState = State.Center;
                        switching = true;
                    }
                    break;
                case State.Center:
                    if (switching)
                    {
                        nextSwitchTime = Time.time + rest_duration;
                        switching = false;
                    }

                    if (Time.time > nextSwitchTime)
                    {
                        if (trial >= trial_count)
                        {
                            // Increment ring
                            trial = 0;
                            ring_index += 1;
                        }

                        if (ring_index >= rings.Count)
                        {
                            // End
                            currentState = State.Stop;
                            started = false;
                        }
                        else
                        {
                            currentState = State.ShowTarget;
                        }
                        switching = true;
                    }
                    break;
                case State.ShowTarget:

                    if (switching)
                    {
                        // Show stuff

                        // Check standard or deviant
                        float o = Random.Range(0.0f, 1.0f);
                        if (o >= odd)
                        {
                            isTarget = false;
                        }
                        else
                        {
                            isTarget = true;
                        }

                        // Check which cube to show
                        cube_index = Random.Range(0, rings[ring_index].Count);

                        // Show corresponding things
                        // Cube
                        GameObject c = rings[ring_index][cube_index];
                        c.SetActive(true);

                        // Sign
                        if (isTarget)
                        {
                            targetSign.SetActive(true);
                            // Move Sign
                            Vector3 pos = c.transform.position;
                            pos.z += (-0.06f);
                            targetSign.transform.position = pos;
                        }
                        else
                        {
                            deviantSign.SetActive(true);
                            // Move Sign
                            Vector3 pos = c.transform.position;
                            pos.z += (-0.06f);
                            deviantSign.transform.position = pos;
                        }

                        // Send event message
                        string[] tempSample;
                        if (isTarget)
                        {
                            tempSample = new string[] { "Ring " + ring_index + "; Trial " + trial + "; Standard cube: " + c.transform.position.ToString("G4")};
                        }
                        else
                        {
                            tempSample = new string[] { "Ring " + ring_index + "; Trial " + trial + "; Deviant cube: " + c.transform.position.ToString("G4") };
                        }
                        Debug.Log(tempSample[0]);
                        markerStream.push_sample(tempSample);

                        nextSwitchTime = Time.time + duration;
                        switching = false;
                    }

                    if (Time.time > nextSwitchTime)
                    {
                        // Hide everything
                        // Cube
                        GameObject c = rings[ring_index][cube_index];
                        c.SetActive(false);

                        if (isTarget)
                        {
                            targetSign.SetActive(false);
                        }
                        else
                        {
                            deviantSign.SetActive(false);
                        }

                        // Increment Trial
                        trial += 1;

                        // Send event message
                        string[] tempSample;
                        tempSample = new string[] { "End Trial" };
                        Debug.Log(tempSample[0]);
                        markerStream.push_sample(tempSample);

                        currentState = State.Center;
                        switching = true;
                    }
                    break;
            }

        }
    }
}
