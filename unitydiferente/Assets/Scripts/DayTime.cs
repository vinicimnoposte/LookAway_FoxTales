﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTime : MonoBehaviour
{
    public float daySeconds;
    float radDay;
    public float daySpeed = 1;
    Light sunlight;
    float intensity;
    public Gradient ambient;
    public delegate void SunEvent();
    public SunEvent DawnCall;
    public SunEvent DuskCall;

    public static DayTime instance;
    bool day = false;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        sunlight = GetComponent<Light>();
        DawnCall += Afternon;
        DuskCall += Morning;
    }
    //86400
    // Update is called once per frame
    void Update()
    {
        daySeconds += Time.deltaTime* daySpeed;
        radDay = daySeconds / (86400/2);
        transform.localRotation = Quaternion.Euler(radDay * (180 / Mathf.PI), 0, 0);

        intensity = Mathf.Clamp01(Vector3.Dot(transform.forward, Vector3.down)+0.3f);
        sunlight.intensity = intensity;

        RenderSettings.ambientLight = ambient.Evaluate(intensity);

        RenderSettings.fogDensity = 0.001f * intensity;
        if (intensity > 0.4f&& !day)
        {
            DuskCall();
            day = true;
        }
        if (intensity < 0.4f&& day)
        {
            DawnCall();
            day = false;
        }

    }
    void Morning()
    {
        print("Good day");
    }
    void Afternon()
    {
        print("Good night");
    }
}
