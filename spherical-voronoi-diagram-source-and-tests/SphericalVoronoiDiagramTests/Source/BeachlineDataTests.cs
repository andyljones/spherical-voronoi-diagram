using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Xunit;
using SphericalVoronoiDiagramTests.FixtureCustomizations;
using Xunit;
using Xunit.Extensions;

namespace SphericalVoronoiDiagramTests
{
    public class BeachlineDataTests
    {
        private const int LengthOfBeachline = 3;

        [Theory]
        [BeachlineData(LengthOfBeachline)]
        public void BeachlineData_ShouldBeSuchThatEachIntersectionsRightSiteIsTheSameAsTheNextIntersectionsLeftSite
            (Beachline sut)
        {
            // Fixture setup

            // Exercise system
            var intersections = sut.ToList();

            // Verify outcome
            var results = 
                intersections.Zip(
                    intersections.Skip(1),
                    (intersection, nextIntersection) => (intersection.RightSite == nextIntersection.LeftSite));

            var result = results.All(b => b) && (intersections.Last().RightSite == intersections.First().LeftSite);

            Assert.True(result);

            // Teardown
        }

        [Theory]
        [BeachlineData(LengthOfBeachline)]
        public void BeachlineData_sIntersectionsShouldBeOrderedByAzimuth
            (Beachline sut)
        {
            // Fixture setup

            // Exercise system
            var result = sut.Select(intersection => intersection.Azimuth).ToList();

            // Verify outcome
            var expectedResult = result.OrderBy(i => i).ToList();

            Assert.Equal(expectedResult, result);

            // Teardown
        }
    }
}
