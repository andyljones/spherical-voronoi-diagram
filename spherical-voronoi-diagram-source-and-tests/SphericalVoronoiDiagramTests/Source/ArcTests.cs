using System;
using System.Collections.Generic;
using CyclicalSkipList;
using SphericalVoronoiDiagramTests.DataAttributes;
using UnityEngine;
using Xunit;
using Xunit.Extensions;
using Debug = System.Diagnostics.Debug; 

namespace SphericalVoronoiDiagramTests
{
    public class ArcTests
    {
        private const int Tolerance = 2;

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void AzimuthOfLeftIntersection_WhenLeftNeighbourSiteIsTheSameAsArcSite_ShouldReturnAzimuthOfArcSite
            (Site siteA, Sweepline sweepline)
        {
            // Fixture setup
            var sut = new Arc(siteA, sweepline);

            var expectedResult = sut.Site.Azimuth();

            // Exercise system
            var result = sut.AzimuthOfLeftIntersection();

            // Verify outcome
            Assert.Equal(result, expectedResult);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void AzimuthOfLeftIntersection_WhenBothSitesAreDefined_ShouldBeEquidistantFromBothSitesAndTheSweeplineWhenConvertedToAPointOnTheEllipse
            (Site siteA, Site siteB, Sweepline sweepline)
        {
            // Fixture setup
            var sut = new Arc(siteA, sweepline) {LeftNeighbour = siteB, RightNeighbour = siteB};

            // Exercise system
            var result = sut.AzimuthOfLeftIntersection();

            // Verify outcome
            var pointOnEllipse = BeachlineDrawer.PointOnEllipse(sut, result);

            var distanceToLeftNeighbour = Mathf.Acos(Vector3.Dot(pointOnEllipse, sut.LeftNeighbour.Position));
            var distanceToSite = Mathf.Acos(Vector3.Dot(pointOnEllipse, sut.Site.Position));
            var distanceToSweepline = Mathf.Abs(Mathf.Acos(sweepline.Z) - Mathf.Acos(pointOnEllipse.z));

            Assert.Equal(distanceToLeftNeighbour, distanceToSite, Tolerance);
            Assert.Equal(distanceToSite, distanceToSweepline, Tolerance);
            Assert.Equal(distanceToSweepline, distanceToLeftNeighbour, Tolerance);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void AzimuthOfRightIntersection_WhenBothSitesAreDefined_ShouldBeEquidistantFromBothSitesAndTheSweeplineWhenConvertedToAPointOnTheEllipse
            (Site siteA, Site siteB, Sweepline sweepline)
        {
            // Fixture setup
            var sut = new Arc(siteA, sweepline) { LeftNeighbour = siteB, RightNeighbour = siteB };

            // Exercise system
            var result = sut.AzimuthOfRightIntersection();

            // Verify outcome
            var pointOnEllipse = BeachlineDrawer.PointOnEllipse(sut, result);

            var distanceToLeftNeighbour = Mathf.Acos(Vector3.Dot(pointOnEllipse, sut.LeftNeighbour.Position));
            var distanceToSite = Mathf.Acos(Vector3.Dot(pointOnEllipse, sut.Site.Position));
            var distanceToSweepline = Mathf.Abs(Mathf.Acos(sweepline.Z) - Mathf.Acos(pointOnEllipse.z));

            Assert.Equal(distanceToLeftNeighbour, distanceToSite, Tolerance);
            Assert.Equal(distanceToSite, distanceToSweepline, Tolerance);
            Assert.Equal(distanceToSweepline, distanceToLeftNeighbour, Tolerance);

            // Teardown
        }

        [Theory]
        [SphericalVectorAndSweeplineData]
        public void AzimuthsOfIntersection_WhenBothSitesAreDefined_ShouldBeInOrderWithRespectToTheArcsSite
            (Site siteA, Site siteB, Sweepline sweepline)
        {
            // Fixture setup
            var sut = new Arc(siteA, sweepline) { LeftNeighbour = siteB, RightNeighbour = siteB };

            Func<float, float, float, bool> inOrder = 
                new CompareToCyclicOrdererAdapter<float>(Comparer<float>.Default.Compare).InOrder;

            // Exercise system
            var leftAzimuth = sut.AzimuthOfLeftIntersection();
            var siteAzimuth = sut.Site.Azimuth();
            var rightAzimuth = sut.AzimuthOfRightIntersection();

            // Verify outcome
            var failureString = String.Format(
                "Left azimuth: {0}\n Site azimuth: {1}\n Right azimuth: {2}\n were found to be not in order",
                180 / Mathf.PI * leftAzimuth,
                180 / Mathf.PI * siteAzimuth,
                180 / Mathf.PI * rightAzimuth);

            Assert.True(inOrder(leftAzimuth, siteAzimuth, rightAzimuth), failureString);

            // Teardown
        }

    }
}
