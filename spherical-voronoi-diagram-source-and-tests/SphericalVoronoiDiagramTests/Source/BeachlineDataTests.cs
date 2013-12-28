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
        [Theory]
        [BeachlineData(3)]
        public void BeachlineData_ShouldBeSuchThatEachIntersectionsRightSiteIsTheSameAsTheNextIntersectionsLeftSite
            (Beachline sut)
        {
            // Fixture setup

            // Exercise system
            var intersections = sut.ToList();
            var results = intersections.Zip(
                intersections.Skip(1), 
                (intersection, nextIntersection) => (intersection.RightSite == nextIntersection.LeftSite));
            
            var result = results.All(b => b) && (intersections.Last().RightSite == intersections.First().LeftSite);

            // Verify outcome
            Assert.True(result);

            // Teardown
        }
    }
}
