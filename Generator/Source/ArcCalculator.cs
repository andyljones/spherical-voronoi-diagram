using System;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;

namespace Generator
{
    public static class ArcCalculator
    {
        public static Vector3 PointAt(this IArc arc, Vector3 vector, Sweepline sweepline)
        {
            var p = arc.Site.Position;
            var n = AngleUtilities.EquatorialDirection(vector);
            var Z = sweepline.Z;

            if (Number.AlmostEqual(p.Z, Z) && Vector.AlmostEqual(AngleUtilities.EquatorialDirection(p), n))
            {
                return p;
            }
            if (Number.AlmostEqual(p.Z, Z))
            {
                return new Vector3(0, 0, 1);
            }

            var tanOfColatitude = (Z - p.Z)/(p.ScalarMultiply(n) - Math.Sqrt(1 - Z*Z));

            var x = n.X * Math.Abs(tanOfColatitude/Math.Sqrt(1 + tanOfColatitude*tanOfColatitude));
            var y = n.Y * Math.Abs(tanOfColatitude/Math.Sqrt(1 + tanOfColatitude * tanOfColatitude));
            var z = Math.Sign(tanOfColatitude) / Math.Sqrt(1 + tanOfColatitude * tanOfColatitude);

            return new Vector3(x, y, z);
        }

        public static Vector3 LeftIntersection(this IArc arc, Sweepline sweepline)
        {
            var p = arc.LeftNeighbour.Position;
            var q = arc.Site.Position;
            var Z = sweepline.Z;

            if (Vector.AlmostEqual(p, q))
            {
                return AngleUtilities.EquatorialDirection(p);
            }
            if (Number.AlmostEqual(Z, p.Z) && Number.AlmostEqual(Z, q.Z))
            {
                return AngleUtilities.EquatorialMidpoint(p, q);
            }

            var A = p.X*(Z - q.Z) - q.X*(Z - p.Z);
            var B = p.Y*(Z - q.Z) - q.Y*(Z - p.Z);
            var C = (p.Z - q.Z)*Math.Sqrt(1 - Z*Z);

            var A2PlusB2MinusC2 = Math.Max(A*A + B*B - C*C, 0);
            var x =  (A*C + B*Math.Sqrt(A2PlusB2MinusC2)) / (A*A + B*B);
            var y =  (B*C - A*Math.Sqrt(A2PlusB2MinusC2)) / (A*A + B*B);

            return new Vector3(x, y, 0);
        }
    }
}
