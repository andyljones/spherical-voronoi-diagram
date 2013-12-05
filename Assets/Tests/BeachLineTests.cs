﻿using System.Collections.Generic;
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
        public void TryRemove_WhenGivenAnArcInBeachline_ShouldRemoveArc()
        {
            var arc = new Arc(default(Vector3), default(Vector3));
            var beachline = new BeachLine {arc};

            beachline.TryRemove(arc);

            Assert.That(beachline.Count, Is.EqualTo(0));
        }

        [Test]
        public void TryRemove_WhenGivenAnArcInBeachline_ShouldReturnTrue()
        {
            var arc = new Arc(default(Vector3), default(Vector3));
            var beachline = new BeachLine { arc };

            Assert.That(beachline.TryRemove(arc), Is.True);
        }

        [Test]
        public void TryRemove_WhenGivenAnArcNotInBeachline_ShouldReturnFalse()
        {
            var firstArc = new Arc(default(Vector3), default(Vector3));
            var secondArc = new Arc(default(Vector3), default(Vector3));
            var beachline = new BeachLine { firstArc };

            Assert.That(beachline.TryRemove(secondArc), Is.False);
        }

        [Test]
        public void TryRemove_WhenGivenAnArcInBeachline_ShouldConnectNeighbours()
        {
            var firstArc = new Arc(new Vector3(1, 0, 0), new Vector3(0, -1, 0));
            var secondArc = new Arc(new Vector3(0, -1, 0), new Vector3(-1, 0, 0));
            var thirdArc = new Arc(new Vector3(-1, 0, 0), new Vector3(1, 0, 0));
            var beachline = new BeachLine { firstArc, secondArc, thirdArc };

            beachline.TryRemove(secondArc);

            Assert.That(firstArc.RightEndpoint, Is.EqualTo(thirdArc.LeftEndpoint));
        }
    }
}
