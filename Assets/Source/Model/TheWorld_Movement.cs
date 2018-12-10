using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class TheWorld : MonoBehaviour {

	public SceneNode RootNode, Hook, Arm;
	private float SizeOfBase = 2.5f;
    public Vector3 HookPosition;
	 private float mMouseX = 0f;
    //private float mMouseY = 0f;
    private bool movement = true;
	
    public bool HasBlock = false;

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
            float angle = 100 * Time.deltaTime;
            if (HookisRotatable(angle))
            {
                RootNode.transform.Rotate(-Vector3.up * 100 * Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            float angle = -100 * Time.deltaTime;

            if (HookisRotatable(angle))
            {
                RootNode.transform.Rotate(Vector3.up * 100 * Time.deltaTime);
            }
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
            generateFlyingBlocks = false;

            foreach (GameObject terrain in ImpassableTerrains) {
                terrain.SetActive(false);
            }
        }
    }

    private void RotateArm() {
        if (Input.GetMouseButtonDown(0))
        {
            mMouseX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            float dx = mMouseX - Input.mousePosition.x;
            mMouseX = Input.mousePosition.x;
            
            float angle = dx * Time.deltaTime * 30;
            
            if (HookisRotatable(angle))
            {
               Arm.transform.Rotate(-Vector3.up * dx * Time.deltaTime * 30, Space.World);
            }
        }
    }

    private bool HookisRotatable(float angle)
    {
        Vector3 Hookafterrotation = HookPosition;
        Hookafterrotation = Hookafterrotation - RootNode.transform.position;

        Hookafterrotation.x = (Hookafterrotation.x * (float)Math.Cos(angle * Mathf.Deg2Rad)) - (Hookafterrotation.z * (float)Math.Sin(angle * Mathf.Deg2Rad));
        Hookafterrotation.z = (Hookafterrotation.x * (float)Math.Sin(angle * Mathf.Deg2Rad)) + (Hookafterrotation.z * (float)Math.Cos(angle * Mathf.Deg2Rad));

        Hookafterrotation = Hookafterrotation + RootNode.transform.position;

        if (HasBlock)
        {
            if (ClearofCubes(Hookafterrotation))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (ClearofCubes(Hookafterrotation))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void MoveHook() {
        Vector2 d = Input.mouseScrollDelta;
        Vector3 p = Hook.transform.position;
        Vector3 test = HookPosition;
        test.y = test.y + (d.y / 8);
        p.y = p.y + (d.y / 8);
        float UpperBound = 6f;
       // float LowerBound = -3.2f;
        float LowerBound = -5.2f;
        if (HasBlock) {
            LowerBound += 2f;
        }
        if (p.y <= UpperBound && p.y >= LowerBound )
        {
            if (d.y > 0 || ClearofCubes(test))
            {
                kSightLength = kSightLength - (d.y / 8);
                Hook.transform.position = p;
            }
        }
    }
}
