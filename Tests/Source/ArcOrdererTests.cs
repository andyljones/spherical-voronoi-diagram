using Generator;
using NSubstitute;
using Ploeh.AutoFixture.AutoNSubstitute;
using SphericalVoronoiTests.DataAttributes;
using Xunit;
using Xunit.Extensions;

namespace SphericalVoronoiTests
{
    public class ArcOrdererTests
    {
        [Theory]
        [NSubstituteData]
        public void AreInOrder_OnThreeCyclicallyOrderedArcs_ShouldReturnTrue
            (IOrderableArc arcA, IOrderableArc arcB, IOrderableArc arcC)
        {
            // Fixture setup
            arcA.LeftIntersection(null).ReturnsForAnyArgs(new SphericalCoords(90, -90).CartesianCoordinates());
            arcB.LeftIntersection(null).ReturnsForAnyArgs(new SphericalCoords(90, 0).CartesianCoordinates());
            arcC.LeftIntersection(null).ReturnsForAnyArgs(new SphericalCoords(90, 90).CartesianCoordinates());

            var anonymousSweepline = new Sweepline();
            var arcOrderer = new ArcOrderer(anonymousSweepline);

            // Exercise system
            var areInOrder = arcOrderer.AreInOrder(arcA, arcB, arcC);

            // Verify outcome
            Assert.True(areInOrder);

            // Teardown
        }

        [Theory]
        [NSubstituteData]
        public void AreInOrder_OnThreeCyclicallyUnorderedArcs_ShouldReturnFalse
            (IOrderableArc arcA, IOrderableArc arcB, IOrderableArc arcC)
        {
            // Fixture setup
            arcA.LeftIntersection(null).ReturnsForAnyArgs(new SphericalCoords(90, -90).CartesianCoordinates());
            arcB.LeftIntersection(null).ReturnsForAnyArgs(new SphericalCoords(90, 0).CartesianCoordinates());
            arcC.LeftIntersection(null).ReturnsForAnyArgs(new SphericalCoords(90, 90).CartesianCoordinates());

            var anonymousSweepline = new Sweepline();
            var arcOrderer = new ArcOrderer(anonymousSweepline);

            // Exercise system
            var areInOrder = arcOrderer.AreInOrder(arcA, arcC, arcB);

            // Verify outcome
            Assert.False(areInOrder);

            // Teardown
        }
    }
}
