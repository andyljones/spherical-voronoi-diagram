using System.Collections.Generic;
using System.Linq;
using SphericalVoronoiDiagramTests.DataAttributes;
using Xunit;
using Xunit.Extensions;

namespace SphericalVoronoiDiagramTests
{
    public class BeachlineTests
    {
        [Theory]
        [BeachlineData(1)]
        public void Insert_ingASiteIntoABeachline_ShouldSetTheSweeplinesHeightToTheNodesHeight
            (Beachline sut, List<Site> anonymousSites)
        {
            // Fixture setup
            var anonymousSite = anonymousSites.First();

            var expectedResult = anonymousSite.Position.z;

            // Exercise system
            sut.Insert(anonymousSite);

            // Verify outcome
            var result = sut.Sweepline.Height;

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [BeachlineData(1)]
        public void Insert_ingASiteIntoAnEmptyBeachline_ShouldAddAnArcWithThatSiteAsItsSite
            (Beachline sut, List<Site> anonymousSites)
        {
            // Fixture setup
            var anonymousSite = anonymousSites.First();

            // Exercise system
            sut.Insert(anonymousSite);

            // Verify outcome
            var result = sut.First();

            Assert.Equal(anonymousSite, result.Site);

            // Teardown
        }

        [Theory]
        [BeachlineData(1)]
        public void Insert_ingASiteIntoAnEmptyBeachline_ShouldAddAnArcNeighbouredByItself
            (Beachline sut, List<Site> anonymousSites)
        {
            // Fixture setup
            var anonymousSite = anonymousSites.First();

            // Exercise system
            sut.Insert(anonymousSite);

            // Verify outcome
            var result = sut.First();
            
            Assert.Equal(anonymousSite, result.LeftNeighbour);
            Assert.Equal(anonymousSite, result.RightNeighbour);

            // Teardown
        }

        [Theory]
        [BeachlineData(2)]
        public void Insert_ingTwoSitesIntoAnEmptyBeachlineWhenTheSecondIsLowerThanTheFirst_ShouldAddTwoArcsNeighbouredByEachother
            (Beachline sut, List<Site> anonymousSites)
        {
            // Fixture setup
            var anonymousSiteA = anonymousSites[0];
            var anonymousSiteB = anonymousSites[1];

            // Exercise system
            sut.Insert(anonymousSiteA);
            sut.Insert(anonymousSiteB);

            // Verify outcome
            var result1 = sut.First();
            var result2 = sut.Last();

            Assert.Equal(result2.Site, result1.LeftNeighbour);
            Assert.Equal(result2.Site, result1.RightNeighbour);

            Assert.Equal(result1.Site, result2.LeftNeighbour);
            Assert.Equal(result1.Site, result2.RightNeighbour);

            // Teardown
        }

        [Theory]
        [BeachlineData(3)]
        public void Insert_ingThreeSitesIntoAnEmptyBeachlineInDescendingOrderOfHeight_ShouldAddFourArcs
            (Beachline sut, List<Site> anonymousSites)
        {
            // Fixture setup
            var expectedResult = 4;
            
            var anonymousSiteA = anonymousSites[0];
            var anonymousSiteB = anonymousSites[1];
            var anonymousSiteC = anonymousSites[2];

            // Exercise system
            sut.Insert(anonymousSiteA);
            sut.Insert(anonymousSiteB);
            sut.Insert(anonymousSiteC);

            // Verify outcome
            var result = sut.Count();

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [BeachlineData(3)]
        public void Insert_ingThreeSitesIntoAnEmptyBeachlineInDescendingOrderOfHeight_ShouldGiveArcsWithEachRightNeighbourBeingTheNextArcsSite
            (Beachline sut, List<Site> anonymousSites)
        {
            // Fixture setup
            var anonymousSiteA = anonymousSites[0];
            var anonymousSiteB = anonymousSites[1];
            var anonymousSiteC = anonymousSites[2];

            // Exercise system
            sut.Insert(anonymousSiteA);
            sut.Insert(anonymousSiteB);
            sut.Insert(anonymousSiteC);

            // Verify outcome
            var sites = sut.Select(arc => arc.Site).ToList();
            var expectedResult = sites.Skip(1).Concat(sites.Take(1)).ToList();
            var result = sut.Select(arc => arc.RightNeighbour).ToList();

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [BeachlineData(3)]
        public void Insert_ingThreeSitesIntoAnEmptyBeachlineInDescendingOrderOfHeight_ShouldGiveArcsWithEachLeftNeighbourBeingThePreviousArcsSite
            (Beachline sut, List<Site> anonymousSites)
        {
            // Fixture setup
            var anonymousSiteA = anonymousSites[0];
            var anonymousSiteB = anonymousSites[1];
            var anonymousSiteC = anonymousSites[2];

            // Exercise system
            sut.Insert(anonymousSiteA);
            sut.Insert(anonymousSiteB);
            sut.Insert(anonymousSiteC);

            // Verify outcome
            var sites = sut.Select(arc => arc.Site).ToList();
            var expectedResult = sites.Skip(sites.Count-1).Concat(sites.Take(sites.Count-1)).ToList();
            var result = sut.Select(arc => arc.LeftNeighbour).ToList();

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [BeachlineData(3)]
        public void Insert_ingThreeSitesIntoAnEmptyBeachlineInDescendingOrderOfHeight_ShouldGiveArcsOrderedByTheirLeftThenRightAzimuth
            (Beachline sut, List<Site> anonymousSites)
        {
            // Fixture setup
            var anonymousSiteA = anonymousSites[0];
            var anonymousSiteB = anonymousSites[1];
            var anonymousSiteC = anonymousSites[2];

            // Exercise system
            sut.Insert(anonymousSiteA);
            sut.Insert(anonymousSiteB);
            sut.Insert(anonymousSiteC);

            // Verify outcome
            var sites = sut.Select(arc => arc.Site).ToList();
            var expectedResult = sites.Skip(sites.Count - 1).Concat(sites.Take(sites.Count - 1)).ToList();
            var result = sut.Select(arc => arc.LeftNeighbour).ToList();

            Assert.Equal(expectedResult, result);

            // Teardown
        }
    }
}
