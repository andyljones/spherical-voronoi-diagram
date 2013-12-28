using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace SphericalVoronoiDiagramTests.FixtureCustomizations
{
    public class BeachlineDataAttribute : AutoDataAttribute
    {
        public BeachlineDataAttribute(int count)
        {
            Fixture.RepeatCount = count;
            Fixture.Customize(new SweeplineCustomization());
            Fixture.Customize(new SphericalVectorCustomization());
            Fixture.Customize(new BeachlineCustomization());
        }
    }
}
