using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TheWorld : MonoBehaviour {

    // to keep track of last time a cube is generated
    float mSinceLastGenerated = 10f;
    float mGenerateInterval = 0.1f;  // in second

    public bool ProcessBarrier(FlyingBlock b, Transform shadowXform)
    {
        bool castShadow = false;
        if (Barrier.PtInfrontOf(b.GetPosition()))
        {
            float d = Barrier.DistantToPoint(b.GetPosition());
            Vector3 onBarrier = b.GetPosition() - Barrier.GetNormal() * d;

            if (Barrier.InActiveZone(onBarrier))
            {
                castShadow = true;
                // first, process shadow
                Quaternion q = Quaternion.FromToRotation(Vector3.up, Barrier.GetNormal());
                shadowXform.localRotation = q;
                shadowXform.localPosition = onBarrier + Barrier.GetNormal() * 0.1f; // slight offet
                shadowXform.localScale = new Vector3(1f, 0.1f, 1f);
            }
        }
        return castShadow;
    }
}
