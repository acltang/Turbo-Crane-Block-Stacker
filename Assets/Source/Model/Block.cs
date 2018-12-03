using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	private TheWorld TheWorld;
	public bool picked = false;

	private Vector3 TopAnchor;
	private float mD;
	private float AnchorRadius = 0.5f;

	// Use this for initialization
	void Start () {
		TheWorld = (TheWorld)GameObject.FindObjectOfType(typeof(TheWorld));
	}
	
	// Update is called once per frame
	void Update () {
		// Check for hook collision
		TopAnchor = new Vector3(transform.localPosition.x, transform.localPosition.y + (transform.localScale.y / 2), transform.localPosition.z);
		mD = Vector3.Dot(TopAnchor, transform.up);
		Vector3 p = TheWorld.HookPosition - transform.up * (Vector3.Dot(TheWorld.HookPosition, transform.up) - mD);
		Vector3 d = p - TopAnchor;
		if (d.magnitude < AnchorRadius && (TheWorld.HookPosition.y - TopAnchor.y) <= 0f && (TheWorld.HookPosition.y - TopAnchor.y) > -0.25f) {
			picked = true;
			TheWorld.HasBlock = true;
			GetComponent<Renderer>().material.color = Color.black;
		}

		if (picked) {
			transform.position = new Vector3(TheWorld.HookPosition.x, TheWorld.HookPosition.y - (transform.localScale.y / 2), TheWorld.HookPosition.z);
		}
	}
}
