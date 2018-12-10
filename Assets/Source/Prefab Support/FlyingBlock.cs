using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBlock : MonoBehaviour {

	float mSpeed = 0f;
    Vector3 mDir = Vector3.right;
    float mMaxLengthAlive = 0f;  // in seconds
    float mTimeAlive = 0f;

    TheWorld mWorld;

    public GameObject mShadow;
    public GameObject mProjectedOnBig;


    // Use this for initialization
    void Start()
    {
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mShadow = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mShadow.GetComponent<Renderer>().material.color = Color.black;
        mShadow.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mShadow.GetComponent<Renderer>().enabled = false; // don't show by default

        mWorld = GameObject.Find("TheWorld").GetComponent<TheWorld>();
    }
	
	// Update is called once per frame
	void Update () {
        mTimeAlive += Time.deltaTime;
        if (mTimeAlive > mMaxLengthAlive)
        {
            Destroy(mShadow);
            Destroy(mProjectedOnBig);
            Destroy(transform.gameObject);
        }

        bool castShadow = mWorld.ProcessBarrier(GetComponent<FlyingBlock>(), mShadow.transform);
        mShadow.GetComponent<Renderer>().enabled = castShadow;

        transform.localPosition += mSpeed * Time.deltaTime * mDir;
	}

    public void Initialize()
    {
        SetStartPosition(new Vector3(Random.Range(0f, 50f), Random.Range(10f, 25f), Random.Range(-20f, 30f)));
        SetDir(new Vector3(Random.Range(-0.1f, -0.7f), Random.Range(-0.1f, -0.2f), 0f).normalized);
        float speed = Random.Range(30f, 60f);
        SetSpeed(speed);
        //SetAliveTime(speed / 25f);
        SetAliveTime(4f);
    }

    public Vector3 GetPosition() { return transform.localPosition; }

    // private setters
    private void SetSpeed(float s) { mSpeed = s; }
    private void SetDir(Vector3 d) { mDir = d; }
    private void SetStartPosition(Vector3 p) { 
        transform.localPosition = p; 
    }
    private void SetAliveTime(float t) { mMaxLengthAlive = t; }
}
