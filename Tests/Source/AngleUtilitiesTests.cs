using System;
using System.Diagnostics;
using Generator;
using MathNet.Numerics;
using SphericalVoronoiTests.DataAttributes;
using Xunit;
using Xunit.Extensions;

namespace SphericalVoronoiTests
{
    public class AngleUtilitiesTests
    {
        private const double Tolerance = 0.000001f;

        [Theory]
        [ZOrderedVectorData]
        public void EquatorialMidpoint_OfTwoNonpolarVectors_ShouldBeEquidistantFromThoseVectorsEquatorialDirections
            (Vector3 u, Vector3 v)
        {
            // Fixture setup
            var directionOfU = AngleUtilities.EquatorialDirection(u);
            var directionOfV = AngleUtilities.EquatorialDirection(v);

            // Exercise system
            var midpoint = AngleUtilities.EquatorialMidpoint(u, v);

            // Verify outcome
            var distanceToU = Trig.InverseCosine(directionOfU.ScalarMultiply(midpoint));
            var distanceToV = Trig.InverseCosine(directionOfV.ScalarMultiply(midpoint));

            Debug.WriteLine(distanceToU + "," + distanceToV);

            var failureString = String.Format("Midpoint was {0}", midpoint);
            Assert.True(Number.AlmostEqual(distanceToU, distanceToV, Tolerance), failureString);

            // Teardown
        }

        [Theory]
        [ZOrderedVectorData]
        public void EquatorialMidpoint_OfTwoNonpolarVectors_ShouldBeInOrderWithTheTwoVectorsEquatorialDirections
            (Vector3 u, Vector3 v)
        {
            // Fixture setup
            var directionOfU = AngleUtilities.EquatorialDirection(u);
            var directionOfV = AngleUtilities.EquatorialDirection(v);

            // Exercise system
            var midpoint = AngleUtilities.EquatorialMidpoint(u, v);

            // Verify outcome
            var areInOrder = ArcOrderer.AreInOrder(directionOfU, midpoint, directionOfV);

            var failureString = String.Format("Midpoint was {0}", midpoint);
            Assert.True(areInOrder, failureString);

            // Teardown
        }
    }
}
