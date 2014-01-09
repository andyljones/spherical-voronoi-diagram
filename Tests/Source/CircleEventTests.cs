using System;
using Generator;
using Xunit;
using MathNet.Numerics;

namespace SphericalVoronoiTests
{
    public class CircleEventTests
    {
        private const double Tolerance = 0.000001f;

        [Fact]
        public void Priority_OfACircleThrough0N90WAnd90NAnd0N90E_ShouldBe0()
        {
            // Fixture setup
            var arcA = new Arc {Site = Utilities.SiteAt(90, -90)};
            var arcB = new Arc {Site = Utilities.SiteAt(0, 0)};
            var arcC = new Arc {Site = Utilities.SiteAt(90, 90)};

            var circle = new CircleEvent(arcA, arcB, arcC); 

            // Exercise system
            var result = circle.Priority;

            // Verify outcome
            var expectedResult = 0;

            var failureString = String.Format("Priority was {0}", result);
            Assert.True(Number.AlmostEqual(result, expectedResult, Tolerance), failureString);

            // Teardown
        }

        [Fact]
        public void Priority_OfACircleThrough0N45WAnd45N0EAnd0N45E_ShouldBeCorrect()
        {
            // Fixture setup
            var arcA = new Arc { Site = Utilities.SiteAt(90, -45) };
            var arcB = new Arc { Site = Utilities.SiteAt(45, 0) };
            var arcC = new Arc { Site = Utilities.SiteAt(90, 45) };

            var circle = new CircleEvent(arcA, arcB, arcC);

            // Exercise system
            var result = circle.Priority;

            // Verify outcome
            var expectedResult = Trig.Cosine(Trig.DegreeToRadian(135)) + 1;

            var failureString = String.Format("Priority was {0}", result);
            Assert.True(Number.AlmostEqual(result, expectedResult, Tolerance), failureString);

            // Teardown
        }

        [Fact]
        public void Priority_OfACircleThrough0N45EAnd45N0EAnd0N45W_ShouldBeCorrect()
        {
            // Fixture setup
            var arcA = new Arc { Site = Utilities.SiteAt(90, 45) };
            var arcB = new Arc { Site = Utilities.SiteAt(45, 0) };
            var arcC = new Arc { Site = Utilities.SiteAt(90, -45) };

            var circle = new CircleEvent(arcA, arcB, arcC);

            // Exercise system
            var result = circle.Priority;

            // Verify outcome
            var expectedResult = -Trig.Cosine(Trig.DegreeToRadian(135)) - 1;

            var failureString = String.Format("Priority was {0}", result);
            Assert.True(Number.AlmostEqual(result, expectedResult, Tolerance), failureString);

            // Teardown
        }
    }
}
