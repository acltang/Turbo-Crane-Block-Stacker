using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plane : MonoBehaviour {
    // Vector3 mN;     // normal, normalized, this is actually -transform.forward
    float mD;       // mN dot P = mD;
    //public LineSegment mNormal; // This is the normal vector we will display

    const float kNormalLength = 5f; // length of the normal vector

	public List<Vector3> BlockPositions;

	// Update is called once per frame
	void Update () {
        UpdatePlaneEquation();
        Vector3 center = transform.localPosition;
        //mNormal.SetEndPoints(center, p1);
    }

    public void UpdatePlaneEquation()
    {
        mD = Vector3.Dot(transform.localPosition, GetNormal());
    }

    public Vector3 GetNormal() { return -transform.forward; }

    public float DistantToPoint(Vector3 p)
    {
        return  Vector3.Dot(p, GetNormal()) - mD;
    }

    public bool InActiveZone(Vector3 p)
    {
		Vector3 barrierD = p - transform.localPosition; 
		foreach (Vector3 position in BlockPositions) {
			Vector3 d = p - position;
			if (d.magnitude < 2f || !AboveBarrier(barrierD)) {
				return false;
			}
		}
        return true;
    }

	private bool AboveBarrier(Vector3 d) {
		return d.x < (transform.localScale.x / 2f) + 1 && d.z < (transform.localScale.y / 2f) &&
			   d.x > -(transform.localScale.x / 2f) + 1 && d.z > -(transform.localScale.y / 2f);
	}

    public bool PtInfrontOf(Vector3 p)
    {
        Vector3 va = p - transform.localPosition;
        return (Vector3.Dot(va, GetNormal()) > 0f);
    }
}