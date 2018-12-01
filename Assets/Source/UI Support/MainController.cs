using System; // for assert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for GUI elements: Button, Toggle

public partial class MainController : MonoBehaviour {

    // reference to all UI elements in the Canvas
    public TheWorld mModel = null;
    public SceneNodeControl SNControl = null;
    public Transform LookAt = null;


    // Use this for initialization
    void Start() {
        Debug.Assert(mModel != null);
        Debug.Assert(SNControl != null);
        Debug.Assert(LookAt != null);


        LookAt.localPosition = new Vector3(0f, 0f, 0f);
    }

}
