using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeFadeAction : EscapeAction
{
    public bool fade = true;
    public float fadeTime = 1;
    public string colorName;

    private bool triggered = false;
    private MeshRenderer meshRenderer;
    private Material material;
    private float targetAlpha = 0;
    private float timer = 0;

    protected override void Start()
    {
        base.Start();
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
    }

    public override void DoAction()
    {
        triggered = true;
        if (!fade)
            targetAlpha = 1;
    }

    private void Update()
    {
        if (meshRenderer && material && triggered && timer < fadeTime + 1)
        {
            var color = material.GetColor(colorName);
            float alpha = color.a;
            alpha = Mathf.Lerp(alpha, targetAlpha, fadeTime * Time.deltaTime);
            color.a = alpha;
            material.SetColor(colorName, color);
            timer += Time.deltaTime;
        }
    }
}
