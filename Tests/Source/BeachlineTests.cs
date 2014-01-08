using System;
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

            Debug.WriteLine(beachline);


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
        public void Test( )
        {
            // Fixture setup

            // Exercise system

            // Verify outcome
            Assert.True(false, "Test not implemented");

            // Teardown
        }
    }
}
