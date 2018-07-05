using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EscapeAction : MonoBehaviour
{
    public List<EscapeTrigger> triggers;
    private int triggeredCount = 0;

    protected virtual void Start()
    {
        foreach (var trigger in triggers)
            trigger.AddAction(this);
    }

    public abstract void DoAction();

    public void OnTriggerFire(GameObject trigger)
    {
        if (++triggeredCount >= triggers.Count)
            DoAction();
    }

    public void OnTriggerClear(GameObject trigger)
    {
        triggeredCount--;
    }
}
