using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TheWorld : MonoBehaviour {

    public SceneNode RootNode, Hook;
    public Camera NodeCam;
    public LineSegment LineOfSight;
    private float kSightLength = 7f;
    private float kNodeCamPos = 8.5f;
    public GameObject UnpassableTerrain;
    public GameObject UnpassableTerrain2;
    public GameObject Block;
    public GameObject Wall1, Wall2, Wall3, Wall4;
    private float SizeOfBase = 2.5f;
    public Vector3 HookPosition;
    public bool HasBlock = false;

    	// Use this for initialization
	void Start () {
        Debug.Assert(RootNode != null);
        Debug.Assert(NodeCam != null);
        Debug.Assert(LineOfSight != null);
        LineOfSight.SetWidth(0.05f);
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

        //CRANE MOVEMENT
        if (Input.GetKey(KeyCode.W) && ClearofTerrain(RootNode.transform.position + -RootNode.transform.forward * Time.deltaTime * 10))
        {
            RootNode.transform.position += -RootNode.transform.forward * Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.S)&& ClearofTerrain(RootNode.transform.position + RootNode.transform.forward * Time.deltaTime * 10))
        {
            RootNode.transform.position += RootNode.transform.forward * Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {

        }
        else if (Input.GetKey(KeyCode.A))
        {
            RootNode.transform.Rotate(-Vector3.up * 100 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RootNode.transform.Rotate(Vector3.up * 100 * Time.deltaTime);
        }

        Vector2 d = Input.mouseScrollDelta;
        Vector3 p = Hook.transform.position;
        p.y = p.y + (d.y / 8);
        float UpperBound = 6f;
        float LowerBound = -3.2f;
        if (HasBlock) {
            LowerBound += 2f;
        }
        if (p.y <= UpperBound && p.y >= LowerBound)
        {
            kSightLength = kSightLength - (d.y / 8);
            Hook.transform.position = p;
        }

    }

    public SceneNode GetRootNode() { return RootNode; }

     public bool ClearofTerrain(Vector3 position)
    {
        if(    ClearofSpecificTerrain(position, UnpassableTerrain)
            && ClearofSpecificTerrain(position, UnpassableTerrain2)
            && ClearofSpecificTerrain(position, Wall1)
            && ClearofSpecificTerrain(position, Wall2)
            && ClearofSpecificTerrain(position, Wall3)
            && ClearofSpecificTerrain(position, Wall4))
        {
            return true;
        }
        return false;
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
                return false;
            }
        }
        return true;
    }
}
