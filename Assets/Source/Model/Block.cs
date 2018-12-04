using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	private TheWorld TheWorld;
	public bool picked = false;
	private bool interactable = true;

	private Vector3 TopAnchor;
	private Vector3 BottomAnchor;

	private float mD;
	private float AnchorRadius = 0.5f;

	// Use this for initialization
	void Start () {
		TheWorld = (TheWorld)GameObject.FindObjectOfType(typeof(TheWorld));
	}
	
	// Update is called once per frame
	void Update () {
		TopAnchor = new Vector3(transform.localPosition.x, transform.localPosition.y + (transform.localScale.y / 2), transform.localPosition.z);
		BottomAnchor = new Vector3(transform.localPosition.x, transform.localPosition.y - (transform.localScale.y / 2), transform.localPosition.z);

		// Check for hook collision
		if (interactable) {
			mD = Vector3.Dot(TopAnchor, transform.up);
			Vector3 p = TheWorld.HookPosition - transform.up * (Vector3.Dot(TheWorld.HookPosition, transform.up) - mD);
			Vector3 d = p - TopAnchor;
			if (d.magnitude < AnchorRadius && (TheWorld.HookPosition.y - TopAnchor.y) <= 0f && (TheWorld.HookPosition.y - TopAnchor.y) > -0.25f) {
				picked = true;
				TheWorld.HasBlock = true;
				GetComponent<Renderer>().material.color = new Color(0f, 0f, 0f, 0.5f);
			}
		}

		// Move block with hook
		if (picked) {
			transform.position = new Vector3(TheWorld.HookPosition.x, TheWorld.HookPosition.y - (transform.localScale.y / 2), TheWorld.HookPosition.z);
			// Check for block stack
			for (int i = 0; i < transform.parent.childCount; i++) {
				Block block = transform.parent.GetChild(i).gameObject.GetComponent<Block>();
				float BlockMD = Vector3.Dot(block.TopAnchor, block.transform.up);
				Vector3 BlockP = BottomAnchor - block.transform.up * (Vector3.Dot(BottomAnchor, block.transform.up) - BlockMD);
				Vector3 BlockD = BlockP - block.TopAnchor;
				if (BlockD.magnitude < AnchorRadius && (BottomAnchor.y - block.TopAnchor.y) <= 0f && (BottomAnchor.y - block.TopAnchor.y) > -0.25f) {
					picked = false;
					TheWorld.HasBlock = false;
					interactable = false;
					transform.position = new Vector3(block.transform.localPosition.x, block.transform.localPosition.y + transform.localScale.y, block.transform.localPosition.z);
					GetComponent<Renderer>().material.color = Color.yellow;
					block.interactable = false;
					block.GetComponent<Renderer>().material.color = Color.yellow;
				}
			}
		}
	}
}
