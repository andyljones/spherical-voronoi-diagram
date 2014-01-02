using SphericalVoronoiDiagramTests.DataAttributes;
using Xunit;
using Xunit.Extensions;

namespace SphericalVoronoiDiagramTests
{
    public class MathUtilsTests
    {
        [Fact]
        public void AreInCyclicOrder_OnVectorsOrderedEastToWest_ShouldReturnPositive1()
        {
            // Fixture setup
            var a = MathUtils.CreateVectorAt(90, 0);
            var b = MathUtils.CreateVectorAt(90, 90);
            var c = MathUtils.CreateVectorAt(90, 180);

            // Exercise system
            var result = MathUtils.AreInCyclicOrder(a, b, c);

            // Verify outcome
            var expectedResult = 1;

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Fact]
        public void AreInCyclicOrder_OnVectorsOrderedWestToEast_ShouldReturnNegative1()
        {
            // Fixture setup
            var a = MathUtils.CreateVectorAt(90, 90);
            var b = MathUtils.CreateVectorAt(90, 0);
            var c = MathUtils.CreateVectorAt(90, 180);

            // Exercise system
            var result = MathUtils.AreInCyclicOrder(a, b, c);

            // Verify outcome
            var expectedResult = -1;

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Fact]
        public void AreInCyclicOrder_OnVectorsWhereTheFirstTwoHaveTheSameAzimuth_ShouldReturn0()
        {
            // Fixture setup
            var a = MathUtils.CreateVectorAt(45, 0);
            var b = MathUtils.CreateVectorAt(90, 0);
            var c = MathUtils.CreateVectorAt(90, 90);

            // Exercise system
            var result = MathUtils.AreInCyclicOrder(a, b, c);

            // Verify outcome
            var expectedResult = 0;

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Fact]
        public void AreInCyclicOrder_OnVectorsWhereTheLastTwoHaveTheSameAzimuth_ShouldReturn0()
        {
            // Fixture setup
            var a = MathUtils.CreateVectorAt(90, 0);
            var b = MathUtils.CreateVectorAt(90, 90);
            var c = MathUtils.CreateVectorAt(45, 90);

            // Exercise system
            var result = MathUtils.AreInCyclicOrder(a, b, c);

            // Verify outcome
            var expectedResult = 0;

            Assert.Equal(expectedResult, result);

            // Teardown
        }
    }
}
