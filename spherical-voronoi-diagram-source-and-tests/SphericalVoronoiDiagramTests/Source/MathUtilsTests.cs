using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Ploeh.AutoFixture.Xunit;
using SphericalVoronoiDiagramTests.DataAttributes;
using UnityEngine;
using Xunit;
using Xunit.Extensions;
using SDebug = System.Diagnostics.Debug;

namespace SphericalVoronoiDiagramTests
{
    public class MathUtilsTests
    {
        private const int Tolerance = 3; 

        [Fact]
        public void AreInCyclicOrder_OnVectorsOrderedEastToWest_ShouldReturnTrue()
        {
            // Fixture setup
            var a = MathUtils.CreateVectorAtDegrees(90, 0);
            var b = MathUtils.CreateVectorAtDegrees(90, 90);
            var c = MathUtils.CreateVectorAtDegrees(90, 180);

            // Exercise system
            var result = MathUtils.AreInCyclicOrder(a, b, c);

            // Verify outcome
            var expectedResult = true;

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Fact]
        public void AreInCyclicOrder_OnVectorsOrderedWestToEast_ShouldReturnFalse()
        {
            // Fixture setup
            var a = MathUtils.CreateVectorAtDegrees(90, 90);
            var b = MathUtils.CreateVectorAtDegrees(90, 0);
            var c = MathUtils.CreateVectorAtDegrees(90, 180);

            // Exercise system
            var result = MathUtils.AreInCyclicOrder(a, b, c);

            // Verify outcome
            var expectedResult = false;

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Fact]
        public void AreInCyclicOrder_OnVectorsWhereTheFirstTwoHaveTheSameAzimuth_ShouldReturnTrue()
        {
            // Fixture setup
            var a = MathUtils.CreateVectorAtDegrees(45, 0);
            var b = MathUtils.CreateVectorAtDegrees(90, 0);
            var c = MathUtils.CreateVectorAtDegrees(90, 90);

            // Exercise system
            var result = MathUtils.AreInCyclicOrder(a, b, c);

            // Verify outcome
            var expectedResult = true;

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Fact]
        public void AreInCyclicOrder_OnVectorsWhereTheLastTwoHaveTheSameAzimuth_ShouldReturnFalse()
        {
            // Fixture setup
            var a = MathUtils.CreateVectorAtDegrees(90, 0);
            var b = MathUtils.CreateVectorAtDegrees(90, 90);
            var c = MathUtils.CreateVectorAtDegrees(45, 90);

            // Exercise system
            var result = MathUtils.AreInCyclicOrder(a, b, c);

            // Verify outcome
            var expectedResult = false;

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Fact]
        public void AreInCyclicOrder_OnVectorsWhereFirstAndTheLastHaveTheSameAzimuth_ShouldReturnTrue()
        {
            // Fixture setup
            var a = MathUtils.CreateVectorAtDegrees(90, 0);
            var b = MathUtils.CreateVectorAtDegrees(90, 90);
            var c = MathUtils.CreateVectorAtDegrees(90, 0);

            // Exercise system
            var result = MathUtils.AreInCyclicOrder(a, b, c);

            // Verify outcome
            var expectedResult = true;

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void EquatorialMidpoint_OnTwoNonpolarVectors_ShouldBeEquidistantFromBothVectorsWhenProjectedToEquator
            (Vector3 left, Vector3 right)
        {
            // Fixture setup
            
            // Exercise system
            var sut = MathUtils.EquatorialMidpointBetween(left, right);

            // Verify outcome
            var expectedResult = (MathUtils.EquatorialVectorOf(left) - sut).magnitude;
            var result = (MathUtils.EquatorialVectorOf(right) - sut).magnitude;

            Assert.Equal(expectedResult, result, Tolerance);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void EquatorialMidpoint_WithAPolarVector_ShouldReturnTheEquatorialVectorOfTheNonpolarVector
            (Vector3 left)
        {
            // Fixture setup

            // Exercise system
            var sut = MathUtils.EquatorialMidpointBetween(left, new Vector3(0, 0, 1));

            // Verify outcome
            var expectedResult = MathUtils.AzimuthOf(left);
            var result = MathUtils.AzimuthOf(sut);

            Assert.Equal(expectedResult, result, Tolerance);

            // Teardown
        }
    }
}