using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TheWorld : MonoBehaviour {

	public List<GameObject> ImpassableTerrains;
    public List<GameObject> Blocks;

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
        if (HasBlock)
        {
            foreach (GameObject block in Blocks)
            {
                if (block && !ClearofCubeWithBlock(position, block))
                {
                    return false;
                }
            }
            return true;
        }
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

    public bool ClearofCubeWithBlock(Vector3 position, GameObject cube)
    {
        if (cube != holdingBlock)
        {
            if (position.z - 1 <= cube.transform.position.z +
            (cube.transform.localScale.z / 2) && position.x - 1 <=
            cube.transform.position.x + (cube.transform.localScale.x / 2))
            {
                if (position.z + 1 >= cube.transform.position.z -
                (cube.transform.localScale.z / 2) && position.x + 1 >=
                cube.transform.position.x - (cube.transform.localScale.x / 2))
                {
                    if (position.y - 2 <= cube.transform.position.y + ((cube.transform.localScale.y / 2) - .25) &&
                    position.y - 2 >= cube.transform.position.y - ((cube.transform.localScale.y / 2) + .25))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}
