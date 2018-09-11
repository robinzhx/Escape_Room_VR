using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeTransformTrigger : EscapeTrigger
{
    public Transform target;
    public Vector3 currentLocation;
    public Vector3 expectedPosition;
    public bool inversePositioncheck;
    public bool checkPosition;
    public bool istriggered;
    public Vector3 currentRotation;
    public Vector3 expectedAngle;
    public bool checkRotation;
    public float epsilon = 5.0f;
    

    private void Start()
    {
        if (!target)
            target = transform;
        istriggered = false;
    }

    void Update () {
        currentRotation = target.eulerAngles;
        currentLocation = target.position;
        if (checkRotation && !istriggered && !EscapeUtil.EulerAngleEpsilonEquals(target.eulerAngles, expectedAngle, epsilon))
        {
            return;
        }
        if (checkPosition && !istriggered && !EscapeUtil.EpsilonEquals(target.position, expectedPosition, epsilon))
        {
            return;
        }

        if (checkRotation && istriggered && !EscapeUtil.EulerAngleEpsilonEquals(target.eulerAngles, expectedAngle, epsilon))
        {
            Clear();
            istriggered = false;
            return;
        }

        if (checkPosition && istriggered && !EscapeUtil.EpsilonEquals(target.position, expectedPosition, epsilon))
        {
            Clear();
            istriggered = false;
            return;
        }


        if (inversePositioncheck && !istriggered && EscapeUtil.EpsilonEquals(target.position, expectedPosition, epsilon))
        {
            return;
        }

        if (inversePositioncheck && istriggered && !EscapeUtil.EpsilonEquals(target.position, expectedPosition, epsilon))
        {
            Clear();
            istriggered = false;
            return;
        }

        Trigger();
        istriggered = true;
	}
}