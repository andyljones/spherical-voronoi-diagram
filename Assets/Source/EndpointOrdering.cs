using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class EndpointOrdering
{
    public static float FindIntersection(Site leftSite, Site rightSite, float pz)
    {
        var a = leftSite.Position;
        var b = rightSite.Position;

        var aDeltaZ = a.z - pz;
        var bDeltaZ = b.z - pz;

        var B = aDeltaZ*b.x - bDeltaZ*a.x;
        var A = -(aDeltaZ*b.y - bDeltaZ*a.y);
        var c = (a.z - b.z)*Mathf.Sqrt(1 - pz*pz);

        var R = Mathf.Sqrt(B*B + A*A);
        var psi = Mathf.Atan2(B, A);

        Debug.Log("R:" + R);
        Debug.Log("psi:" + psi*180/Math.PI);

        return Mathf.Asin(c/R) - psi;
    }
}