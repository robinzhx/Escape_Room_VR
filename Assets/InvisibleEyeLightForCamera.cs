using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleEyeLightForCamera : MonoBehaviour
{

    public Light limelight;

    void OnPreCull()
    {
        if (limelight != null)
            limelight.enabled = false;
    }

    void OnPreRender()
    {
        if (limelight != null)
            limelight.enabled = false;
    }
    void OnPostRender()
    {
        if (limelight != null)
            limelight.enabled = true;
    }
}
