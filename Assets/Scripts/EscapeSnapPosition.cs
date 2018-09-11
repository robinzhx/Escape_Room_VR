using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSnapPosition : MonoBehaviour {

    public List<Vector3> SnapPositions;
    public bool EnablePositionSnapping = true;
    public List<Vector3> SnapRotations;
    public bool EnableRotationSnapping = true;
    public float Epsilon = 0.05f;
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < SnapPositions.Count; i++)
        {
            var pos = SnapPositions[i];
            var rot = i<SnapRotations.Count ?SnapRotations[i]:SnapRotations[SnapRotations.Count-1];
            if (Vector3.Distance(transform.position, pos) < Epsilon)
            {
                if (EnablePositionSnapping)
                    transform.position = pos;
                if (EnableRotationSnapping)
                    transform.localEulerAngles = rot;
            }
        }
    }
}
