using System;
using System.Collections.Generic;
using System.Security.Policy;
using Ploeh.AutoFixture.Xunit;
using SphericalVoronoiDiagramTests.DataAttributes;
using UnityEngine;
using Xunit;
using Xunit.Extensions;

namespace SphericalVoronoiDiagramTests
{
    public class ArcOrdererTests
    {
        [Theory]
        [SphericalVectorAndSweeplineData]
        public void AreInOrder_WhenFirstAndLastArgumentsAreTheSame_ShouldAlwaysBeTrue
            (Arc arcA, Arc arcB)
        {
            // Fixture setup

            // Exercise system
            var result = ArcOrderer.AreInOrder(arcA, arcB, arcA);

            // Verify outcome
            Assert.True(result);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void AreInOrder_OnThreeArcsWhoseLeftIntersectionsAreInCyclicOrder_ShouldBeTrue
            (Arc arcA, Arc arcB, Arc arcC)
        {
            // Fixture setup
            var orderedArcA = arcA;
            Arc orderedArcB;
            Arc orderedArcC;

            if (MathUtils.AreInCyclicOrder(arcA.LeftIntersection(), arcB.LeftIntersection(), arcC.LeftIntersection()))
            {
                orderedArcB = arcB;
                orderedArcC = arcC;
            }
            else
            {
                orderedArcB = arcC;
                orderedArcC = arcB;
            }

            // Exercise system
            var result = ArcOrderer.AreInOrder(orderedArcA, orderedArcB, orderedArcC);

            // Verify outcome
            Assert.True(result);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void AreInOrder_OnThreeArcsWhoseLeftIntersectionsAreNotCyclicOrder_ShouldBeFalse
            (Arc arcA, Arc arcB, Arc arcC)
        {
            // Fixture setup
            var orderedArcA = arcA;
            Arc orderedArcB;
            Arc orderedArcC;

            if (MathUtils.AreInCyclicOrder(arcA.LeftIntersection(), arcB.LeftIntersection(), arcC.LeftIntersection()))
            {
                orderedArcB = arcB;
                orderedArcC = arcC;
            }
            else
            {
                orderedArcB = arcC;
                orderedArcC = arcB;
            }

            // Exercise system
            var result = ArcOrderer.AreInOrder(orderedArcA, orderedArcC, orderedArcB);

            // Verify outcome
            Assert.False(result);

            // Teardown
        }
    }
}
