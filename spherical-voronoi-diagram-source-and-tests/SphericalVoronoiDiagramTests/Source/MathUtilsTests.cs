using SphericalVoronoiDiagramTests.DataAttributes;
using Xunit;
using Xunit.Extensions;

namespace SphericalVoronoiDiagramTests
{
    public class MathUtilsTests
    {
        [Fact]
        [SphericalVectorAndSweeplineData]
        public void AreInCyclicOrder_OnVectors0N0EAnd0N90EAnd0N180E_ShouldReturnPositive1()
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
        [SphericalVectorAndSweeplineData]
        public void AreInCyclicOrder_OnVectors0N90EAnd0N0EAnd0N180E_ShouldReturnNegative1()
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
        [SphericalVectorAndSweeplineData]
        public void AreInCyclicOrder_OnVectors45N0EAnd0N0EAnd0N90E_ShouldReturn0()
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
        [SphericalVectorAndSweeplineData]
        public void AreInCyclicOrder_OnVectors0N0EAnd0N90EAnd45N90E_ShouldReturn0()
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
