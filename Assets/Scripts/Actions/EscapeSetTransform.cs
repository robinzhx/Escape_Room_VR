using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSetTransform : EscapeAction {
    public Vector3 position;
    public bool setPosition;
    public Vector3 rotation;
    public bool setRotation;
    public Vector3 scale;
    public bool setScale;

    public float animationTime = 1;
    private bool triggered = false;
    private float timer = 0;

    public override void DoAction()
    {
        triggered = true;
    }

    private void Update() {
        if (triggered && timer < animationTime + 0.1f)
        {
            if (setPosition)
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * animationTime);
            if (setRotation)
                transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, rotation, Time.deltaTime * animationTime);
            if (setScale)
                transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime * animationTime);
            timer += Time.deltaTime;
        }
    }
}
