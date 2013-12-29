using System;
using System.Collections.Generic;
using System.Linq;
using CyclicalSkipList;
using SphericalVoronoiDiagramTests.DataAttributes;
using UnityEngine;
using Xunit;
using Xunit.Extensions;
using Debug = System.Diagnostics.Debug;

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
            var result = sut.Sweepline.Z;

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
        public void
            Insert_ingTwoSitesIntoAnEmptyBeachlineWhenTheSecondIsLowerThanTheFirst_ShouldAddTwoArcsNeighbouredByEachother
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

            Debug.WriteLine(sut);

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
        public void
            Insert_ingThreeSitesIntoAnEmptyBeachlineInDescendingOrderOfHeight_ShouldGiveArcsWithEachRightNeighbourBeingTheNextArcsSite
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
        public void
            Insert_ingThreeSitesIntoAnEmptyBeachlineInDescendingOrderOfHeight_ShouldGiveArcsWithEachLeftNeighbourBeingThePreviousArcsSite
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

            Debug.WriteLine(sut);

            // Verify outcome
            var sites = sut.Select(arc => arc.Site).ToList();
            var expectedResult = sites.Skip(sites.Count - 1).Concat(sites.Take(sites.Count - 1)).ToList();
            var result = sut.Select(arc => arc.LeftNeighbour).ToList();

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [BeachlineData(3)]
        public void Insert_ingThreeSitesIntoAnEmptyBeachlineInDescendingOrderOfHeight_ShouldGiveOrderedArcs
            (Beachline sut, List<Site> anonymousSites)
        {
            // Fixture setup
            var anonymousSiteA = anonymousSites[0];
            var anonymousSiteB = anonymousSites[1];
            var anonymousSiteC = anonymousSites[2];

            anonymousSiteA = SiteAt(20, 157);
            anonymousSiteB = SiteAt(50, 168);
            anonymousSiteC = SiteAt(107, 219);

            Func<Arc, Arc, Arc, bool> inOrder =
                new CompareToCyclicOrdererAdapter<Arc>(Comparer<Arc>.Default.Compare).InOrder;

            // Exercise system
            sut.Insert(anonymousSiteA);
            sut.Insert(anonymousSiteB);
            sut.Insert(anonymousSiteC);

            //Debug.WriteLine(String.Join(", ", anonymousSites.Select(site => site.ToString())));
            Debug.WriteLine(sut);

            // Verify outcome
            var arcs = sut.ToList();
            for (int i = 1; i < arcs.Count - 1; i++)
            {
                Assert.True(inOrder(arcs[i - 1], arcs[i], arcs[i + 1]));
            }
            Assert.True(inOrder(arcs[arcs.Count - 2], arcs[arcs.Count - 1], arcs[0]));
            Assert.True(inOrder(arcs[arcs.Count - 1], arcs[0], arcs[1]));

            // Teardown
        }

        private Site SiteAt(float colatitude, float azimuth)
        {
            colatitude = colatitude*Mathf.PI/180;
            azimuth = azimuth*Mathf.PI/180;

            var x = Mathf.Sin(colatitude)*Mathf.Cos(azimuth);
            var y = Mathf.Sin(colatitude)*-Mathf.Sin(azimuth);
            var z = Mathf.Cos(colatitude);

            return new Site(new Vector3(x, y, z));
        }
    }
}
