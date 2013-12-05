using System.Collections.Generic;
using System.Linq;
using Events;
using FakeItEasy;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    [TestFixture]
    public class BeachLineTests
    {
        [Test]
        public void BeachLine_ShouldStoreArcsInAzimuthalOrder()
        {
            var endpoints = new List<Endpoint>
            {
                new Endpoint(0, -1, 0),
                new Endpoint(-1, 0, 0),
                new Endpoint(1, 1, 0),
            };

            var firstArc = new Arc(endpoints[0], endpoints[1]);
            var secondArc = new Arc(endpoints[1], endpoints[2]);
            var thirdArc = new Arc(endpoints[2], endpoints[0]);

            var beachline = new BeachLine {thirdArc, firstArc, secondArc};

            var storedOrder = beachline.ToList();

            Assert.That(storedOrder[0], Is.EqualTo(firstArc));
            Assert.That(storedOrder[1], Is.EqualTo(secondArc));
            Assert.That(storedOrder[2], Is.EqualTo(thirdArc));
        }

        [Test]
        public void Insert_WhenSetIsEmpty_ShouldAddOneArcWithCorrectSite()
        {
            var site = new SiteEvent();
            var beachline = new BeachLine();

            beachline.Insert(site);

            Assert.That(beachline.Single().Site, Is.EqualTo(site));
        }

        [Test]
        public void Insert_WhenSetContainsOneArc_ShouldConnectArcsToEachother()
        {
            var arc = new Arc(new Vector3(1, 0, 0), new Vector3(1, 0, 0));
            var site = new SiteEvent(new Vector3(-1, 0, 0));
            var beachline = new BeachLine {arc};
            
            beachline.Insert(site);

            Assert.That(beachline.First().LeftEndpoint, Is.EqualTo(beachline.Last().RightEndpoint));
            Assert.That(beachline.First().RightEndpoint, Is.EqualTo(beachline.Last().LeftEndpoint));
        }

        [Test]
        public void Insert_WhenGivenASiteIntersectingOneArc_ShouldSplitOldArc()
        {
            
        }

        [Test]
        public void Insert_WhenGivenASiteIntersectingTwoArcs_ShouldAddNewArcBetweenOldArcs()
        {
            
        }
    }
}
