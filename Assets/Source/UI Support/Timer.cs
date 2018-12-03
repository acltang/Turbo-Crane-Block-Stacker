﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public Text timer;
    private float starttime;
    private void Start()
    {
        starttime = Time.time;
    }


    void Update () {
        float t = Time.time - starttime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        timer.text = minutes + ":" + seconds;
    }
}
