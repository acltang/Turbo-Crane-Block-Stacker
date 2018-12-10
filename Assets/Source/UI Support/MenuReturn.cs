using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuReturn : MonoBehaviour {
    public Text Score;
    public Text Timer;
    public Button returnButton;
    // Use this for initialization
    void Start()
    {
        returnButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        string[] times = Timer.text.Split(':');
        string[] points = Score.text.Split(' ');
        double[] score = new double[3];
        score[0] = double.Parse(points[1]);
        score[1] = double.Parse(times[0]);
        score[2] = double.Parse(times[1]);
        HighScores.scores.Add(score);
        SceneManager.LoadScene("MenuScene");
    }
}
