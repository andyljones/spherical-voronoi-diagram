using Generator;
using Xunit;

namespace SphericalVoronoiTests
{
    public class ArcOrdererTests
    {
        [Fact]
        public void AreInOrder_OnThreeCyclicallyOrderedEquatorialVectors_ShouldReturnTrue()
        {
            // Fixture setup
            var a = new SphericalCoords(90, -90).CartesianCoordinates();
            var b = new SphericalCoords(90, 0).CartesianCoordinates();
            var c = new SphericalCoords(90, 90).CartesianCoordinates();

            // Exercise system
            var areInOrder = ArcOrderer.AreInOrder(a, b, c);

            // Verify outcome
            Assert.True(areInOrder);

            // Teardown
        }

        [Fact]
        public void AreInOrder_OnThreeCyclicallyUnorderedEquatorialVectors_ShouldReturnFalse()
        {
            // Fixture setup
            var a = new SphericalCoords(90,-90).CartesianCoordinates();
            var b = new SphericalCoords(90,  0).CartesianCoordinates();
            var c = new SphericalCoords(90, 90).CartesianCoordinates();

            // Exercise system
            var areInOrder = ArcOrderer.AreInOrder(a, c, b);

            // Verify outcome
            Assert.False(areInOrder);

            // Teardown
        }
    }
}
