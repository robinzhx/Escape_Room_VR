using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSetScriptActive : EscapeAction
{
    public MonoBehaviour script;
    public bool active = false;

    public override void DoAction()
    {
        script.enabled = active;
    }
}
