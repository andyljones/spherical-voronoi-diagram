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
            (Arc arc)
        {
            // Fixture setup
            var longitude = (float) (2*Mathf.PI*new System.Random().NextDouble());

            // Exercise system
            var result = BeachlineDrawer.PointOnEllipse(arc, longitude);

            // Verify outcome
            var distanceFromSite = Mathf.Acos(Vector3.Dot(result, arc.SiteEvent.Position));
            var distanceFromSweepline = Mathf.Abs(Mathf.Acos(result.z) - Mathf.Acos(arc.Sweepline.Z));

            Assert.Equal(distanceFromSite, distanceFromSweepline, Tolerance);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void PointOnEllipse_ShouldGenerateAPointAtTheRequestedLongitude
            (Arc arc)
        {
            // Fixture setup
            var longitude = (float)(2 * Mathf.PI * new System.Random().NextDouble());

            // Exercise system
            var result = BeachlineDrawer.PointOnEllipse(arc, longitude);

            // Verify outcome
            var actualLongitude = MathMod(Mathf.Atan2(-result.y, result.x), 2*Mathf.PI);

            Assert.Equal(longitude, actualLongitude, Tolerance);

            // Teardown
        }

        private static float MathMod(float x, float m)
        {
            return ((x%m) + m)%m;
        }
    }
}
