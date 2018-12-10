using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScores : MonoBehaviour
{
    public static List<double[]> scores = new List<double[]>();

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
