using SphericalVoronoiDiagramTests.DataAttributes;
using UnityEngine;
using Xunit;
using Xunit.Extensions;
using Debug = System.Diagnostics.Debug;

namespace SphericalVoronoiDiagramTests
{
    public class EllipseDrawerTests
    {
        private const int Tolerance = 3; 

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void PointOnEllipse_ShouldGenerateAPointEquidistantFromTheFocusAndTheSweepline
            (Vector3 focus, Sweepline sweepline)
        {
            // Fixture setup
            var longitude = (float) (2*Mathf.PI*new System.Random().NextDouble());

            // Exercise system
            var result = BeachlineDrawer.PointOnEllipse(focus, sweepline.Height, longitude);

            // Verify outcome
            var distanceFromSite = Mathf.Acos(Vector3.Dot(result, focus));
            var distanceFromSweepline = Mathf.Abs(Mathf.Acos(result.z) - Mathf.Acos(sweepline.Height));

            Assert.Equal(distanceFromSite, distanceFromSweepline, Tolerance);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void PointOnEllipse_ShouldGenerateAPointAtTheRequestedLongitude
            (Vector3 focus, Sweepline sweepline)
        {
            // Fixture setup
            var expectedLongitude = (float)(2 * Mathf.PI * new System.Random().NextDouble());

            // Exercise system
            var result = BeachlineDrawer.PointOnEllipse(focus, sweepline.Height, expectedLongitude);

            // Verify outcome
            var actualLongitude = MathMod(Mathf.Atan2(-result.y, result.x), 2*Mathf.PI);

            Assert.Equal(expectedLongitude, actualLongitude, Tolerance);

            // Teardown
        }

        private static float MathMod(float x, float m)
        {
            return ((x%m) + m)%m;
        }
    }
}
