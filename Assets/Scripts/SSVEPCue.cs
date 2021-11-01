﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSVEPCue : MonoBehaviour
{
    
    public float frequency = 20.0f;
    public Renderer selfRenderer = null;
    public bool isEnable = false;
    public float accumTime = 0.0f;
    public float step = 0.0f;


    public Color onColor = Color.white;
    public Color offColor = Color.black;
    public Color oldColor;

    private Material _cloneMat;
    private bool _currState = false;

    public Flicky flickyController;

    public void Toggle(bool flag)
    {
        /*
        isEnable = flag;
        if (isEnable)
        {
            if(_cloneMat)
                _cloneMat.color = offColor;
        }
        */
        isEnable = flag;
        if (isEnable)
        {
            flickyController = GetComponent<Flicky>();
            //flickyController.Initialize();
            flickyController.SetMainColor(Color.black);
            flickyController.SetBlinkColor(Color.white);
            flickyController.SetFrequency(10);
        }
        else
        {
            flickyController.PauseFlickering();
        }

    }
    
    // Start is called before the first frame update
    void Start()
    {
        //if (!selfRenderer)
        //{
        //    selfRenderer = this.gameObject.GetComponent<Renderer>();
       // }
        //_cloneMat = Instantiate(selfRenderer.material);
        //selfRenderer.material = _cloneMat;
        //oldColor = _cloneMat.color;
        step = 1.0f / frequency;


    }

    // Update is called once per frame
    void Update()
    {
        if(false)
        if (isEnable)
        {
            //if (_currState)
            //{
                var currDT = Time.deltaTime;
                accumTime += currDT;
                if (accumTime > step / 2.0f)
                {
                    // turn on
                    if (!_currState)
                    {
                        _cloneMat.color = onColor;
                        _currState = true;
                    }
                    else
                    {
                        _cloneMat.color = offColor;
                        _currState = false;
                    }

                    accumTime = 0.0f;
                }
            //}
            //else
            //{
                // turn off
            //    _cloneMat.color = offColor;
            //    _currState = false;
            //}
        }
        else
        {
            if (_currState)
            {
                _cloneMat.color = oldColor;
            }
            accumTime = 0.0f;
        }

    }
}
