using System; // for assert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for GUI elements: Button, Toggle

public class ArmRotation : MonoBehaviour {
    public Camera MainCamera = null;
    private float mMouseX = 0f;
    private float mMouseY = 0f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            mMouseX = Input.mousePosition.x;
            mMouseY = Input.mousePosition.y;
        }else if (Input.GetMouseButton(0))
        {
            float dx = mMouseX - Input.mousePosition.x;
            float dy = mMouseY - Input.mousePosition.y;
            mMouseX = Input.mousePosition.x;
            mMouseY = Input.mousePosition.y;
            transform.Rotate(-Vector3.up * dx * Time.deltaTime * 30, Space.World);

        }
    }
}
