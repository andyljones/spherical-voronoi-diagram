using System;
using System.Collections.Generic;
using Generator;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using SphericalVoronoiTests.DataAttributes;
using Xunit;
using Xunit.Extensions;

namespace SphericalVoronoiTests
{
    public class VectorAboveSweeplineAttributeTest
    {
        [Theory]
        [VectorsAboveSweepline]
        public void Vectors_ShouldAllBeOfUnitLength(Vector3 vector)
        {
            // Fixture setup

            // Exercise system

            // Verify outcome
            var result = vector.Norm();
            var expectedResult = 1.0;

            var failureString = String.Format("Had length {0}, expected 1.0", vector);
            Assert.True(Number.AlmostEqual(result, expectedResult), failureString);

            // Teardown
        }

        [Theory]
        [VectorsAboveSweepline]
        public void Vectors_ShouldAllBeAboveSweepline(List<Vector3> vectors, Sweepline anonymousSweepline)
        {
            // Fixture setup

            // Exercise system

            // Verify outcome
            var expectedResult = Trig.Cosine(anonymousSweepline.Colatitude);

            foreach (var vector in vectors)
            {
                var result = vector[2];

                var failureString = String.Format("Had Z {0}, expected Z was {1}", result, expectedResult);
                Assert.True(result >= expectedResult, failureString);
            }
            // Teardown
        }
    }
}
