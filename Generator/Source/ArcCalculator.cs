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
            var focus = arc.Site.Position;
            var focusColatitude = focus.SphericalCoordinates().Colatitude;
            var focusAzimuth = focus.SphericalCoordinates().Azimuth;
            var vectorAzimuth = vector.SphericalCoordinates().Azimuth;
            var sweeplineColatitude = sweepline.Colatitude;
            
            if (Number.AlmostEqual(focusColatitude, sweeplineColatitude) && 
                Number.AlmostEqual(focusAzimuth, vectorAzimuth))
            {
                return focus;
            }

            var tanOfResultColatitude = (Trig.Cosine(sweeplineColatitude) - Trig.Cosine(focusColatitude)) /
                                        (Trig.Sine(focusColatitude)*Trig.Cosine(vectorAzimuth - focusAzimuth) - Trig.Sine(sweeplineColatitude));

            var resultColatitude = Trig.InverseTangent(tanOfResultColatitude);

            var result = AngleUtilities.CartesianCoordinates(resultColatitude, vectorAzimuth);

            return result;
        }


        public static Vector3 LeftIntersection(this IArc arc, Sweepline sweepline)
        {
            var p = arc.LeftNeighbour.Position;
            var q = arc.Site.Position;
            var Z = Trig.Cosine(sweepline.Colatitude);

            if (Vector.AlmostEqual(p, q))
            {
                return AngleUtilities.DirectionOf(p);
            }

            var A =   p.X*(Z - q.Z) - q.X*(Z - p.Z);
            var B =  (p.Y*(Z - q.Z) - q.Y*(Z - p.Z));
            var C =  (p.Z - q.Z)*Math.Sqrt(1 - Z*Z);

            var A2PlusB2MinusC2 = Math.Max(A*A + B*B - C*C, 0);
            var x =  (A*C + B*Math.Sqrt(A2PlusB2MinusC2)) / (A*A + B*B);
            var y =  (B*C - A*Math.Sqrt(A2PlusB2MinusC2)) / (A*A + B*B);

            Debug.WriteLine(x*x + y*y);
            Debug.WriteLine(x);
            Debug.WriteLine(y);

            return new Vector3(x, y, 0);
        }

        private static double StablizedInverseSine(double ratio)
        {
            var needsClamping = Math.Abs(ratio) >= 1;

            if (needsClamping)
            {
                return Math.Sign(ratio)*Constants.Pi_2;
            }
            else
            {
                return Trig.InverseSine(ratio);
            }
        }

        private static Vector3 SelectLeftIntersection(double inverseSineOfCOverR, double gamma, double focusAzimuth)
        {
            var phiA = inverseSineOfCOverR - gamma;
            var intersectionA = new SphericalCoords(Trig.DegreeToRadian(90), phiA).CartesianCoordinates();
            var phiB = Constants.Pi - inverseSineOfCOverR - gamma;
            var intersectionB = new SphericalCoords(Trig.DegreeToRadian(90), phiB).CartesianCoordinates();

            var focusDirection = new SphericalCoords(Trig.DegreeToRadian(90), focusAzimuth).CartesianCoordinates();

            var intersectionAIsOnTheLeft = (intersectionB - focusDirection).CrossMultiply(intersectionA - focusDirection)[2] >= 0;

            return intersectionAIsOnTheLeft ? intersectionA : intersectionB;
        }
    }
}
