using System.Security.Policy;
using Ploeh.AutoFixture.Xunit;
using SphericalVoronoiDiagramTests.DataAttributes;
using UnityEngine;
using Xunit;
using Xunit.Extensions;

namespace SphericalVoronoiDiagramTests
{
    public class EllipseIntersectionCalculatorTests
    {
        private const int Tolerance = 2;

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void PointOfIntersection_WhenSitesAreTheSame_ShouldReturnPositionOfSites
            (SiteEvent siteEventA, Sweepline sweepline)
        {
            // Fixture setup

            // Exercise system
            var result = EllipseIntersectionCalculator.IntersectionBetween(siteEventA, siteEventA, sweepline);

            // Verify outcome
            var expectedResult = siteEventA.Position;

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void PointOfIntersection_WhenSitesAreDistinct_ShouldBeEquidistantFromBothSitesAndTheSweepline
            (SiteEvent siteEventA, SiteEvent siteEventB, Sweepline sweepline)
        {
            // Fixture setup

            // Exercise system
            var result = EllipseIntersectionCalculator.IntersectionBetween(siteEventA, siteEventB, sweepline);

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
        public void PointOfIntersection_WhenSweeplineIsSouthOfBothSites_ShouldBeInCyclicOrderAThenIntersectionThenB
            (SiteEvent siteEventA, SiteEvent siteEventB, Sweepline sweepline)
        {
            // Fixture setup

            // Exercise system
            var intersection = EllipseIntersectionCalculator.IntersectionBetween(siteEventA, siteEventB, sweepline);

            // Verify outcome
            var result = MathUtils.AreInCyclicOrder(siteEventA.Position, intersection, siteEventB.Position);

            Assert.True(result >= 0);

            // Teardown
        }
    }
}
