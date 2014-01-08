using MathNet.Numerics;

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

        public static Vector3 DirectionOf(Vector3 v)
        {
            var length = Fn.Hypot(v.X, v.Y);

            return new Vector3(v.X/length, v.Y/length, 0);
        }
    }
}
