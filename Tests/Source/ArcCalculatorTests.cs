using System;
using System.Collections.Generic;
using System.Linq;
using Generator;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using SphericalVoronoiTests.DataAttributes;
using Xunit;
using Xunit.Extensions;

namespace SphericalVoronoiTests
{
    public class ArcCalculatorTests
    {
        private const double Tolerance = 0.000001f;

        [Theory]
        [VectorsAboveSweepline]
        public void PointAt_ShouldReturnAPointEquidistantFromTheSweeplineAndTheFocus
            (Arc arc, Vector3 vector, Sweepline sweepline)
        {
            // Fixture setup
            var focus = arc.Site.Position;

            // Exercise system
            var point = arc.PointAt(vector, sweepline);

            // Verify outcome
            var distanceToFocus = Trig.InverseCosine(point.ScalarMultiply(focus));
            var distanceToSweepline = Math.Abs(sweepline.Colatitude - Trig.InverseCosine(point.Z));

            var failureString = String.Format("Distance to focus was {0}\nDistance to sweepline was {1}", distanceToFocus, distanceToSweepline);
            Assert.True(Number.AlmostEqual(distanceToFocus, distanceToSweepline, Tolerance), failureString);

            // Teardown
        }

        [Theory]
        [VectorsAboveSweepline]
        public void PointAt_ShouldReturnAPointWithTheSameAzimuthAsTheArgument
            (Arc arc, Vector3 vector, Sweepline sweepline)
        {
            // Fixture setup

            // Exercise system
            var point = arc.PointAt(vector, sweepline);

            // Verify outcome
            var argumentAzimuth = Trig.InverseTangentFromRational(vector.Y, vector.X);
            var resultAzimuth = Trig.InverseTangentFromRational(point.Y, point.X);

            var failureString = String.Format("Argument azimuth was {0} \nResult azimuth was {1}", argumentAzimuth, resultAzimuth);
            Assert.True(Number.AlmostEqual(argumentAzimuth, resultAzimuth, Tolerance), failureString);

            // Teardown
        }


        [Theory]
        [VectorsAboveSweepline]
        public void PointAt_IfSweeplinePassesThroughFocusAndVectorIsAtSameAzimuthAsFocus_ShouldReturnFocus
            (Arc arc)            
        {
            // Fixture setup
            var vector = arc.Site.Position;
            var sweepline = new Sweepline {Colatitude = Trig.InverseCosine(arc.Site.Position.Z)};

            // Exercise system
            var point = arc.PointAt(vector, sweepline);

            // Verify outcome
            var failureString = String.Format("Point was {0}", point);

            Assert.True(Vector.AlmostEqual(arc.Site.Position, point, Tolerance), failureString);

            // Teardown
        }


        [Theory]
        [VectorsAboveSweepline]
        public void PointAt_IfSweeplinePassesThroughFocusAndVectorIsNotAtSameAzimuthAsFocus_ShouldReturnNorthPole
            (Arc arc, Vector3 vector)
        {
            // Fixture setup
            var sweepline = new Sweepline { Colatitude = Trig.InverseCosine(arc.Site.Position.Z) };
            var northPole = new Vector3(0, 0, 1);

            // Exercise system
            var point = arc.PointAt(vector, sweepline);

            // Verify outcome
            var failureString = String.Format("Point was {0}", point);

            Assert.True(Vector.AlmostEqual(northPole, point, Tolerance), failureString);

            // Teardown
        }

        [Theory]
        [VectorsAboveSweepline]
        public void LeftIntersection_WhenFociiAreAboveSweepline_ShouldReturnAVectorWhichWhenMappedToAPointOnTheArcIsEquidistantFromBothSites
            (Arc arc, Sweepline sweepline)
        {
            // Fixture setup
            var focus = arc.Site.Position;
            var leftFocus = arc.LeftNeighbour.Position;

            // Exercise system
            var directionOfIntersection = arc.LeftIntersection(sweepline);
            var intersection = arc.PointAt(directionOfIntersection, sweepline);

            // Verify outcome
            var distanceFromSite = Trig.InverseCosine(focus.ScalarMultiply(intersection));
            var distanceFromLeftSite = Trig.InverseCosine(leftFocus.ScalarMultiply(intersection));

            var failureString = String.Format("Direction of intersection: {0},\n" +
                                              "Intersection: {1},\n" + 
                                              "Distance from site: {2},\n" +
                                              "Distance from left site: {3}",
                                              directionOfIntersection, intersection, distanceFromSite, distanceFromLeftSite);
            Assert.True(Number.AlmostEqual(distanceFromSite, distanceFromLeftSite, Tolerance), failureString);

            // Teardown
        }

        [Theory]
        [VectorsAboveSweepline]
        public void LeftIntersection_WhenFociiAreAboveSweepline_ShouldReturnAVectorWhichWhenMappedToAPointOnTheArcIsEquidistantFromSiteAndSweepline
            (Arc arc, Sweepline sweepline)
        {
            // Fixture setup
            var focus = arc.Site.Position;

            // Exercise system
            var directionOfIntersection = arc.LeftIntersection(sweepline);
            var intersection = arc.PointAt(directionOfIntersection, sweepline);

            // Verify outcome
            var distanceFromSite = Trig.InverseCosine(focus.ScalarMultiply(intersection));
            var distanceFromSweepline = Math.Abs(sweepline.Colatitude - intersection.SphericalCoordinates().Colatitude);

            var failureString = String.Format("Direction of intersection: {0},\n" +
                                              "Intersection: {1},\n" +
                                              "Distance from site: {2},\n" +
                                              "Distance from sweepline: {3}",
                                              directionOfIntersection, intersection, distanceFromSite, distanceFromSweepline);
            Assert.True(Number.AlmostEqual(distanceFromSite, distanceFromSweepline, Tolerance), failureString);

            // Teardown
        }

        [Theory]
        [VectorsAboveSweepline]
        public void LeftIntersection_WhenFociiAreAboveSweepline_ShouldBeInOrderLeftIntersectionThenSiteThenRightIntersection
            (Arc arc, Sweepline sweepline)
        {
            // Fixture setup
            var focus = arc.Site.Position;
            var directionOfFocus = new Vector3(focus.X, focus.Y, 0).Normalize();

            var dualArc = new Arc {LeftNeighbour = arc.Site, Site = arc.LeftNeighbour};

            // Exercise system
            var directionOfLeftIntersection = arc.LeftIntersection(sweepline);
            var directionOfRightIntersection = dualArc.LeftIntersection(sweepline);

            // Verify outcome
            var toLeftIntersection = directionOfLeftIntersection - directionOfFocus;
            var toRightIntersection = directionOfRightIntersection - directionOfFocus;
            var areInOrder = toRightIntersection.CrossMultiply(toLeftIntersection)[2] > 0;

            var failureString = String.Format("Direction of left intersection: {0},\n" +
                                              "Direction of right intersection: {1},\n",
                                              directionOfLeftIntersection, directionOfRightIntersection);
            Assert.True(areInOrder, failureString);

            // Teardown
        }

        [Theory]
        [VectorsAboveSweepline]
        public void LeftIntersection_WhenSweeplinePassesThroughLowerOfTheTwoFocii_ShouldReturnDirectionOfThatFocus
            (Arc arc, Sweepline sweepline)
        {
            // Fixture setup
            var focus = arc.Site.Position;
            var leftFocus = arc.Site.Position;
            var lowerFocus = focus.Z < leftFocus.Z ? focus : leftFocus;

            var directionOfLowerFocus = new Vector3(lowerFocus.X, lowerFocus.Y, 0).Normalize();
            sweepline.Colatitude = lowerFocus.SphericalCoordinates().Colatitude;

            // Exercise system
            var directionOfLeftIntersection = arc.LeftIntersection(sweepline);

            // Verify outcome
            var failureString = String.Format("Direction of left intersection: {0},\n",
                                              directionOfLeftIntersection);
            Assert.True(Vector.AlmostEqual(directionOfLowerFocus, directionOfLeftIntersection, Tolerance), failureString);

            // Teardown
        }
    }
}
