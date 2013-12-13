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
            var firstArc =  new Arc { Site = new SiteEvent(new Vector3(1, 0, 0))};
            var secondArc = new Arc { Site = new SiteEvent(new Vector3(1, -1, -1)) };
            var thirdArc = new Arc { Site = new SiteEvent(new Vector3(1, 1, 1)) };

            var beachline = new BeachLine {thirdArc, firstArc, secondArc};

            var storedOrder = beachline.ToList();

            Assert.That(storedOrder[0], Is.EqualTo(firstArc));
            Assert.That(storedOrder[1], Is.EqualTo(secondArc));
            Assert.That(storedOrder[2], Is.EqualTo(thirdArc));
        }

        [Test]
        public void TryRemove_WhenGivenAnArcInBeachline_ShouldRemoveArc()
        {
            var arc = new Arc();
            var beachline = new BeachLine {arc};

            beachline.TryRemove(arc);

            Assert.That(beachline.Count, Is.EqualTo(0));
        }

        [Test]
        public void TryRemove_WhenGivenAnArcInBeachline_ShouldReturnTrue()
        {
            var arc = new Arc();
            var beachline = new BeachLine { arc };

            Assert.That(beachline.TryRemove(arc), Is.True);
        }

        [Test]
        public void TryRemove_WhenGivenAnArcNotInBeachline_ShouldReturnFalse()
        {
            var firstArc = new Arc();
            var secondArc = new Arc();
            var beachline = new BeachLine { firstArc };

            Assert.That(beachline.TryRemove(secondArc), Is.False);
        }

        [Test]
        public void TryRemove_WhenGivenAnArcInBeachline_ShouldConnectNeighbours()
        {
            var firstArc = new Arc();
            var secondArc = new Arc();
            var thirdArc = new Arc();
            var beachline = new BeachLine { firstArc, secondArc, thirdArc };

            beachline.TryRemove(secondArc);

            Assert.That(firstArc.RightArc, Is.EqualTo(thirdArc));
            Assert.That(firstArc.LeftArc, Is.EqualTo(firstArc));
        }
    }
}
