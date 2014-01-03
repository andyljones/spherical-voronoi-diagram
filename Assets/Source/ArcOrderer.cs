﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ArcOrderer
{
    public static bool AreInOrder(IArc a, IArc b, IArc c)
    {
        var aLeft = a.DirectionOfLeftIntersection;
        var bLeft = b.DirectionOfLeftIntersection;
        var cLeft = c.DirectionOfLeftIntersection;

        //If a has zero width, make sure it can't return true unless b is zero width too
        // Distinguish a within b from a before b?
        return MathUtils.AreInCyclicOrder(aLeft, bLeft, cLeft);
    }

}