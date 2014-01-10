using Generator;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using SphericalVoronoiTests.FixtureCustomizations;
using SphericalVoronoiTests.Source.FixtureCustomizations;

namespace SphericalVoronoiTests.DataAttributes
{
    public class CircleEventData : AutoDataAttribute
    {
        public CircleEventData()
        {
            Fixture.Customize(new SphericalVectorCustomization());
            Fixture.Register(() => CreateCircleEvent(Fixture));
        }

        private CircleEvent CreateCircleEvent(IFixture fixture)
        {
            var arcA = fixture.Create<Arc>();
            var arcB = fixture.Create<Arc>();
            var arcC = fixture.Create<Arc>();

            return new CircleEvent(arcA, arcB, arcC);
        }
    }
}
