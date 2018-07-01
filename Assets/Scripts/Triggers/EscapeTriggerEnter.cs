using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeTriggerEnter : EscapeTrigger
{
    public Vector3 maxPosition;
    public Vector3 minPosition;
    public Transform target;
    public bool isInside = false;

    void Update()
    {
        if (!target)
            return;

        var position = target.position;
        bool currIsInside = EscapeUtil.LessThan(minPosition, position) && EscapeUtil.LessThan(position, maxPosition);

        if (!isInside && currIsInside)
            Trigger();

        isInside = currIsInside;
    }
}
