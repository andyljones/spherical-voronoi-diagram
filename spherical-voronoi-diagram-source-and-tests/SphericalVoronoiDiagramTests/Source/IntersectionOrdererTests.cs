using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Xunit;
using SphericalVoronoiDiagramTests.FixtureCustomizations;
using Xunit;
using Xunit.Extensions;

namespace SphericalVoronoiDiagramTests
{
    public class IntersectionOrdererTests
    {
        [Theory]
        [SphericalVectorAndSweeplineData]
        public void InOrder_ForThreeIntersectionsOrderedByIncreasingAzimuth_ShouldReturnTrue
            (List<Intersection> intersections)
        {
            // Fixture setup
            intersections = intersections.OrderBy(intersection => intersection.Azimuth).ToList();

            // Exercise system
            var result = IntersectionOrderer.InOrder(intersections[0], intersections[1], intersections[2]);

            // Verify outcome
            Assert.True(result);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void InOrder_ForACyclicPermutationOfThreeIntersectionsOrderedByIncreasingAzimuth_ShouldReturnTrue
            (List<Intersection> intersections)
        {
            // Fixture setup
            intersections = intersections.OrderBy(intersection => intersection.Azimuth).ToList();

            // Exercise system
            var result = IntersectionOrderer.InOrder(intersections[1], intersections[2], intersections[0]);

            // Verify outcome
            Assert.True(result);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void InOrder_ForANoncyclicPermutationOfThreeIntersectionsOrderedByIncreasingAzimuth_ShouldReturnFalse
            (List<Intersection> intersections)
        {
            // Fixture setup
            intersections = intersections.OrderBy(intersection => intersection.Azimuth).ToList();

            // Exercise system
            var result = IntersectionOrderer.InOrder(intersections[0], intersections[2], intersections[1]);

            // Verify outcome
            Assert.False(result);

            // Teardown
        }
    }
}
