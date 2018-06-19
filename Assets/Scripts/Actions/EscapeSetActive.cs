using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSetActive : EscapeAction {
    public bool active = false;

    private bool triggered = false;
    private MeshRenderer meshRenderer;
    private Material material;
    private float _targetAlpha = 0;
    private float timer = 0;

    protected override void Start()
    {
        base.Start();

        if (active)
            gameObject.SetActive(false);
    }

    public override void DoAction()
    {
        triggered = true;
        gameObject.SetActive(active);
    }
}
