using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSetTransform : EscapeAction {
    public Transform target;
    public Vector3 position;
    public bool setPosition;
    public Vector3 rotation;
    public bool setRotation;
    public Vector3 scale;
    public bool setScale;

    public float animationTime = 1;
    private bool triggered = false;
    private float timer = 0;

    private void Awake()
    {
        if (!target)
            target = transform;
    }

    public override void DoAction()
    {
        triggered = true;
        if(animationTime <= 0)
        {
            if (setPosition)
                target.position = position;
            if (setRotation)
                target.localEulerAngles = rotation;
            if (setScale)
                target.localScale = scale;
            triggered = false;
        }
    }

    private void Update() {
        if (triggered && animationTime > 0)
        {
            if (timer > animationTime)
            {
                timer = 0;
                triggered = false;
            }
            else
            {
                if (setPosition)
                    target.position = Vector3.Lerp(target.position, position, Time.deltaTime * animationTime);
                if (setRotation)
                {
                    var quat = Quaternion.Euler(rotation);
                    target.rotation = Quaternion.Lerp(target.rotation, quat, Time.deltaTime * animationTime);
                }
                if (setScale)
                    target.localScale = Vector3.Lerp(target.localScale, scale, Time.deltaTime * animationTime);
                timer += Time.deltaTime;
            }
        }
    }
}
