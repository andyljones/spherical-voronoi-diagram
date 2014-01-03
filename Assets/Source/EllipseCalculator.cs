using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class EllipseCalculator
{
    public static Vector3 EquatorialVectorOfIntersection(SiteEvent leftEvent, SiteEvent rightEvent, Sweepline sweepline)
    {
        var a = leftEvent.Position;
        var b = rightEvent.Position;
        var Z = sweepline.Z;

        if (Mathf.Abs(a.z - Z) < MathUtils.ComparisonTolerance && Mathf.Abs(b.z - Z) < MathUtils.ComparisonTolerance)
        {
            return MathUtils.EquatorialMidpointBetween(a, b);
        }
        if (Mathf.Abs(a.z - Z) < MathUtils.ComparisonTolerance)
        {
            return MathUtils.EquatorialVectorOf(a);
        }
        if (Mathf.Abs(b.z - Z) < MathUtils.ComparisonTolerance)
        {
            return MathUtils.EquatorialVectorOf(b);
        }
        if ((a - b).magnitude < MathUtils.ComparisonTolerance)
        {
            return MathUtils.EquatorialVectorOf((a+b)/2);
        }

        var A = a.x * (Z - b.z) - b.x * (Z - a.z);
        var B = -(a.y * (Z - b.z) - b.y * (Z - a.z));
        var C = (a.z - b.z) * Mathf.Sqrt(1 - Z * Z); //TODO: Does this need a Sign(z), since it represents Sin(zeta)?

        var x = (A * C + B * Mathf.Sqrt(A * A + B * B - C * C)) / (A * A + B * B);
        var y = -(B * C - A * Mathf.Sqrt(A * A + B * B - C * C)) / (A * A + B * B);

        return new Vector3(x, y, 0);
    }

    public static Vector3 PointOnEllipseAboveVector(Vector3 v, Vector3 focus, Sweepline sweepline)
    {
        var p = focus;
        var Z = sweepline.Z;
        var t = MathUtils.EquatorialVectorOf(v);
        
        var tanOfColat = (Z - p.z) / (p.x*t.x + p.y*t.y - Mathf.Sqrt(1 - Z * Z));
        var z = Mathf.Sign(tanOfColat) / Mathf.Sqrt(1 + tanOfColat*tanOfColat);

        var pointOnEllipse = Mathf.Sqrt(1 - z*z)*t + new Vector3(0, 0, z);

        return pointOnEllipse;
    }

    public static Vector3 IntersectionBetween(SiteEvent leftEvent, SiteEvent rightEvent, Sweepline sweepline)
    {
        var a = leftEvent.Position;
        var b = rightEvent.Position;
        var Z = sweepline.Z;

        if (Mathf.Abs(a.z - Z) < MathUtils.ComparisonTolerance && Mathf.Abs(b.z - Z) < MathUtils.ComparisonTolerance)
        {
            return new Vector3(0, 0, 1);
        }

        var v = EquatorialVectorOfIntersection(leftEvent, rightEvent, sweepline);

        var focus = Mathf.Abs(a.z - Z) < MathUtils.ComparisonTolerance ? b : a;
        var w = PointOnEllipseAboveVector(v, focus, sweepline);
        return w;
    }
}