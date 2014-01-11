using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Generator;
using SphericalVoronoiTests.DataAttributes;
using Xunit;
using Xunit.Extensions;

namespace SphericalVoronoiTests
{
    public class BeachlineTests
    {
        [Theory]
        [ZOrderedVectorData]
        public void Insert_ingASingleSiteIntoTheBeachline_ShouldCreateOneArcWithTheSameSiteOnTheLeft
            (Beachline beachline, SiteEvent site)
        {
            // Fixture setup

            // Exercise system
            beachline.Insert(site);

            // Verify outcome
            var arc = beachline.First();

            var failureString = String.Format("Beachline was {0}", beachline);
            Assert.True(site == arc.Site, failureString);
            Assert.True(site == arc.LeftNeighbour, failureString);

            // Teardown
        }

        [Theory]
        [ZOrderedVectorData]
        public void Insert_ingTwoSitesIntoTheBeachline_ShouldCreateTwoArcsEachWithTheOthersSiteOnTheLeft
            (Beachline beachline, SiteEvent site1, SiteEvent site2)
        {
            // Fixture setup

            // Exercise system
            beachline.Insert(site1);
            beachline.Insert(site2);

            // Verify outcome
            var arc1 = beachline.First();
            var arc2 = beachline.Last();

            var failureString = String.Format("Beachline was {0}", beachline);
            Assert.True(beachline.Count() == 2);
            Assert.True(arc1.Site == arc2.LeftNeighbour, failureString);
            Assert.True(arc2.Site == arc1.LeftNeighbour, failureString);

            // Teardown
        }

        [Theory]
        [ZOrderedVectorData]
        public void Insert_ingThreeSitesIntoTheBeachline_ShouldCreateFourArcsEachWithTheOthersSiteOnTheLeft
            (Beachline beachline, SiteEvent site1, SiteEvent site2, SiteEvent site3)
        {
            // Fixture setup

            // Exercise system
            beachline.Insert(site1);
            beachline.Insert(site2);
            beachline.Insert(site3);

            Debug.WriteLine(beachline);

            // Verify outcome
            var sites = beachline.Select(arc => arc.Site).ToList();
            var leftNeighbours = beachline.Select(arc => arc.LeftNeighbour).ToList();
            leftNeighbours = leftNeighbours.Skip(1).Concat(leftNeighbours.Take(1)).ToList();

            var failureString = String.Format("Beachline was {0}", beachline);
            Assert.True(beachline.Count() == 4, failureString);
            Assert.True(sites.SequenceEqual(leftNeighbours), failureString);

            // Teardown
        }

        [Theory]
        [ZOrderedVectorData]
        public void Insert_ingASiteIntoTheIntersectionBetweenTwoSites_ShouldCreateFourArcsEachWithTheOthersSiteOnTheLeft
            (Beachline beachline, SiteEvent leftSite, SiteEvent centralSite)
        {
            // Fixture setup
            var upperColatitude = leftSite.Position.SphericalCoordinates().Colatitude;
            var leftAzimuth = leftSite.Position.SphericalCoordinates().Azimuth;
            var centralAzimuth = centralSite.Position.SphericalCoordinates().Azimuth;
            var rightSite = new SiteEvent(new SphericalCoords(upperColatitude, centralAzimuth + (centralAzimuth - leftAzimuth)).CartesianCoordinates());

            // Exercise system
            beachline.Insert(leftSite);
            beachline.Insert(rightSite);
            beachline.Insert(centralSite);

            // Verify outcome
            var sites = beachline.Select(arc => arc.Site).ToList();
            var leftNeighbours = beachline.Select(arc => arc.LeftNeighbour).ToList();
            leftNeighbours = leftNeighbours.Skip(1).Concat(leftNeighbours.Take(1)).ToList();

            var failureString = String.Format("Beachline was {0}", beachline);
            Assert.True(beachline.Count() == 4, failureString);
            Assert.True(sites.SequenceEqual(leftNeighbours), failureString);

            // Teardown
        }

        [Theory]
        [ZOrderedVectorData]
        public void Insert_ingThreeSitesIntoTheBeachline_ShouldInsertTwoItemsIntoThePotentialCircleEventsList
            (Beachline beachline, SiteEvent site1, SiteEvent site2, SiteEvent site3)
        {
            // Fixture setup

            // Exercise system
            beachline.Insert(site1);
            beachline.Insert(site2);
            beachline.Insert(site3);

            // Verify outcome
            var result = beachline.PotentialCircleEvents.Count;
            
            var failureString = 
                String.Format("Beachline was {0}\n" +
                              "Modified arcs list was {1}", 
                              beachline, 
                              Utilities.ToString(beachline.PotentialCircleEvents));
            Assert.True(result == 2, failureString);

            // Teardown
        }

        [Theory]
        [ZOrderedVectorData]
        public void Remov_ingAZeroLengthArc_ShouldLeaveThreeArcsEachWithTheOthersSiteOnTheLeft
            (Beachline beachline, SiteEvent leftSite, SiteEvent centralSite)
        {
            // Fixture setup
            var upperColatitude = leftSite.Position.SphericalCoordinates().Colatitude;
            var leftAzimuth = leftSite.Position.SphericalCoordinates().Azimuth;
            var centralAzimuth = centralSite.Position.SphericalCoordinates().Azimuth;
            var rightSite = new SiteEvent(new SphericalCoords(upperColatitude, centralAzimuth + (centralAzimuth - leftAzimuth)).CartesianCoordinates());

            beachline.Insert(leftSite);
            beachline.Insert(rightSite);
            beachline.Insert(centralSite);

            var arcThatWasSplit = beachline.Where(arc1 => beachline.Count(arc2 => arc1.Site == arc2.Site) > 1);
            var arcToBeRemoved = arcThatWasSplit.First(arc => arc.Site.Position.Z > beachline.Sweepline.Z);

            // Exercise system
            Debug.WriteLine(beachline);
            Debug.WriteLine(arcToBeRemoved);
            beachline.Remove(arcToBeRemoved);

            Debug.WriteLine(beachline);

            // Verify outcome
            var sites = beachline.Select(arc => arc.Site).ToList();
            var leftNeighbours = beachline.Select(arc => arc.LeftNeighbour).ToList();
            leftNeighbours = leftNeighbours.Skip(1).Concat(leftNeighbours.Take(1)).ToList();

            var failureString = String.Format("Beachline was {0}", beachline);
            Assert.True(beachline.Count() == 3, failureString);
            Assert.True(sites.SequenceEqual(leftNeighbours), failureString);

            // Teardown
        }
    }
}
