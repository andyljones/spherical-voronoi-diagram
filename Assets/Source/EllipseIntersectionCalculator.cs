using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class EllipseIntersectionCalculator
{

    public static Vector3 IntersectionBetween(SiteEvent site1, SiteEvent site2, Sweepline sweepline)
    {
        if (Equals(site1.Position, site2.Position))
        {
            return site1.Position;
        }

        var a = site1.Position;
        var b = site2.Position;
        var Z = sweepline.Z;

        float tx;
        float ty;
        float tz;

        if (Mathf.Abs(a.z - Z) < MathUtils.ComparisonTolerance)
        {
            tx = a.x / Mathf.Sqrt(a.x*a.x + a.y*a.y);
            ty = a.y / Mathf.Sqrt(a.x*a.x + a.y*a.y);
            tz = (Z-b.z) / (b.x*tx + b.y*ty - Mathf.Sqrt(1 - Z * Z));
        }
        else if (Mathf.Abs(b.z - Z) < MathUtils.ComparisonTolerance)
        {
            tx = b.x/Mathf.Sqrt(b.x*b.x + b.y*b.y);
            ty = b.y/Mathf.Sqrt(b.x*b.x + b.y*b.y);
            tz = (Z - a.z)/(a.x*tx + a.y*ty - Mathf.Sqrt(1 - Z*Z));
        }
        else
        {
            var A = a.x*(Z - b.z) - b.x*(Z - a.z);
            var B = -(a.y*(Z - b.z) - b.y*(Z - a.z));
            var C = (a.z - b.z)*Mathf.Sqrt(1 - Z*Z); //TODO: Does this need a Sign(z), since it represents Sin(zeta)?

            tx = (A*C + B*Mathf.Sqrt(A*A + B*B - C*C))/(A*A + B*B);
            ty = -(B*C - A*Mathf.Sqrt(A*A + B*B - C*C))/(A*A + B*B);

            tz = (Z - a.z)/(a.x*tx + a.y*ty - Mathf.Sqrt(1 - Z*Z));
        }

        var x = 1 / Mathf.Sqrt(1 + 1 / (tz * tz)) * tx;
        var y = 1 / Mathf.Sqrt(1 + 1 / (tz * tz)) * ty;
        var z = Mathf.Sign(tz) * 1 / Mathf.Sqrt(1 + tz * tz);

        return new Vector3(x, y, z);
    }
}