using System.Collections;
using System.Collections.Generic;
using LSL;
using NewtonVR;
using UnityEngine;

public class GrabStreamer : MonoBehaviour {

    private liblsl.StreamOutlet markerStream;

	// Use this for initialization
	void Start ()
    {
        liblsl.StreamInfo inf =
            new liblsl.StreamInfo("GrabMarker", "Markers", 1, 0, liblsl.channel_format_t.cf_string, "controller");
        markerStream = new liblsl.StreamOutlet(inf);
	}

    public void SendGrabMarkerLeft(NVRInteractable interactable)
    {
        string name = "";
        if (interactable.GetComponent<UniqueID>())
            name = interactable.GetComponent<UniqueID>().guid;
        else 
            name = interactable.name;

        string markerString = "Left hand grabs:" + name;
        Debug.Log(markerString);
        string[] tempSample = { markerString };
        markerStream.push_sample(tempSample);
    }

    public void SendGrabMarkerRight(NVRInteractable interactable)
    {
        string name = "";
        if (interactable.GetComponent<UniqueID>())
            name = interactable.GetComponent<UniqueID>().guid;
        else 
            name = interactable.name;

        string markerString = "Right hand grabs:" + name;
        Debug.Log(markerString);
        string[] tempSample = { markerString };
        markerStream.push_sample(tempSample);
    }

}
