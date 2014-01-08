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
            var focus1 = arc.Site.Position.SphericalCoordinates();
            var focus2 = arc.LeftNeighbour.Position.SphericalCoordinates();

            var xi = sweepline.Colatitude;
            var theta1 = focus1.Colatitude;
            var phi1 = focus1.Azimuth;
            var theta2 = focus2.Colatitude;
            var phi2 = focus2.Azimuth;

            if (Vector.AlmostEqual(focus1, focus2))
            {
                return new SphericalCoords(Trig.DegreeToRadian(90), focus1.Azimuth).CartesianCoordinates();
            }

            var a = (Trig.Cosine(xi) - Trig.Cosine(theta2))*Trig.Sine(theta1)*Trig.Cosine(phi1) -
                    (Trig.Cosine(xi) - Trig.Cosine(theta1))*Trig.Sine(theta2)*Trig.Cosine(phi2);

            var b = (Trig.Cosine(xi) - Trig.Cosine(theta2))*Trig.Sine(theta1)*Trig.Sine(phi1) -
                    (Trig.Cosine(xi) - Trig.Cosine(theta1))*Trig.Sine(theta2)*Trig.Sine(phi2);
            var c = (Trig.Cosine(theta1) - Trig.Cosine(theta2))*Trig.Sine(xi);

            var R = Fn.Hypot(a, b);
            var gamma = Trig.InverseTangentFromRational(a, b);

            var inverseSineOfCOverR = StablizedInverseSine(c/R);

            var leftIntersection = SelectLeftIntersection(inverseSineOfCOverR, gamma, focus1.Azimuth);

            return leftIntersection;
        }

        private static double StablizedInverseSine(double ratio)
        {
            var needsClamping = Math.Abs(ratio) < 1;

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
