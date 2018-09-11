using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeLimitTransform : MonoBehaviour
{
    public Vector3 MinPosition;
    public Vector3 MaxPosition;

    void FixedUpdate()
    {
        var pos = transform.position;

        pos = new Vector3(
            Mathf.Clamp(pos.x, MinPosition.x, MaxPosition.x)
            , Mathf.Clamp(pos.y, MinPosition.y, MaxPosition.y)
            , Mathf.Clamp(pos.z, MinPosition.z, MaxPosition.z)
            );
    }
}
