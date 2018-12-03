using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TheWorld : MonoBehaviour {

    public SceneNode RootNode;
    public Camera NodeCam;
    public LineSegment LineOfSight;
    private float kSightLength = 7f;
    private float kNodeCamPos = 8.5f;
    public GameObject UnpassableTerrain;
    public GameObject UnpassableTerrain2;
    private float SizeOfBase = 2.5f;

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
    }

    public SceneNode GetRootNode() { return RootNode; }

     public bool ClearofTerrain(Vector3 position)
    {
        if(position.z - SizeOfBase <= UnpassableTerrain.transform.position.z +
            (UnpassableTerrain.transform.localScale.z/2) && position.x - SizeOfBase <=
            UnpassableTerrain.transform.position.x + (UnpassableTerrain.transform.localScale.x / 2))
        {
            if (position.z + SizeOfBase >= UnpassableTerrain.transform.position.z -
            (UnpassableTerrain.transform.localScale.z / 2) && position.x + SizeOfBase >=
            UnpassableTerrain.transform.position.x - (UnpassableTerrain.transform.localScale.x / 2))
            {
                return false;
            }
        }

        if (position.z - SizeOfBase <= UnpassableTerrain2.transform.position.z +
            (UnpassableTerrain2.transform.localScale.z / 2) && position.x - SizeOfBase <=
            UnpassableTerrain2.transform.position.x + (UnpassableTerrain2.transform.localScale.x / 2))
        {
            if (position.z + SizeOfBase >= UnpassableTerrain2.transform.position.z -
            (UnpassableTerrain2.transform.localScale.z / 2) && position.x + SizeOfBase >=
            UnpassableTerrain2.transform.position.x - (UnpassableTerrain2.transform.localScale.x / 2))
            {
                return false;
            }
        }
        return true;
    }
    
}
