using Events;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax;
using FakeItEasy.ExtensionSyntax.Full;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    [TestFixture]
    public class EventComparerTests
    {
        [Test]
        public void Compare_IfLHSIsSameDirectionAsRHS_ShouldReturn0()
        {
            var northEvent = A.Fake<IEvent>();
            A.CallTo(() => northEvent.Position).Returns(new Vector3(5, 0, 0));
            var equatorEvent = A.Fake<IEvent>();
            A.CallTo(() => equatorEvent.Position).Returns(new Vector3(1, 0, 0));

            var comparer = new EventComparer();
            var result = comparer.Compare(northEvent, equatorEvent);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Compare_IfLHSIsNorthOfRHS_ShouldReturn1()
        {
            var northEvent = A.Fake<IEvent>();
            A.CallTo(() => northEvent.Position).Returns(new Vector3(0, 0, 1));
            var equatorEvent = A.Fake<IEvent>();
            A.CallTo(() => equatorEvent.Position).Returns(new Vector3(5, 0, -1));

            var comparer = new EventComparer();
            var result = comparer.Compare(northEvent, equatorEvent);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void Compare_IfLHSIsSouthOfRHS_ShouldReturnMinus1()
        {
            var northEvent = A.Fake<IEvent>();
            A.CallTo(() => northEvent.Position).Returns(new Vector3(0, 0, -1));
            var equatorEvent = A.Fake<IEvent>();
            A.CallTo(() => equatorEvent.Position).Returns(new Vector3(5, 0, -1));

            var comparer = new EventComparer();
            var result = comparer.Compare(northEvent, equatorEvent);

            Assert.That(result, Is.EqualTo(-1));
        }


        [Test]
        public void Compare_IfLHSIsAtSameLatitudeOfRHSButFurtherEast_ShouldReturn1()
        {
            var northEvent = A.Fake<IEvent>();
            A.CallTo(() => northEvent.Position).Returns(new Vector3(1, 0, 0));
            var equatorEvent = A.Fake<IEvent>();
            A.CallTo(() => equatorEvent.Position).Returns(new Vector3(5, 5, 0));

            var comparer = new EventComparer();
            var result = comparer.Compare(northEvent, equatorEvent);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void Compare_IfLHSIsAtSameLatitudeOfRHSButFurtherWest_ShouldReturn1()
        {
            var northEvent = A.Fake<IEvent>();
            A.CallTo(() => northEvent.Position).Returns(new Vector3(1, 1, 0));
            var equatorEvent = A.Fake<IEvent>();
            A.CallTo(() => equatorEvent.Position).Returns(new Vector3(5, 0, 0));

            var comparer = new EventComparer();
            var result = comparer.Compare(northEvent, equatorEvent);

            Assert.That(result, Is.EqualTo(-1));
        }


    }
}
