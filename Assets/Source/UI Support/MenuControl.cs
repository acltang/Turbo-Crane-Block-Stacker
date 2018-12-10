using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuControl : MonoBehaviour {

    public Button StartButton;
    public Text HighscoreText;
    // Use this for initialization
    void Start () {
        StartButton.onClick.AddListener(TaskOnClick);

        //sort the scores
        var orderResult = HighScores.scores.OrderBy(x => x[0]).ToList();
        orderResult.Reverse();
        //add the scores up to 10
        string listofScores = "";
        int i = 1;
        while (i <= HighScores.scores.Count && i < 10)
        {
            listofScores = listofScores + "\n" + i + ". Score: " + orderResult[i - 1][0] + "   Time: " + orderResult[i - 1][1] + ":" + orderResult[i - 1][2];
            i++;
        }
        HighscoreText.text = HighscoreText.text + listofScores;

    }
    

    void TaskOnClick()
    {
        SceneManager.LoadScene("Stacker");
    }
}
