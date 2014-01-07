using System;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;

namespace Generator
{
    public static class ArcCalculator
    {
        public static Vector3 PointAt(this Arc arc, Vector3 vector, Sweepline sweepline)
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


        public static Vector3 LeftIntersection(this Arc arc, Sweepline sweepline)
        {
            var focus1 = arc.Site.Position.SphericalCoordinates();
            var focus2 = arc.LeftNeighbour.Position.SphericalCoordinates();

            var xi = sweepline.Colatitude;
            var theta1 = focus1.Colatitude;
            var phi1 = focus1.Azimuth;
            var theta2 = focus2.Colatitude;
            var phi2 = focus2.Azimuth;

            var a = (Trig.Cosine(xi) - Trig.Cosine(theta2))*Trig.Sine(theta1)*Trig.Cosine(phi1) -
                    (Trig.Cosine(xi) - Trig.Cosine(theta1))*Trig.Sine(theta2)*Trig.Cosine(phi2);

            var b = (Trig.Cosine(xi) - Trig.Cosine(theta2))*Trig.Sine(theta1)*Trig.Sine(phi1) -
                    (Trig.Cosine(xi) - Trig.Cosine(theta1))*Trig.Sine(theta2)*Trig.Sine(phi2);
            var c = (Trig.Cosine(theta1) - Trig.Cosine(theta2))*Trig.Sine(xi);

            var R = Fn.Hypot(a, b);
            var gamma = Trig.InverseTangentFromRational(a, b);

            var phi = Constants.Pi - Trig.InverseSine(c / R) - gamma;

            return new SphericalCoords(Trig.DegreeToRadian(90), phi).CartesianCoordinates();
        }
    }
}
