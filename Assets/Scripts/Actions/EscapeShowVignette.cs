using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class EscapeShowVignette : EscapeAction
{
    public float TunnelOverTime = 0.2f;
    public float VignettePower = 17;
    public float VignetteEaseInTime = 0.125f;
    public float VignetteEaseOutTime = 0.1f;

    public override void DoAction()
    {
        StartCoroutine(ShowVignette());
    }

    private IEnumerator ShowVignette()
    {
        float easeInStartTime = Time.time;
        float easeInEndTime = easeInStartTime + VignetteEaseInTime;

        while (Time.time < easeInEndTime)
        {
            yield return null;
            NVRVignette.instance.SetAmount(Mathf.Lerp(0, VignettePower, (Time.time - easeInStartTime) / VignetteEaseInTime));
        }
        NVRVignette.instance.SetAmount(VignettePower);

        float moveTimeStart = Time.time;
        float moveTimeEnd = moveTimeStart + TunnelOverTime;
        while (Time.time < moveTimeEnd)
        {
            yield return null;
        }

        float easeOutStartTime = Time.time;
        float easeOutEndTime = easeOutStartTime + VignetteEaseOutTime;

        while (Time.time < easeOutEndTime)
        {
            yield return null;
            NVRVignette.instance.SetAmount(Mathf.Lerp(VignettePower, 0, (Time.time - easeOutStartTime) / VignetteEaseOutTime));
        }

        yield return null;
        NVRVignette.instance.SetAmount(0);
    }
}
