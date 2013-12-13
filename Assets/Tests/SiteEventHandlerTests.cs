using System.Linq;
using Events;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    [TestFixture]
    public class SiteEventHandlerTests
    {
        [Test]
        public void CreateArcAt_WhenSetIsEmpty_ShouldAddOneArcWithCorrectSite()
        {
            var site = new SiteEvent();
            var beachline = new BeachLine();
            var handler = new SiteEventHandler(beachline, new EventQueue());

            handler.Handle(site);

            Assert.That(beachline.Single().Site, Is.EqualTo(site));
        }

        [Test]
        public void CreateArcAt_WhenSetContainsOneArc_ShouldConnectArcsToEachother()
        {
            var arc = new Arc();
            var site = new SiteEvent(new Vector3(-1, 0, 0));
            var beachline = new BeachLine { arc };
            var handler = new SiteEventHandler(beachline, new EventQueue());

            handler.Handle(site);

            Assert.That(beachline.First().LeftArc, Is.EqualTo(beachline.Last().RightArc));
            Assert.That(beachline.First().RightArc, Is.EqualTo(beachline.Last().LeftArc));
        }

        [Test]
        public void CreateArcAt_WhenGivenASiteIntersectingOneArc_ShouldSplitOldArc()
        {
            var firstSite = new SiteEvent();
            var firstArc = new Arc
            {
                Site = firstSite
            };
            var secondSite = new SiteEvent();
            var secondArc = new Arc
            {
                Site = secondSite
            };
            var newSite = new SiteEvent(new Vector3(1, 0, 0));
            var beachline = new BeachLine { firstArc, secondArc };
            var handler = new SiteEventHandler(beachline, new EventQueue());

            handler.Handle(newSite);
            var producedSites = beachline.Select(arc => arc.Site).ToList();

            Assert.That(producedSites.Count(site => site == firstSite), Is.EqualTo(2));
            Assert.That(producedSites.Count(site => site == secondSite), Is.EqualTo(1));
            Assert.That(producedSites.Count(site => site == newSite), Is.EqualTo(1));

        }

        [Test]
        public void CreateArcAt_WhenGivenASiteIntersectingTwoArcs_ShouldAddNewArcBetweenOldArcs()
        {
            var firstSite = new SiteEvent();
            var firstArc = new Arc
            {
                Site = firstSite
            };
            var secondSite = new SiteEvent();
            var secondArc = new Arc
            {
                Site = secondSite
            };
            var newSite = new SiteEvent(new Vector3(1, 0, 0));
            var beachline = new BeachLine { firstArc, secondArc };
            var handler = new SiteEventHandler(beachline, new EventQueue());

            handler.Handle(newSite);
            var sitesOfArcs = beachline.Select(arc => arc.Site).ToList();

            Assert.That(sitesOfArcs.Count(site => site == firstSite), Is.EqualTo(1));
            Assert.That(sitesOfArcs.Count(site => site == secondSite), Is.EqualTo(1));
            Assert.That(sitesOfArcs.Count(site => site == newSite), Is.EqualTo(1));
        }
    }
}
