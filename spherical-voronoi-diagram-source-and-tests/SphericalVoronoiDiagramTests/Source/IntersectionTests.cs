using System;
using System.Security.Policy;
using Ploeh.AutoFixture.Xunit;
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
        public void Longitude_WhenConvertedToAPointOnTheEllipse_ShouldBePerpendicularToTheVectorBetweenItsSites
            (Intersection sut, Sweepline sweepline)
        {

            //var a = new Vector3(1.0f, -0.1f, 0.1f).normalized;
            //var b = new Vector3(0.0f, -0.4f, -0.9f).normalized;
            //var h = 0.05f;
            //sut = new Intersection(new Site(a), new Site(b), new Sweepline(h));

            // Fixture setup
            var vectorBetweenSites = sut.LeftSite.Position - sut.RightSite.Position;

            // Exercise system
            var longitude = sut.Longitude();

            // Verify outcome
            var pointOnEllipse = EllipseDrawer.PointOnEllipse(sut.LeftSite.Position, sweepline.Height, longitude);

            var result = Vector3.Dot(vectorBetweenSites, pointOnEllipse);

            Assert.Equal(0, result, Tolerance);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void Longitude_WhenConvertedToAPointOnTheEllipse_ShouldBeEquidistantFromBothSitesAndTheSweepline
            (Intersection sut, Sweepline sweepline)
        {
            // Fixture setup

            // Exercise system
            var longitude = sut.Longitude();

            // Verify outcome
            var pointOnEllipse = EllipseDrawer.PointOnEllipse(sut.LeftSite.Position, sweepline.Height, longitude);

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
