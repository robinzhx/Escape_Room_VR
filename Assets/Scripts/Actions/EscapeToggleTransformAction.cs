using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeToggleTransformAction : EscapeAction
{
    public Vector3 position1;
    public Vector3 rotation1;
    public Vector3 position2;
    public Vector3 rotation2;
    private bool use1 = true;
    
    private bool triggered = false;

    public override void DoAction()
    {
        triggered = true;
    }

    private void Update()
    {
        if (triggered)
        {
            Vector3 position = use1 ? position1 : position2;
            Vector3 rotation = use1 ? rotation1 : rotation2;
            transform.position = position;
            transform.localEulerAngles = rotation;
            triggered = false;
            use1 = !use1;
        }
    }
}
