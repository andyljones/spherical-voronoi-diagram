using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Generator
{
    public static class AngleUtilities
    {
        public static SphericalCoords SphericalCoordinates(this Vector3 vector)
        {
            var colatitude = Trig.InverseCosine(vector.Z);
            var azimuth = Trig.InverseTangentFromRational(vector.Y, vector.X);

            return new SphericalCoords(colatitude, azimuth);
        }

        public static Vector3 CartesianCoordinates(this SphericalCoords coords)
        {
            return CartesianCoordinates(coords.Colatitude, coords.Azimuth);
        }

        public static Vector3 CartesianCoordinates(double colatitude, double azimuth)
        {
            var x = Trig.Sine(colatitude) * Trig.Cosine(azimuth);
            var y = Trig.Sine(colatitude) * Trig.Sine(azimuth);
            var z = Trig.Cosine(colatitude);

            return new Vector3(x, y, z);
        }

        public static Vector3 EquatorialDirection(Vector3 v)
        {
            var length = Fn.Hypot(v.X, v.Y);

            return new Vector3(v.X/length, v.Y/length, 0);
        }

        public static Vector3 EquatorialMidpoint(Vector3 u, Vector3 v)
        {
            var northPole = new Vector3(0, 0, 1);
            var equatorialU = EquatorialDirection(u);
            var equatorialV = EquatorialDirection(v);

            var midpoint = (equatorialU - northPole).CrossMultiply(equatorialV - northPole);

            return EquatorialDirection(midpoint.ToVector3());

        }

        public static Vector3 ToVector3(this Vector v)
        {
            return new Vector3(v[0], v[1], v[2]);
        }
    }
}
