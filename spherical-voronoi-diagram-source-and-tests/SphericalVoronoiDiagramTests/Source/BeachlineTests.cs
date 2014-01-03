﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            (Beachline sut, List<SiteEvent> anonymousSites)
        {
            // Fixture setup
            var anonymousSite = anonymousSites.First();


            // Exercise system
            sut.Insert(anonymousSite);

            // Verify outcome
            var result = sut.Sweepline.Z;
            var expectedResult = anonymousSite.Position.z;

            var failureString =
                String.Format(
                "Sites were:\n{0}\n\nBeachline was {1}",
                String.Join(", ", anonymousSites.Select(site => site.ToString()).ToArray()),
                sut.ToString());

            Assert.True(expectedResult == result, failureString);

            // Teardown
        }

        [Theory]
        [BeachlineData(1)]
        public void Insert_ingASiteIntoAnEmptyBeachline_ShouldAddAnArcWithThatSiteAsItsSite
            (Beachline sut, List<SiteEvent> anonymousSites)
        {
            // Fixture setup
            var anonymousSite = anonymousSites.First();

            // Exercise system
            sut.Insert(anonymousSite);

            // Verify outcome
            var result = sut.First().SiteEvent;
            var expectedResult = anonymousSite;

            var failureString =
                String.Format(
                "Sites were:\n{0}\n\nBeachline was {1}",
                String.Join(", ", anonymousSites.Select(site => site.ToString()).ToArray()),
                sut.ToString());

            Assert.True(expectedResult == result, failureString);

            // Teardown
        }

        [Theory]
        [BeachlineData(1)]
        public void Insert_ingASiteIntoAnEmptyBeachline_ShouldAddAnArcNeighbouredByItself
            (Beachline sut, List<SiteEvent> anonymousSites)
        {
            // Fixture setup
            var anonymousSite = anonymousSites.First();

            // Exercise system
            sut.Insert(anonymousSite);

            // Verify outcome
            var result1 = sut.First().LeftNeighbour;
            var result2 = sut.First().RightNeighbour;
            var expectedResult = anonymousSite;

            var failureString =
                String.Format(
                "Sites were:\n{0}\n\nBeachline was {1}",
                String.Join(", ", anonymousSites.Select(site => site.ToString()).ToArray()),
                sut.ToString());

            Assert.True(expectedResult == result1, failureString);
            Assert.True(expectedResult == result2, failureString);

            // Teardown
        }

        [Theory]
        [BeachlineData(2)]
        public void
            Insert_ingTwoSitesIntoAnEmptyBeachlineWhenTheSecondIsLowerThanTheFirst_ShouldAddTwoArcsNeighbouredByEachother
            (Beachline sut, List<SiteEvent> anonymousSites)
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

            var failureString =
                String.Format(
                "Sites were:\n{0}\n\nBeachline was {1}",
                String.Join(", ", anonymousSites.Select(site => site.ToString()).ToArray()),
                sut.ToString());

            Assert.True(result2.SiteEvent == result1.LeftNeighbour, failureString);
            Assert.True(result2.SiteEvent == result1.RightNeighbour, failureString);
                                          
            Assert.True(result1.SiteEvent == result2.LeftNeighbour, failureString);
            Assert.True(result1.SiteEvent == result2.RightNeighbour, failureString);

            // Teardown
        }

        [Theory]
        [BeachlineData(3)]
        public void Insert_ingThreeSitesIntoAnEmptyBeachlineInDescendingOrderOfHeight_ShouldAddFourArcs
            (Beachline sut, List<SiteEvent> anonymousSites)
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

            var failureString =
                String.Format(
                "Sites were:\n{0}\n\nBeachline was {1}",
                String.Join(", ", anonymousSites.Select(site => site.ToString()).ToArray()),
                sut.ToString());

            Assert.True(expectedResult == result, failureString);

            // Teardown
        }

        [Theory]
        [BeachlineData(3)]
        public void
            Insert_ingThreeSitesIntoAnEmptyBeachlineInDescendingOrderOfHeight_ShouldGiveArcsWithEachRightNeighbourBeingTheNextArcsSite
            (Beachline sut, List<SiteEvent> anonymousSites)
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
            var sites = sut.Select(arc => arc.SiteEvent).ToList();
            var expectedResult = sites.Skip(1).Concat(sites.Take(1)).ToList();
            var result = sut.Select(arc => arc.RightNeighbour).ToList();

            var failureString =
                String.Format(
                "Sites were:\n{0}\n\nBeachline was {1}",
                String.Join(", ", anonymousSites.Select(site => site.ToString()).ToArray()),
                sut.ToString());

            Debug.WriteLine(failureString);
            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [BeachlineData(3)]
        public void
            Insert_ingThreeSitesIntoAnEmptyBeachlineInDescendingOrderOfHeight_ShouldGiveArcsWithEachLeftNeighbourBeingThePreviousArcsSite
            (Beachline sut, List<SiteEvent> anonymousSites)
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
            var sites = sut.Select(arc => arc.SiteEvent).ToList();
            var expectedResult = sites.Skip(sites.Count - 1).Concat(sites.Take(sites.Count - 1)).ToList();
            var result = sut.Select(arc => arc.LeftNeighbour).ToList();

            var failureString =
                String.Format(
                "Sites were:\n{0}\n\nBeachline was {1}",
                String.Join(", ", anonymousSites.Select(site => site.ToString()).ToArray()),
                sut.ToString());

            Debug.WriteLine(failureString);
            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [BeachlineData(3)]
        public void Insert_ingThreeSitesIntoAnEmptyBeachlineInDescendingOrderOfHeight_ShouldGiveOrderedArcs
            (Beachline sut, List<SiteEvent> anonymousSites)
        {
            // Fixture setup
            var anonymousSiteA = anonymousSites[0];
            var anonymousSiteB = anonymousSites[1];
            var anonymousSiteC = anonymousSites[2];

            // Exercise system
            sut.Insert(anonymousSiteA);
            sut.Insert(anonymousSiteB);
            sut.Insert(anonymousSiteC);

            Debug.WriteLine(String.Join(", ", anonymousSites.Select(site => site.ToString()).ToArray()));

            // Verify outcome
            var failureString =
                String.Format(
                "Sites were:\n{0}\n\nBeachline was {1}",
                String.Join(", ", anonymousSites.Select(site => site.ToString()).ToArray()),
                sut.ToString());

            var arcs = sut.ToList();
            for (int i = 1; i < arcs.Count - 1; i++)
            {
                Assert.True(ArcOrderer.AreInOrder(arcs[i - 1], arcs[i], arcs[i + 1]), failureString);
            }
            Assert.True(ArcOrderer.AreInOrder(arcs[arcs.Count - 2], arcs[arcs.Count - 1], arcs[0]), failureString);
            Assert.True(ArcOrderer.AreInOrder(arcs[arcs.Count - 1], arcs[0], arcs[1]), failureString);

            // Teardown
        }

        [Theory]
        [BeachlineData(3)]
        public void Insert_ingThreeSitesIntoAnEmptyBeachlineInDescendingOrderOfHeight_ShouldReturnTwoCircleEvents
            (Beachline sut, List<SiteEvent> anonymousSites)
        {
            // Fixture setup
            var anonymousSiteA = anonymousSites[0];
            var anonymousSiteB = anonymousSites[1];
            var anonymousSiteC = anonymousSites[2];

            // Exercise system
            sut.Insert(anonymousSiteA);
            sut.Insert(anonymousSiteB);
            var result = sut.Insert(anonymousSiteC).Count();

            // Verify outcome
            var expectedResult = 2;

            var failureString =
                String.Format(
                "Sites were:\n{0}\n\nBeachline was {1}",
                String.Join(", ", anonymousSites.Select(site => site.ToString()).ToArray()),
                sut.ToString());

            Assert.True(expectedResult == result, failureString);

            // Teardown
        }

        [Theory]
        [BeachlineData(3)]
        public void Insert_ingOneOrTwoSitesIntoAnEmptyBeachlineInDescendingOrderOfHeight_ShouldReturnNoCircleEvents
            (Beachline sut, List<SiteEvent> anonymousSites)
        {
            // Fixture setup
            var anonymousSiteA = anonymousSites[0];
            var anonymousSiteB = anonymousSites[1];

            // Exercise system
            var result = sut.Insert(anonymousSiteB).Count() + sut.Insert(anonymousSiteA).Count();

            // Verify outcome
            var expectedResult = 0;

            var failureString =
                String.Format(
                "Sites were:\n{0}\n\nBeachline was {1}",
                String.Join(", ", anonymousSites.Select(site => site.ToString()).ToArray()),
                sut.ToString());

            Assert.True(expectedResult == result, failureString);

            // Teardown
        }

        [Fact]
        public void Test()
        {
            // Fixture setup
            var positions = new List<Vector3>
        {
            MathUtils.CreateVectorAt(0, 0),
            MathUtils.CreateVectorAt(10, -45),
            MathUtils.CreateVectorAt(10, 45),
            MathUtils.CreateVectorAt(20, 5)
        };
            var _diagram = new VoronoiDiagram(positions);
            _diagram.ProcessNextEvent();
            _diagram.ProcessNextEvent();
            _diagram.ProcessNextEvent();
            _diagram.ProcessNextEvent();
            _diagram.ProcessNextEvent();

            // Exercise system

            // Verify outcome
            Assert.True(false, "Test not implemented");

            // Teardown
        }
    }
}
