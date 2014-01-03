using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ArcOrderer
{
    public static bool AreInOrder(Arc a, Arc b, Arc c)
    {
        var aLeft = EquatorialVectorOfLeftIntersection(a);
        var bLeft = EquatorialVectorOfLeftIntersection(b);
        var cLeft = EquatorialVectorOfLeftIntersection(c);

        var orderingOnLeftIntersections = MathUtils.AreInCyclicOrder(aLeft, bLeft, cLeft);

        return orderingOnLeftIntersections;
    }

    private static Vector3 EquatorialVectorOfLeftIntersection(Arc a)
    {
        return EllipseCalculator.EquatorialVectorOfIntersection(a.LeftNeighbour, a.SiteEvent, a.Sweepline);
    }

    private static Vector3 EquatorialVectorOfRightIntersection(Arc a)
    {
        return EllipseCalculator.EquatorialVectorOfIntersection(a.SiteEvent, a.RightNeighbour, a.Sweepline);
    }

}