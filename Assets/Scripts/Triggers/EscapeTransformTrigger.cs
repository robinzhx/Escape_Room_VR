﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeTransformTrigger : EscapeTrigger
{
    public Transform target;
    private Vector3 currentRotation;
    private Vector3 currentLocation;
    public Vector3 expectedPosition;
    public bool checkPosition;
    public Vector3 expectedAngle;
    public bool checkRotation;
    public float epsilon = 5.0f;

    private void Start()
    {
        if (!target)
            target = transform;
    }

    void Update () {
        currentRotation = target.eulerAngles;
        currentLocation = target.position;
        if (checkRotation && !EscapeUtil.EulerAngleEpsilonEquals(target.eulerAngles, expectedAngle, epsilon))
            return;

        if (checkPosition && !EscapeUtil.EpsilonEquals(target.position, expectedPosition, epsilon))
            return;

        Trigger();
	}
}
