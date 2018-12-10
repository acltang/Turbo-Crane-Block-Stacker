using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	private TheWorld TheWorld;
	public bool picked = false;
	public bool stacked = false;
	public bool prestacked = false;
	public bool droppable = true;
	private bool falling = false;

	private Vector3 TopAnchor;
	private Vector3 BottomAnchor;

	private float mD;
	private float AnchorRadius = 0.5f;

	public List<Block> BlockStack;
	public Block BottomBlock;
	public int BlockIndex = 0;

	// Use this for initialization
	void Start () {
		TheWorld = (TheWorld)GameObject.FindObjectOfType(typeof(TheWorld));
		BlockStack.Add(GetComponent<Block>());
		BottomBlock = GetComponent<Block>();

		if (name.Equals("Block(Clone)")) {
			FindPreStackedBlocks();
		}
	}
	
	// Update is called once per frame
	void Update () {
		TopAnchor = new Vector3(transform.localPosition.x, transform.localPosition.y + (transform.localScale.y / 2), transform.localPosition.z);
		BottomAnchor = new Vector3(transform.localPosition.x, transform.localPosition.y - (transform.localScale.y / 2), transform.localPosition.z);

		// Check for hook collision
		Pickup();

		// Move block with hook (and check for Stack)
		Travel();

		// Floor block is falling
		if (falling) {
			float ground = -14f + ((BlockIndex - 1) * 2);
			if (stacked && transform.localPosition.y < ground) {
				falling = false;
				transform.localPosition = new Vector3(transform.localPosition.x, ground, transform.localPosition.z);
			}
			else {
				if (!stacked && transform.localPosition.y < -14f) { // block is too low, destroy
					Explode();
				}
				else {
					Fall();
				}
			}
		}

		// indicator for max points on stack
		if (BlockIndex == 4) {
			GetComponent<Renderer>().material.color = new Color(255f, 216f, 0f);
		}
	}

	private void Pickup() {
		if (!stacked && !prestacked && !TheWorld.HasBlock) {
			mD = Vector3.Dot(TopAnchor, transform.up);
			Vector3 p = TheWorld.HookPosition - transform.up * (Vector3.Dot(TheWorld.HookPosition, transform.up) - mD);
			Vector3 d = p - TopAnchor;
			if (d.magnitude < AnchorRadius && (TheWorld.HookPosition.y - TopAnchor.y) <= 0f && (TheWorld.HookPosition.y - TopAnchor.y) > -0.25f) {
				picked = true;
				TheWorld.HasBlock = true;
                TheWorld.holdingBlock = gameObject;
				GetComponent<Renderer>().material.color = new Color(0f, 0f, 0f, 0.7f);
			}
		}
	}

	private void Travel() {
		if (picked) {
			transform.position = new Vector3(TheWorld.HookPosition.x, TheWorld.HookPosition.y - (transform.localScale.y / 2), TheWorld.HookPosition.z);
			// Check for block stack
			Stack();
		}
	}

	private void Stack() {
		for (int i = 0; i < transform.parent.childCount; i++) {
			Block block = transform.parent.GetChild(i).gameObject.GetComponent<Block>();
			float BlockMD = Vector3.Dot(block.TopAnchor, block.transform.up);
			Vector3 BlockP = BottomAnchor - block.transform.up * (Vector3.Dot(BottomAnchor, block.transform.up) - BlockMD);
			Vector3 BlockD = BlockP - block.TopAnchor;
			if (BlockD.magnitude < AnchorRadius && (BottomAnchor.y - block.TopAnchor.y) <= 0f && (BottomAnchor.y - block.TopAnchor.y) > -0.25f) {
				picked = false;
				TheWorld.HasBlock = false;
				droppable = false;
				transform.position = new Vector3(block.transform.localPosition.x, block.transform.localPosition.y + transform.localScale.y, block.transform.localPosition.z);
				SetStacked();

				BottomBlock = block;
				BlockIndex = block.BlockIndex + 1;
				for (int j = 1; j < BlockIndex; j++) {
					BottomBlock = BottomBlock.BottomBlock;
				}
				BottomBlock.BlockStack.Add(GetComponent<Block>());
				for (int j = 1; j < BottomBlock.BlockStack.Count; j++) {
					BottomBlock.BlockStack[j].SetStacked();
				}
				TheWorld.Score += BlockIndex;
			}
		}
	}

	private void SetStacked() {
		stacked = true;
		GetComponent<Renderer>().material.color = Color.black;
	}

	private void SetPreStacked() {
		prestacked = true;
		GetComponent<Renderer>().material.color = Color.gray;
	}


	// Handle block floor falling____________________________________
	public void Drop(float seconds) {
		foreach (Block block in BlockStack) {
			if (block && !block.stacked) {
				block.GetComponent<Renderer>().material.color = Color.red;
			}
		}
		Invoke("SetFall", seconds);
	}

	private void SetFall() {
		foreach (Block block in BlockStack) {
			if (block) {
				block.falling = true;
				if (!block.stacked) {
					block.GetComponent<Renderer>().material = Resources.Load("Materials/DarkRed") as Material;
				}
			}
		}
	}

	private void Fall() {
		transform.localPosition += -transform.up * Time.deltaTime * 10;
	}
	//_______________________________________________________________

	public void FindPreStackedBlocks() {
		foreach (GameObject block in TheWorld.Blocks) {
			if (block && !block.name.Equals("Block(Clone)")) {
				Block stackedBlock = block.GetComponent<Block>();
				if (stackedBlock.transform.localPosition.x == transform.localPosition.x &&
				stackedBlock.transform.localPosition.z == transform.localPosition.z) {
					stackedBlock.BottomBlock = GetComponent<Block>();
					stackedBlock.BlockIndex = (int)((stackedBlock.transform.localPosition.y + 1) / 2);
					stackedBlock.SetPreStacked();
					BlockStack.Add(stackedBlock);
				}
			}
		}
	}

	private void Explode() {
		ParticleSystem explosion = GameObject.Find("Explosion").GetComponent<ParticleSystem>();
		explosion.transform.localPosition = transform.localPosition;
		ParticleSystem.MainModule settings = explosion.main;
		Material darkRed = Resources.Load("Materials/DarkRed") as Material;
		settings.startColor = new ParticleSystem.MinMaxGradient(darkRed.color);
		explosion.Play();
		Destroy(gameObject);
	}
}
