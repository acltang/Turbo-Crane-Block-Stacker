using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour {

    public Button StartButton;
    // Use this for initialization
    void Start () {
        StartButton.onClick.AddListener(TaskOnClick);
    }
	
    void TaskOnClick()
    {
        SceneManager.LoadScene("Stacker");
    }
}
