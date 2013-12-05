using System;
using System.Linq;
using Events;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    [TestFixture]
    public class ArcTests
    {
        [Test]
        public void Constructor_GivenASingleSite_ShouldCreateAnArcWithBothEndpointsEqualToTheSitesPosition()
        {
            var position = new Vector3(1, 0, 0);
            var site = new SiteEvent(position);
            var arc = new Arc(site);

            Assert.That(arc.LeftEndpoint.Position, Is.EqualTo(position));
            Assert.That(arc.RightEndpoint.Position, Is.EqualTo(position));
        }

        [Test]
        public void Contains_GivenTwoNestedIntervalsNotStraddlingPi_ShouldReturnTrue()
        {
            var arcA = new Arc(new Vector3(1, 0, 0), new Vector3(0, -1, 0));
            var arcB = new Arc(new Vector3(1, -1, 0), new Vector3(1, -1, 0));

            Assert.That(arcA.Contains(arcB), Is.True);
        }

        [Test]
        public void Contains_GivenTwoIntersectingNonNestedIntervalsNotStraddlingPi_ShouldReturnFalse()
        {
            var arcA = new Arc(new Vector3(1, 0, 0), new Vector3(0, -1, 0));
            var arcB = new Arc(new Vector3(1, -1, 0), new Vector3(-1, -1, 0));

            Assert.That(arcA.Contains(arcB), Is.False);
        }

        [Test]
        public void Contains_GivenTwoNonIntersectingIntervalsNotStraddlingPi_ShouldReturnFalse()
        {
            var arcA = new Arc(new Vector3(1, 0, 0), new Vector3(0, -1, 0));
            var arcB = new Arc(new Vector3(-1, 0, 0), new Vector3(-1, -1, 0));

            Assert.That(arcA.Contains(arcB), Is.False);
        }

        [Test]
        public void Contains_GivenTwoNestedIntervalsBothStraddlingPi_ShouldReturnTrue()
        {
            var arcA = new Arc(new Vector3(0, -1, 0), new Vector3(0, 1, 0));
            var arcB = new Arc(new Vector3(-1, -1, 0), new Vector3(-1, 1, 0));

            Assert.That(arcA.Contains(arcB), Is.True);
        }

        [Test]
        public void Contains_GivenTwoIntersectingNonNestedIntervalsBothStraddlingPi_ShouldReturnFalse()
        {
            var arcA = new Arc(new Vector3(0, -1, 0), new Vector3(0, 1, 0));
            var arcB = new Arc(new Vector3(0, -1, 0), new Vector3(1, 1, 0));

            Assert.That(arcA.Contains(arcB), Is.False);
        }


        [Test]
        public void Contains_IfContainerStraddlesPiButContainedDoesntAndTheyreNested_ShouldReturnTrue()
        {
            var arcA = new Arc(new Vector3(0, -1, 0), new Vector3(0, 1, 0));
            var arcB = new Arc(new Vector3(-1, 0, 0), new Vector3(-1, 0, 0));

            Assert.That(arcA.Contains(arcB), Is.True);
        }

        [Test]
        public void Contains_IfContainerStraddlesPiButContainedDoesntAndTheyreIntersectingButNotNested_ShouldReturnFalse()
        {
            var arcA = new Arc(new Vector3(0, -1, 0), new Vector3(0, 1, 0));
            var arcB = new Arc(new Vector3(1, 0, 0), new Vector3(-1, 0, 0));

            Assert.That(arcA.Contains(arcB), Is.False);
        }

        [Test]
        public void Contains_IfContainerStraddlesPiButContainedDoesntAndTheyreNonIntersecting_ShouldReturnFalse()
        {
            var arcA = new Arc(new Vector3(0, -1, 0), new Vector3(0, 1, 0));
            var arcB = new Arc(new Vector3(1, 0, 0), new Vector3(1, 0, 0));

            Assert.That(arcA.Contains(arcB), Is.False);
        }

        [Test]
        public void SplitAt_GivenANonContainedInterval_ShouldThrowAnError()
        {
            var site = new SiteEvent();
            var arcA = new Arc(new Vector3(0, 1, 0), new Vector3(0, -1, 0), site);
            var arcB = new Arc(new Vector3(-1, 0, 0), new Vector3(-1, 0, 0));

            Assert.That(() => arcA.SplitAt(arcB), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void SplitAt_GivenAContainedInterval_ShouldReturnArcsWithTheSameSite()
        {
            var site = new SiteEvent();
            var arcA = new Arc(new Vector3(0, 1, 0), new Vector3(0, -1, 0), site);
            var arcB = new Arc(new Vector3(1, 0, 0), new Vector3(1, 0, 0));

            var results = arcA.SplitAt(arcB);

            Assert.That(results.Select(result => result.Site), Has.All.EqualTo(site));
        }

        [Test]
        public void SplitAt_GivenANestedInterval_ShouldReturnArcsWithCorrectEndpoints()
        {
            var site = new SiteEvent();
            var arcA = new Arc(new Vector3(0, 1, 0), new Vector3(0, -1, 0), site);
            var arcB = new Arc(new Vector3(1, 1, 0), new Vector3(1, -1, 0));

            var results = arcA.SplitAt(arcB);

            Assert.That(results[0].LeftEndpoint, Is.EqualTo(arcA.LeftEndpoint));
            Assert.That(results[0].RightEndpoint, Is.EqualTo(arcB.LeftEndpoint));
            Assert.That(results[1].LeftEndpoint, Is.EqualTo(arcB.RightEndpoint));
            Assert.That(results[1].RightEndpoint, Is.EqualTo(arcA.RightEndpoint));
        }

        [Test]
        public void ConnectToLeftOf_GivenTwoArcs_ShouldSetTheFirstsRightEndpointEqualToTheSecondsLeftEndpoint()
        {
            var arcA = new Arc(default(Vector3), new Vector3(0, -1, 0));
            var arcB = new Arc(new Vector3(1, 1, 0), default(Vector3));

            arcA.ConnectToLeftOf(arcB);

            Assert.That(arcA.RightEndpoint, Is.EqualTo(arcB.LeftEndpoint));
        }
    }
}
