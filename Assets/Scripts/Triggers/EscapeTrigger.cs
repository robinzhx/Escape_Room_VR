using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeTrigger : MonoBehaviour {
    private List<EscapeAction> actions;
    public bool triggered = false;

    private void Awake()
    {
        actions = new List<EscapeAction>();
    }

    public void AddAction(EscapeAction action)
    {
        actions.Add(action);
    }

    protected void Trigger()
    {
        if (triggered)
            return;

        foreach(var action in actions)
        {
            action.OnTriggerFire(gameObject);
        }

        triggered = true;
    }
}
