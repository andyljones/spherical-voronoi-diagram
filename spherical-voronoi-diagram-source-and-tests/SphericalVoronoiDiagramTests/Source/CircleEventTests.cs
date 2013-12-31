using UnityEngine;
using Xunit;

namespace SphericalVoronoiDiagramTests
{
    public class CircleEventTests
    {
        [Fact]
        public void Priority_OfACircleEventWithSitesAt90NAnd0N90EAnd0N90W_ShouldBeCorrect
            ()
        {
            // Fixture setup
            var anonymousSweepline = new Sweepline(-1f);
            var sut =
                new CircleEvent(new Arc(MathUtils.CreateSiteAt(90, 0), anonymousSweepline)
                {
                    LeftNeighbour = MathUtils.CreateSiteAt(0, 90),
                    RightNeighbour = MathUtils.CreateSiteAt(0, -90)
                });

            // Exercise system
            var result = sut.Priority;

            // Verify outcome
            var expectedResult = 0;
            Assert.Equal(expectedResult, result, 2);

            // Teardown
        }

        [Fact]
        public void Priority_OfACircleEventWithSitesAt45NAnd90EAnd90W_ShouldBeCorrect
            ()
        {
            // Fixture setup
            var anonymousSweepline = new Sweepline(-1f);
            var sut =
                new CircleEvent(new Arc(MathUtils.CreateSiteAt(45, 0), anonymousSweepline)
                {
                    LeftNeighbour = MathUtils.CreateSiteAt(90, 90),
                    RightNeighbour = MathUtils.CreateSiteAt(90, -90)
                });

            // Exercise system
            var result = sut.Priority;

            // Verify outcome
            var expectedResult = 1-1/Mathf.Sqrt(2);
            Assert.Equal(expectedResult, result, 2);

            // Teardown
        }
    }
}
