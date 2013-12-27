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
        [Theory]
        [SphericalVectorAndSweeplineData]
        public void Longitude_WhenConvertedToAPointOnTheEllipse_ShouldBePerpendicularToTheVectorBetweenItsSites
            (Intersection sut, float sweeplinePosition)
        {
            // Fixture setup
            var vectorBetweenSites = sut.LeftSite.Position - sut.RightSite.Position;

            // Exercise system


            // Verify outcome
            Assert.True(false, "Test not implemented");

            // Teardown
        }
    }
}
