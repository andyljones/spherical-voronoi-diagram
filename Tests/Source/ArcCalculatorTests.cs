using System;
using Generator;
using MathNet.Numerics;
using SphericalVoronoiTests.DataAttributes;
using Xunit;
using Xunit.Extensions;

namespace SphericalVoronoiTests
{
    public class ArcCalculatorTests
    {
        private const double Tolerance = 0.000000001f;

        [Theory]
        [VectorsAboveSweepline]
        public void PointOnArcAboveVector_ShouldReturnAPointEquidistantFromTheSweeplineAndTheFocus
            (Arc arc, Vector3 vector, Sweepline sweepline)
        {
            // Fixture setup
            
            // Exercise system
            var point = arc.PointOnArcAboveVector(vector, sweepline);

            // Verify outcome
            var distanceToFocus = Trig.InverseCosine(point.ScalarMultiply(arc.Site.Position));
            var distanceToSweepline = Math.Abs(sweepline.Colatitude - Trig.InverseCosine(point.Z));

            var failureString = String.Format("Distance to focus was {0}\nDistance to sweepline was {1}", distanceToFocus, distanceToSweepline);
            Assert.True(Number.AlmostEqual(distanceToFocus, distanceToSweepline, Tolerance), failureString);

            // Teardown
        }

        [Theory]
        [VectorsAboveSweepline]
        public void PointOnArcAboveVector_ShouldReturnAPointWithTheSameAzimuthAsTheArgument
            (Arc arc, Vector3 vector, Sweepline sweepline)
        {
            // Fixture setup

            // Exercise system
            var point = arc.PointOnArcAboveVector(vector, sweepline);

            // Verify outcome
            var argumentAzimuth = Trig.InverseTangentFromRational(vector.Y, vector.X);
            var resultAzimuth = Trig.InverseTangentFromRational(point.Y, point.X);

            var failureString = String.Format("Argument azimuth was {0} \nResult azimuth was {1}", argumentAzimuth, resultAzimuth);
            Assert.True(Number.AlmostEqual(argumentAzimuth, resultAzimuth, Tolerance), failureString);

            // Teardown
        }
    }
}
