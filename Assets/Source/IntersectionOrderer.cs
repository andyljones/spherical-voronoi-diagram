using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CyclicalSkipList;

public static class IntersectionOrderer
{
    public static Func<Intersection, Intersection, Intersection, bool> InOrder; 

    static IntersectionOrderer()
    {
        InOrder = new CompareToCyclicOrdererAdapter<Intersection>(LinearCompare).InOrder;
    }

    private static int LinearCompare(Intersection x, Intersection y)
    {
        return x.CompareTo(y);
    }
}