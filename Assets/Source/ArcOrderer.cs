using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ArcOrderer
{
    public static bool AreInOrder(Arc a, Arc b, Arc c)
    {
        //TODO: Optimize this.
        var aLeft = EquatorialVectorOfLeftIntersection(a);
        var bLeft = EquatorialVectorOfLeftIntersection(b);
        var cLeft = EquatorialVectorOfLeftIntersection(c);

        var orderingOnLeftIntersections = MathUtils.AreInCyclicOrder(aLeft, bLeft, cLeft);

        if (orderingOnLeftIntersections != 0)
        {
            return orderingOnLeftIntersections > 0;
        }
        else
        {
            var aMid = EquatorialMidpoint(a);
            var bMid = EquatorialMidpoint(b);
            var cMid = EquatorialMidpoint(c);

            var orderingOnMidpoints = MathUtils.AreInCyclicOrder(aMid, bMid, cMid);

            return orderingOnMidpoints >= 0;
        }
    }

    private static Vector3 EquatorialVectorOfLeftIntersection(Arc a)
    {
        return EllipseCalculator.EquatorialVectorOfIntersection(a.LeftNeighbour, a.SiteEvent, a.Sweepline);
    }

    private static Vector3 EquatorialVectorOfRightIntersection(Arc a)
    {
        return EllipseCalculator.EquatorialVectorOfIntersection(a.SiteEvent, a.RightNeighbour, a.Sweepline);
    }

    private static Vector3 EquatorialMidpoint(Arc a)
    {
        var left = EquatorialVectorOfLeftIntersection(a);
        var right = EquatorialVectorOfRightIntersection(a);

        var mid = MathUtils.EquatorialMidpointBetween(left, right);

        return mid;
    }
}