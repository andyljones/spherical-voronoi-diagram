using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace Generator
{
    public class SiteEvent
    {
        public Vector Position;

        public SiteEvent(double x, double y, double z)
        {
            Position = new Vector(new[] {x, y, z});
        }
    }
}
