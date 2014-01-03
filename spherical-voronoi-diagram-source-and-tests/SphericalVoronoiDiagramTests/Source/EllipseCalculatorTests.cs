using System.Security.Policy;
using Ploeh.AutoFixture.Xunit;
using SphericalVoronoiDiagramTests.DataAttributes;
using UnityEngine;
using Xunit;
using Xunit.Extensions;
using SDebug = System.Diagnostics.Debug;

namespace SphericalVoronoiDiagramTests
{
    public class EllipseCalculatorTests
    {
        private const int Tolerance = 4;

        [Fact]
        public void EquatorialVectorOfIntersection_WhenBothSitesLieOnSweepline_ShouldGiveAnEquatorialVectorHalfwayBetweenTheSites
            ()
        {
            // Fixture setup
            var leftSite = MathUtils.CreateSiteAt(90, -90);
            var rightSite = MathUtils.CreateSiteAt(90, 90);

            var sweepline = new Sweepline(0.0f);

            // Exercise system
            var result = EllipseCalculator.EquatorialVectorOfIntersection(leftSite, rightSite, sweepline);

            // Verify outcome
            var expectedResult = new Vector3(1, 0, 0);

            Assert.Equal(0, (expectedResult - result).magnitude, Tolerance);

            // Teardown
        }

        [Fact]
        public void EquatorialVectorOfIntersection_WhenLeftSiteLiesOnSweepline_ShouldGiveAnEquatorialVectorThroughTheLeftSite
            ()
        {
            // Fixture setup
            var leftSite = MathUtils.CreateSiteAt(90, -90);
            var rightSite = MathUtils.CreateSiteAt(45, 90);

            var sweepline = new Sweepline(0.0f);

            // Exercise system
            var result = EllipseCalculator.EquatorialVectorOfIntersection(leftSite, rightSite, sweepline);

            // Verify outcome
            var expectedResult = new Vector3(0, 1, 0);

            Assert.Equal(0, (expectedResult - result).magnitude, Tolerance);

            // Teardown
        }


        [Fact]
        public void EquatorialVectorOfIntersection_WhenRightSiteLiesOnSweepline_ShouldGiveAnEquatorialVectorThroughTheRightSite
            ()
        {
            // Fixture setup
            var leftSite = MathUtils.CreateSiteAt(45, -90);
            var rightSite = MathUtils.CreateSiteAt(90, 90);

            var sweepline = new Sweepline(0.0f);

            // Exercise system
            var result = EllipseCalculator.EquatorialVectorOfIntersection(leftSite, rightSite, sweepline);

            // Verify outcome
            var expectedResult = new Vector3(0, -1, 0);

            Assert.Equal(0, (expectedResult - result).magnitude, Tolerance);

            // Teardown
        }

        [Fact]
        public void EquatorialVectorOfIntersection_WhenSitesHaveTheSameZ_ShouldGiveAVectorHalfWayBetweenTheTwoSites1
            ()
        {
            // Fixture setup
            var leftSite = MathUtils.CreateSiteAt(90, -90);
            var rightSite = MathUtils.CreateSiteAt(90, 90);

            var sweepline = new Sweepline(-0.5f);

            // Exercise system
            var result = EllipseCalculator.EquatorialVectorOfIntersection(leftSite, rightSite, sweepline);

            // Verify outcome
            var expectedResult = new Vector3(1, 0, 0);

            Assert.Equal(0, (expectedResult - result).magnitude, Tolerance);

            // Teardown
        }

        [Fact]
        public void EquatorialVectorOfIntersection_WhenSitesHaveTheSameZ_ShouldGiveAVectorHalfWayBetweenTheTwoSites2
            ()
        {
            // Fixture setup
            var leftSite = MathUtils.CreateSiteAt(90, 0);
            var rightSite = MathUtils.CreateSiteAt(90, 180);

            var sweepline = new Sweepline(-0.5f);

            // Exercise system
            var result = EllipseCalculator.EquatorialVectorOfIntersection(leftSite, rightSite, sweepline);

            // Verify outcome
            var expectedResult = new Vector3(0, -1, 0);

            Assert.Equal(0, (expectedResult - result).magnitude, Tolerance);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void EquatorialVectorOfIntersection_ForGeneralSites_ShouldLieBetweenTheTwoSites
            (SiteEvent leftSite, SiteEvent rightSite, Sweepline sweepline)
        {
            // Fixture setup

            // Exercise system
            var sut = EllipseCalculator.EquatorialVectorOfIntersection(leftSite, rightSite, sweepline);

            // Verify outcome
            var inOrder = MathUtils.AreInCyclicOrder(leftSite.Position, sut, rightSite.Position);
            
            Assert.True(inOrder);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void PointOnEllipseAboveVector_ShouldGenerateAPointWithTheSameAzimuthAsVectorGiven
            (Vector3 vector, Vector3 focus, Sweepline sweepline)
        {
            // Fixture setup

            // Exercise system
            var point = EllipseCalculator.PointOnEllipseAboveVector(vector, focus, sweepline);

            // Verify outcome
            var expectedResult = MathUtils.AzimuthOf(vector);
            var result = MathUtils.AzimuthOf(point);

            Assert.Equal(expectedResult, result, Tolerance);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void PointOnEllipseAboveVector_ShouldGenerateAPointEquidistantFromTheFocusAndTheSweepline
            (Vector3 anonymousVector, Vector3 focus, Sweepline sweepline)
        {
            // Fixture setup
            
            // Exercise system
            var result = EllipseCalculator.PointOnEllipseAboveVector(anonymousVector, focus, sweepline);

            // Verify outcome
            var distanceFromSite = Mathf.Acos(Vector3.Dot(result, focus));
            var distanceFromSweepline = Mathf.Abs(Mathf.Acos(result.z) - Mathf.Acos(sweepline.Z));

            Assert.Equal(distanceFromSite, distanceFromSweepline, Tolerance);

            // Teardown
        }


        [Fact]
        public void IntersectionBetween_WhenBothSitesLieOnSweepline_ShouldGiveNorthPole
            ()
        {
            // Fixture setup
            var leftSite = MathUtils.CreateSiteAt(90, -90);
            var rightSite = MathUtils.CreateSiteAt(90, 90);

            var sweepline = new Sweepline(0.0f);

            // Exercise system
            var result = EllipseCalculator.IntersectionBetween(leftSite, rightSite, sweepline);

            // Verify outcome
            var expectedResult = new Vector3(0, 0, 1);

            Assert.Equal(0, (expectedResult - result).magnitude, Tolerance);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void IntersectionBetween_WhenSitesAreDistinct_ShouldBeEquidistantFromBothSitesAndTheSweepline
            (SiteEvent siteEventA, SiteEvent siteEventB, Sweepline sweepline)
        {
            // Fixture setup

            // Exercise system
            var result = EllipseCalculator.IntersectionBetween(siteEventA, siteEventB, sweepline);

            // Verify outcome
            var distanceToLeftNeighbour = Mathf.Acos(Vector3.Dot(result, siteEventA.Position));
            var distanceToSite = Mathf.Acos(Vector3.Dot(result, siteEventB.Position));
            var distanceToSweepline = Mathf.Abs(Mathf.Acos(sweepline.Z) - Mathf.Acos(result.z));

            Assert.Equal(distanceToLeftNeighbour, distanceToSite, Tolerance);
            Assert.Equal(distanceToSite, distanceToSweepline, Tolerance);
            Assert.Equal(distanceToSweepline, distanceToLeftNeighbour, Tolerance);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void IntersectionBetween_WhenSweeplineIsSouthOfBothSites_ShouldBeInCyclicOrderAThenIntersectionThenB
            (SiteEvent siteEventA, SiteEvent siteEventB, Sweepline sweepline)
        {
            // Fixture setup

            // Exercise system
            var intersection = EllipseCalculator.IntersectionBetween(siteEventA, siteEventB, sweepline);

            // Verify outcome
            var result = MathUtils.AreInCyclicOrder(siteEventA.Position, intersection, siteEventB.Position);

            Assert.True(result);

            // Teardown
        }
    }
}
