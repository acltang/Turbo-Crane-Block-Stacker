using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuReturn : MonoBehaviour {

    public Button returnButton;
    // Use this for initialization
    void Start()
    {
        returnButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
