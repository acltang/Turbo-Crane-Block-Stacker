using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public partial class TheWorld : MonoBehaviour {

    public Camera MainCamera;
    public Camera NodeCam;
    public LineSegment LineOfSight;
    private float kSightLength = 7f;
    private float kNodeCamPos = 8.5f;

    public GameObject holdingBlock;
    private IEnumerator coroutine; // for falling blocks

    public Plane Barrier;

    public int Score;
    public Text ScoreText;
    public Timer Timer;

    public bool generateFlyingBlocks = true;

    // Use this for initialization
    void Start () {
        Debug.Assert(RootNode != null);
        Debug.Assert(NodeCam != null);
        Debug.Assert(LineOfSight != null);
        LineOfSight.SetWidth(0.05f);

        GenerateFloorBlocks();
        UpdateTerrains();
        UpdateBlocks();

        // start block falling sequence
        coroutine = DropBlocks();
        StartCoroutine(coroutine);

        Score = 0;
        SetScoreText();
    }

    void Update()
    {
        Vector3 pos, dir;
        Matrix4x4 m = Matrix4x4.identity;
        RootNode.CompositeXform(ref m, out pos, out dir);

        // Get second to last Node
        RootNode.CompositeXformModified(ref m, out pos, out dir);

        Vector3 p1 = pos + kNodeCamPos * dir;
        Vector3 p2 = p1 + kSightLength * Vector3.down;
        LineOfSight.SetEndPoints(p1, p2);

        // Now update NodeCam
        NodeCam.transform.localPosition = pos + kNodeCamPos * dir;
        NodeCam.transform.LookAt(p2, Vector3.up);

        // Update hook position
        HookPosition = new Vector3(p2.x, p2.y - 0.2f, p2.z);

        // Player Controls
        if (movement) {
            MoveCrane();
            RotateArm();
            MoveHook();

            // check for falling off map
            if (RootNode.transform.localPosition.y >= 0f && CheckDropCrane()) {
                Timer.count = false;
                movement = false;
                DropAllBlocks();
            }
        }
        else {
            DropCrane();
        }

        // Spawn FlyingBlocks
        if (generateFlyingBlocks) {
            SpawnFlyingBlocks();
        }

        SetScoreText();
    }

    public SceneNode GetRootNode() { return RootNode; }

    private void GenerateFloorBlocks() {
        Vector3 position = new Vector3(0f, -1f, 0f); // initial block position

        // instantiate blocks in a spiral pattern
        // spiral solution from https://stackoverflow.com/questions/398299/looping-in-a-spiral
        int X = 29; // blocks per row
        int Y = 20; // blocks per column
        int x,y,dx,dy;
        x = y = dx =0;
        dy = -1;
        int t = Math.Max(X,Y);
        int maxI = t*t;
        for(int i =0; i < maxI; i++){
            if ((-X/2 <= x) && (x <= X/2) && (-Y/2 <= y) && (y <= Y/2)){
                position.x = x * 2;
                position.z = y * 2;
                GameObject block = Instantiate(Resources.Load("Block", typeof(GameObject))) as GameObject;
                block.transform.parent = GameObject.Find("Blocks").transform;
                block.transform.localPosition = position;
                if (i % 2 == 0) {
                    block.GetComponent<Renderer>().material = Resources.Load("Materials/DarkGreen", typeof(Material)) as Material;
                }
            }
            if( (x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1-y))){
                t = dx;
                dx = -dy;
                dy = t;
            }
            x += dx;
            y += dy;
        }
    }

    private IEnumerator DropBlocks() {
        foreach (GameObject block in Blocks) {
            if (block) {
                Block floorBlock = block.gameObject.GetComponent<Block>();
                if (floorBlock.name.Equals("Block(Clone)") && !floorBlock.picked && !floorBlock.stacked) {
                    floorBlock.Drop(0.4f);
                    Barrier.BlockPositions.Add(floorBlock.transform.localPosition);
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
    }

    private void DropAllBlocks() {
        StopCoroutine(coroutine);
        foreach (GameObject block in Blocks) {
            if (block) {
                Block floorBlock = block.gameObject.GetComponent<Block>();
                if (!floorBlock.picked && !floorBlock.stacked) {
                    floorBlock.Drop(2f);
                    ParticleSystem explosion = Instantiate(Resources.Load("Explosion", typeof(ParticleSystem))) as ParticleSystem;
                    ParticleSystem.MainModule settings = explosion.main;
                    Material darkRed = Resources.Load("Materials/DarkRed") as Material;
                    settings.startColor = new ParticleSystem.MinMaxGradient(darkRed.color);
                    settings.startLifetime = 2;
                    settings.startDelay = 3.27f;
                    Vector3 blockPosition = floorBlock.transform.localPosition; 
                    explosion.transform.localPosition = new Vector3(blockPosition.x, blockPosition.y - 14f, blockPosition.z);
                    explosion.Play();
                }
            }
        }
    }

    private void SpawnFlyingBlocks() {
        mSinceLastGenerated += Time.deltaTime;
        if (mSinceLastGenerated >= mGenerateInterval)
        {
            GameObject g = Instantiate(Resources.Load("FlyingBlock")) as GameObject;
            FlyingBlock b = g.GetComponent<FlyingBlock>();
            b.Initialize();

            mSinceLastGenerated = 0f;
        }
    }

    private void SetScoreText() {
        ScoreText.text = "Score: " + Score.ToString();
    }
}
