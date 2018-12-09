using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public partial class TheWorld_Turbo : MonoBehaviour {

    public SceneNode RootNode, Hook, Arm;
    public Camera MainCamera;
    public Camera NodeCam;
    public LineSegment LineOfSight;
    private float kSightLength = 7f;
    private float kNodeCamPos = 8.5f;

    public List<GameObject> ImpassableTerrains;
    public List<GameObject> Blocks;

    public IEnumerator coroutine;

    private float SizeOfBase = 2.5f;
    public Vector3 HookPosition;
    public bool HasBlock = false;
    private float mMouseX = 0f;
    //private float mMouseY = 0f;
    private bool movement = true;

    public int Score;
    public Text ScoreText;
    public Timer Timer;

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

        SetScoreText();
    }

    public SceneNode GetRootNode() { return RootNode; }

    private void MoveCrane() {
        if (Input.GetKey(KeyCode.W) && ClearofTerrain(RootNode.transform.position + -RootNode.transform.forward * Time.deltaTime * 10)
            && ClearofCubes(HookPosition + -RootNode.transform.forward * Time.deltaTime * 10))
        {
            RootNode.transform.position += -RootNode.transform.forward * Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.S)&& ClearofTerrain(RootNode.transform.position + RootNode.transform.forward * Time.deltaTime * 10)
            && ClearofCubes(HookPosition + RootNode.transform.forward * Time.deltaTime * 10))
        {
            RootNode.transform.position += RootNode.transform.forward * Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {}
        else if (Input.GetKey(KeyCode.A))
        {
            RootNode.transform.Rotate(-Vector3.up * 100 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RootNode.transform.Rotate(Vector3.up * 100 * Time.deltaTime);
        }
    }

    private bool CheckDropCrane() {
        foreach (GameObject block in Blocks) {
            if (block) {
                float dist = Vector3.Distance(block.transform.localPosition, RootNode.transform.localPosition);
                float floorDist = (float)Math.Sqrt((dist * dist) - 4f); // 4 is 2 squared (2 is distance from base.y to block.y)
                if (floorDist < 3f) {
                    return false;
                }
            }
        }
        return true;
    }

    private void DropCrane() {
        if (RootNode.transform.localPosition.y < -15f) {
            movement = true;
        }
        else {
            RootNode.transform.localPosition += -transform.up * Time.deltaTime * 10;
            MainCamera.transform.localPosition += -transform.up * Time.deltaTime * 15;
            Transform lookAt = MainCamera.GetComponent<CameraManipulation>().LookAt;
            lookAt.localPosition += -transform.up * Time.deltaTime * 15;
            NodeCam.gameObject.SetActive(false);
            HasBlock = true;
        }
    }

    private void RotateArm() {
        if (Input.GetMouseButtonDown(0))
        {
            mMouseX = Input.mousePosition.x;
            //mMouseY = Input.mousePosition.y;
        }
        else if (Input.GetMouseButton(0))
        {
            float dx = mMouseX - Input.mousePosition.x;
            //float dy = mMouseY - Input.mousePosition.y;
            mMouseX = Input.mousePosition.x;
            //mMouseY = Input.mousePosition.y;
            Arm.transform.Rotate(-Vector3.up * dx * Time.deltaTime * 30, Space.World);
            /* bool moveit= true;
             Arm.transform.Rotate(-Vector3.up * dx * Time.deltaTime * 35, Space.World);
             if (!ClearofCube(HookPosition, Block))
             {
                 moveit = false;
             }
             Arm.transform.Rotate(Vector3.up * dx * Time.deltaTime * 35, Space.World);
             if (moveit)
             {
                 Arm.transform.Rotate(Vector3.up * dx * Time.deltaTime * 30, Space.World);
             }*/

        }
    }

    private void MoveHook() {
        Vector2 d = Input.mouseScrollDelta;
        Vector3 p = Hook.transform.position;
        Vector3 test = HookPosition;
        test.y = test.y + (d.y / 8);
        p.y = p.y + (d.y / 8);
        float UpperBound = 6f;
        //float LowerBound = -3.2f;
        float LowerBound = -5.2f;
        if (HasBlock) {
            LowerBound += 2f;
        }
        if (p.y <= UpperBound && p.y >= LowerBound && ClearofCubes(test))
        {
            kSightLength = kSightLength - (d.y / 8);
            Hook.transform.position = p;
        }
    }

    private void UpdateTerrains() {
        GameObject WorldTerrains = GameObject.Find("ImpassableTerrains");
        foreach (Transform terrain in WorldTerrains.transform) {
            ImpassableTerrains.Add(terrain.gameObject);
        }
    }

    private void UpdateBlocks() {
        GameObject WorldBlocks = GameObject.Find("Blocks");
        foreach (Transform block in WorldBlocks.transform) {
            Blocks.Add(block.gameObject);
        }
    }

     public bool ClearofTerrain(Vector3 position)
    {
        foreach (GameObject terrain in ImpassableTerrains) {
            if (terrain && !ClearofSpecificTerrain(position, terrain)) {
                return false;
            }
        }
        foreach (GameObject block in Blocks) {
            if (block && !ClearofSpecificTerrain(position, block)) {
                return false;
            }
        }
        return true;
    }

     public bool ClearofCubes(Vector3 position)
    {
        foreach (GameObject block in Blocks) {
            if (block && !ClearofCube(position, block)) {
                return false;
            }
        }
        return true;
    }

    public bool ClearofSpecificTerrain(Vector3 position, GameObject terrain)
    {
        if (position.z - SizeOfBase <= terrain.transform.position.z +
            (terrain.transform.localScale.z / 2) && position.x - SizeOfBase <=
            terrain.transform.position.x + (terrain.transform.localScale.x / 2))
        {
            if (position.z + SizeOfBase >= terrain.transform.position.z -
            (terrain.transform.localScale.z / 2) && position.x + SizeOfBase >=
            terrain.transform.position.x - (terrain.transform.localScale.x / 2))
            {
                if (position.y <= terrain.transform.position.y + ((terrain.transform.localScale.y / 2)-.25) &&
                position.y >= terrain.transform.position.y - ((terrain.transform.localScale.y / 2)+.25))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool ClearofCube(Vector3 position, GameObject cube)
    {
        if (position.z  <= cube.transform.position.z +
            (cube.transform.localScale.z / 2) && position.x <=
            cube.transform.position.x + (cube.transform.localScale.x / 2))
        {
            if (position.z  >= cube.transform.position.z -
            (cube.transform.localScale.z / 2) && position.x >=
            cube.transform.position.x - (cube.transform.localScale.x / 2))
            {
                if (position.y <= cube.transform.position.y + ((cube.transform.localScale.y / 2)-.25) &&
                position.y >= cube.transform.position.y - ((cube.transform.localScale.y / 2)+.25))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void GenerateFloorBlocks() {
        Vector3 position = new Vector3(0f, -1f, 0f); // initial block position

        // instantiate blocks in a spiral pattern
        // spiral solution from https://stackoverflow.com/questions/398299/looping-in-a-spiral
        int X = 30; // blocks per row
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

    private void SetScoreText() {
        ScoreText.text = "Score: " + Score.ToString();
    }
}
