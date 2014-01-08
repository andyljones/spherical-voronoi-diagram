using Generator;
using MathNet.Numerics;

namespace SphericalVoronoiTests
{
    public static class Utilities
    {
        public static Vector3 VectorAt(double colatitude, double azimuth)
        {
            return new SphericalCoords(Trig.DegreeToRadian(colatitude), Trig.DegreeToRadian(azimuth)).CartesianCoordinates();
        }

        public static Sweepline SweeplineAt(double colatitude)
        {
            return new Sweepline {Colatitude = Trig.DegreeToRadian(colatitude)};
        }
    }
}
