using Events;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    [TestFixture]
    public class CircleEventTests
    {
        [Test]
        public void Position_ShouldBeLowestPointOnCircumcircle()
        {
            var arc = new Arc();
            var circumcenter = new Vector3(1, -1, 0);
            var radius = Mathf.PI/4;
            var circle = new CircleEvent(arc, circumcenter, radius);

            var expectedPosition = new Vector3(1, -1, Mathf.Sqrt(2)).normalized;
            var tolerance = 0.001f;

            Assert.That(Vector3.Dot(circle.Position, expectedPosition) - 1, Is.LessThanOrEqualTo(tolerance));
        }
    }
}
