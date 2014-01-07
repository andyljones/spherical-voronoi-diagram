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
            var x = Trig.Sine(coords.Theta)*Trig.Cosine(coords.Phi);
            var y = Trig.Sine(coords.Theta)*Trig.Sine(coords.Phi);
            var z = Trig.Cosine(coords.Theta);

            return new Vector3(x, y, z);
        }
    }
}
