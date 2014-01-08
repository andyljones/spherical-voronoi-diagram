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
            var a = Utilities.VectorAt(90, -90);
            var b = Utilities.VectorAt(90, 0);
            var c = Utilities.VectorAt(90, 90);

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
            var a = Utilities.VectorAt(90, -90);
            var b = Utilities.VectorAt(90, 0);
            var c = Utilities.VectorAt(90, 90);

            // Exercise system
            var areInOrder = ArcOrderer.AreInOrder(a, c, b);

            // Verify outcome
            Assert.False(areInOrder);

            // Teardown
        }

        [Fact]
        public void AreInOrder_WhenFirstTwoVectorsAreEqual_ShouldReturnTrue()
        {
            // Fixture setup
            var a = Utilities.VectorAt(90, 0);
            var b = Utilities.VectorAt(90, 0);
            var c = Utilities.VectorAt(90, 90);

            // Exercise system
            var areInOrder = ArcOrderer.AreInOrder(a, b, c);

            // Verify outcome
            Assert.True(areInOrder);

            // Teardown
        }

        [Fact]
        public void AreInOrder_WhenLastTwoVectorsAreEqual_ShouldReturnFalse()
        {
            // Fixture setup
            var a = Utilities.VectorAt(90, -90);
            var b = Utilities.VectorAt(90, 0);
            var c = Utilities.VectorAt(90, 0);

            // Exercise system
            var areInOrder = ArcOrderer.AreInOrder(a, b, c);

            // Verify outcome
            Assert.False(areInOrder);

            // Teardown
        }

        [Fact]
        public void AreInOrder_WhenFirstAndLastVectorsAreEqual_ShouldReturnFalse()
        {
            // Fixture setup
            var a = Utilities.VectorAt(90, -90);
            var b = Utilities.VectorAt(90, 0);
            var c = Utilities.VectorAt(90, -90);

            // Exercise system
            var areInOrder = ArcOrderer.AreInOrder(a, b, c);

            // Verify outcome
            Assert.False(areInOrder);

            // Teardown
        }
    }
}
