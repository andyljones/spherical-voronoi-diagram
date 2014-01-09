using System;
using System.Collections.Generic;
using System.Linq;
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

        public static SiteEvent SiteAt(double colatitude, double azimuth)
        {
            return new SiteEvent {Position = VectorAt(colatitude, azimuth)};
        }

        public static Sweepline SweeplineAt(double colatitude)
        {
            return new Sweepline {Colatitude = Trig.DegreeToRadian(colatitude)};
        }


        public static String ToString<T>(IEnumerable<T> enumerable)
        {
            return String.Join(", ", enumerable.Select(item => item.ToString()).ToArray());
        }
    }
}
