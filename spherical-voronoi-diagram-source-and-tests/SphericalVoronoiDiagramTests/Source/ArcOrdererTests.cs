using System;
using System.Collections.Generic;
using System.Security.Policy;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using SphericalVoronoiDiagramTests.DataAttributes;
using UnityEngine;
using Xunit;
using Xunit.Extensions;

namespace SphericalVoronoiDiagramTests
{
    public class ArcOrdererTests
    {
        [Fact]
        public void AreInOrder_OnThreeCyclicallyOrderedNondegenerateArcs_ShouldReturnTrue()
        {
            // Fixture setup
            var left =   FakeArc(90,-90,   90,-45);
            var middle = FakeArc(90,-45,   90, 45);
            var right =  FakeArc(90, 45,   90, 90);

            // Exercise system
            var result = ArcOrderer.AreInOrder(left, middle, right);

            // Verify outcome
            Assert.True(result);

            // Teardown
        }

        [Fact]
        public void AreInOrder_OnThreeCyclicallyMisorderedNondegenerateArcs_ShouldReturnTrue()
        {
            // Fixture setup
            var left =   FakeArc(90,-90,   90,-45);
            var middle = FakeArc(90,-45,   90, 45);
            var right =  FakeArc(90, 45,   90, 90);

            // Exercise system
            var result = ArcOrderer.AreInOrder(middle, left, right);

            // Verify outcome
            Assert.False(result);

            // Teardown
        }

        [Fact]
        public void AreInOrder_OnThreeCyclicallyOrderedArcsWithADegenerateFirstArc_ShouldReturnTrue()
        {
            // Fixture setup
            var left =   FakeArc(90,-45,   90,-45);
            var middle = FakeArc(90,-45,   90, 45);
            var right =  FakeArc(90, 45,   90, 90);

            // Exercise system
            var result = ArcOrderer.AreInOrder(left, middle, right);

            // Verify outcome
            Assert.True(result);

            // Teardown
        }

        [Fact]
        public void AreInOrder_OnThreeCyclicallyMisorderedArcsWithADegenerateSecondArc_ShouldReturnTrue()
        {
            // Fixture setup
            var left = FakeArc(90, -45, 90, -45);
            var middle = FakeArc(90, -45, 90, 45);
            var right = FakeArc(90, 45, 90, 90);

            // Exercise system
            var result = ArcOrderer.AreInOrder(middle, left, right);

            // Verify outcome
            Assert.False(result);

            // Teardown
        }

        private IArc FakeArc(float leftColatitude, float leftAzimuth, float rightColatitude, float rightAzimuth)
        {
            var leftIntersection = MathUtils.CreateVectorAt(leftColatitude, leftAzimuth);
            var rightIntersection = MathUtils.CreateVectorAt(rightColatitude, rightAzimuth);

            var fakeArc = Substitute.For<IArc>();
            fakeArc.DirectionOfLeftIntersection.ReturnsForAnyArgs(leftIntersection);
            fakeArc.DirectionOfRightIntersection.ReturnsForAnyArgs(rightIntersection);

            return fakeArc;
        }
    }
}
