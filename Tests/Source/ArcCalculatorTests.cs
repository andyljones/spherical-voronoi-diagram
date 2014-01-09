using System;
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
            var sweepline = new Sweepline {Z = arc.Site.Position.Z};

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
            var sweepline = new Sweepline {Z = arc.Site.Position.Z};
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

        //TODO: The ordering described in this test doesn't hold. Find a new test.
        [Theory]
        [VectorsAboveSweepline]
        public void LeftIntersection_OfLowerFocusWhenSweeplineIsBelowIt_ShouldBeInOrderLeftIntersectionThenSiteThenRightIntersection
            (Arc arc, Sweepline sweepline)
        {
            // Fixture setup
            var dualArc = new Arc { LeftNeighbour = arc.Site, Site = arc.LeftNeighbour };

            var lowerArc =  arc.Site.Position.Z <  dualArc.Site.Position.Z ? arc : dualArc;
            var higherArc = arc.Site.Position.Z >= dualArc.Site.Position.Z ? arc : dualArc;
            var directionOfLowerFocus = AngleUtilities.EquatorialDirection(lowerArc.Site.Position);

            // Exercise system
            var directionOfLeftIntersection = lowerArc.LeftIntersection(sweepline);
            var directionOfRightIntersection = higherArc.LeftIntersection(sweepline);

            // Verify outcome
            var areInOrder = ArcOrderer.AreInOrder(directionOfLeftIntersection, directionOfLowerFocus, directionOfRightIntersection);

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
            arc.LeftNeighbour.Position = Utilities.VectorAt(114, 94);
            arc.Site.Position = Utilities.VectorAt(96, 77);
            sweepline = Utilities.SweeplineAt(121);

            // Fixture setup
            var focus = arc.Site.Position;
            var leftFocus = arc.LeftNeighbour.Position;
            var lowerFocus = focus.Z < leftFocus.Z ? focus : leftFocus;

            var directionOfLowerFocus = AngleUtilities.EquatorialDirection(lowerFocus);
            sweepline.Z = lowerFocus.Z;

            // Exercise system
            var directionOfLeftIntersection = arc.LeftIntersection(sweepline);

            // Verify outcome
            var failureString = String.Format("Direction of left intersection: {0},\n",
                                              directionOfLeftIntersection);
            Assert.True(Vector.AlmostEqual(directionOfLowerFocus, directionOfLeftIntersection, Tolerance), failureString);

            // Teardown
        }

        [Theory]
        [VectorsAboveSweepline]
        public void LeftIntersection_WhenLeftNeigbourIsSameAsArcSite_ShouldReturnDirectionOfFocus
            (Arc arc, Sweepline sweepline)
        {
            // Fixture setup
            arc.LeftNeighbour = arc.Site;
            var directionOfFocus = AngleUtilities.EquatorialDirection(arc.Site.Position);

            // Exercise system
            var directionOfLeftIntersection = arc.LeftIntersection(sweepline);

            // Verify outcome
            var failureString = String.Format("Direction of left intersection: {0},\n",
                                              directionOfLeftIntersection);
            Assert.True(Vector.AlmostEqual(directionOfFocus, directionOfLeftIntersection, Tolerance), failureString);

            // Teardown
        }

        [Theory]
        [VectorsAboveSweepline]
        public void LeftIntersection_WhenSweeplinePassesThroughBothFocuses_ShouldReturnEquatorialMidpointOfFocii
            (Arc arc, Sweepline sweepline)
        {
            // Fixture setup
            var colatitudeOfFocus = arc.Site.Position.SphericalCoordinates().Colatitude;
            var azimuthOfFocus = arc.Site.Position.SphericalCoordinates().Azimuth;      
            var azimuthOfLeftFocus = arc.LeftNeighbour.Position.SphericalCoordinates().Azimuth;

            arc.Site.Position = new SphericalCoords(colatitudeOfFocus, azimuthOfFocus).CartesianCoordinates();
            arc.LeftNeighbour.Position = new SphericalCoords(colatitudeOfFocus, azimuthOfLeftFocus).CartesianCoordinates();

            // Exercise system
            var directionOfLeftIntersection = arc.LeftIntersection(sweepline);

            // Verify outcome
            var equatorialMidpoint = AngleUtilities.EquatorialMidpoint(arc.LeftNeighbour.Position, arc.Site.Position);
            var failureString = String.Format("Direction of left intersection: {0},\n",
                                              directionOfLeftIntersection);
            Assert.True(Vector.AlmostEqual(equatorialMidpoint, directionOfLeftIntersection, Tolerance), failureString);

            // Teardown
        }
    }
}
