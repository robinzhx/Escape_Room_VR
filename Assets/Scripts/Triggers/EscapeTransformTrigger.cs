using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeTransformTrigger : EscapeTrigger
{
    public Vector3 currentRotation;
    public Vector3 expectedPosition;
    public bool checkPosition;
    public Vector3 expectedAngle;
    public bool checkRotation;
    public float epsilon = 5.0f;
	
	void Update () {
        currentRotation = transform.eulerAngles;
        if (checkRotation && !EscapeUtil.EulerAngleEpsilonEquals(transform.eulerAngles, expectedAngle, epsilon))
            return;

        if (checkPosition && !EscapeUtil.EpsilonEquals(transform.position, expectedPosition, epsilon))
            return;

        Trigger();
	}
}
