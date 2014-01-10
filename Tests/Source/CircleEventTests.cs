using System;
using System.Diagnostics;
using Generator;
using SphericalVoronoiTests.DataAttributes;
using Xunit;
using MathNet.Numerics;
using Xunit.Extensions;

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

            Debug.WriteLine(expectedResult);

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

            Debug.WriteLine(expectedResult);

            var failureString = String.Format("Priority was {0}", result);
            Assert.True(Number.AlmostEqual(result, expectedResult, Tolerance), failureString);

            // Teardown
        }

        [Theory]
        [CircleEventData]
        public void Priority_OfACircleEvent_ShouldMatchThePriorityCalculatedByTrigonometry
            (CircleEvent circle)
        {
            // Fixture setup
            var a = circle.LeftArc.Site.Position;
            var b = circle.MiddleArc.Site.Position;
            var c = circle.RightArc.Site.Position;

            var center = (a - b).CrossMultiply(c - b).Normalize();

            var colatitudeOfCenter = Trig.InverseCosine(center[2]);
            var radius = Trig.InverseCosine(a.ScalarMultiply(center));

            var isOnOutsideOfSphere = colatitudeOfCenter + radius <= Constants.Pi;
            var sign = isOnOutsideOfSphere ? 1 : -1;
            var expectedPriority = sign*(1 + Trig.Cosine(colatitudeOfCenter + radius));

            // Exercise system
            var priority = circle.Priority;

            // Verify outcome
            var failureString = 
                String.Format(
                "Expected priority was {0}\nActual priority was {1}", 
                expectedPriority, 
                priority);
            Assert.True(Number.AlmostEqual(expectedPriority, priority, Tolerance), failureString);

            // Teardown
        }

        [Fact]
        public void Test()
        {
            // Fixture setup
            var arcA = new Arc { Site = Utilities.SiteAt(45, -45) };
            var arcB = new Arc { Site = Utilities.SiteAt(0, 0) };
            var arcC = new Arc { Site = Utilities.SiteAt(45, 45) };

            var circle = new CircleEvent(arcA, arcB, arcC);

            var a = arcA.Site.Position;
            var b = arcB.Site.Position;
            var c = arcC.Site.Position;

            var center = (a - b).CrossMultiply(c - b).Normalize();

            var colatitudeOfCenter = Trig.InverseCosine(center[2]);
            var radius = Trig.InverseCosine(a.ScalarMultiply(center));

            var isOnOutsideOfSphere = colatitudeOfCenter + radius <= Constants.Pi;
            var sign = isOnOutsideOfSphere ? 1 : -1;
            var expectedPriority = sign * (1 + Trig.Cosine(colatitudeOfCenter + radius));

            // Exercise system
            var priority = circle.Priority;
            Debug.WriteLine(expectedPriority);
            Debug.WriteLine(Trig.RadianToDegree(colatitudeOfCenter));
            Debug.WriteLine(Trig.RadianToDegree(radius));


            // Verify outcome
            var failureString =
                String.Format(
                "Expected priority was {0}\nActual priority was {1}",
                expectedPriority,
                priority);
            Assert.True(Number.AlmostEqual(expectedPriority, priority, Tolerance), failureString);

            // Teardown
        }
    }
}
