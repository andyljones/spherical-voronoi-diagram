using SphericalVoronoiDiagramTests.FixtureCustomizations;
using UnityEngine;
using Xunit;
using Xunit.Extensions;
using Debug = System.Diagnostics.Debug; 

namespace SphericalVoronoiDiagramTests
{
    public class IntersectionTests
    {
        private const int Tolerance = 3;

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void Azimuth_WhenRightSiteIsUndefined_ShouldReturnLongitudeOfLeftSite
            (Site leftSite, Sweepline sweepline)
        {
            // Fixture setup
            var expectedResult = leftSite.Azimuth;

            var sut = new Intersection(leftSite, null, sweepline);

            // Exercise system
            var result = sut.Azimuth;

            // Verify outcome
            Assert.Equal(result, expectedResult);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void Azimuth_WhenBothSitesAreDefined_ShouldBeEquidistantFromBothSitesAndTheSweeplineWhenConvertedToAPointOnTheEllipse
            (Intersection sut, Sweepline sweepline)
        {
            // Fixture setup

            // Exercise system
            var result = sut.Azimuth;

            // Verify outcome
            var pointOnEllipse = EllipseDrawer.PointOnEllipse(sut.LeftSite.Position, sweepline.Height, result);

            var distanceToLeftSite = Mathf.Acos(Vector3.Dot(pointOnEllipse, sut.LeftSite.Position));
            var distanceToRightSite = Mathf.Acos(Vector3.Dot(pointOnEllipse, sut.RightSite.Position));
            var distanceToSweepline = Mathf.Abs(Mathf.Acos(sweepline.Height) - Mathf.Acos(pointOnEllipse.z));

            Assert.Equal(distanceToLeftSite, distanceToRightSite, Tolerance);
            Assert.Equal(distanceToRightSite, distanceToSweepline, Tolerance);
            Assert.Equal(distanceToSweepline, distanceToLeftSite, Tolerance);

            // Teardown
        }


    }
}
