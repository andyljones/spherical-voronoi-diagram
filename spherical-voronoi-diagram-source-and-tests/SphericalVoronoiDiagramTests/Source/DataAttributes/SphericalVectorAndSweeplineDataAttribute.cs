using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace SphericalVoronoiDiagramTests.FixtureCustomizations
{
    public class SphericalVectorAndSweeplineDataAttribute : AutoDataAttribute
    {
        public SphericalVectorAndSweeplineDataAttribute()
        {
            Fixture.Customize(new SweeplineCustomization());
            Fixture.Customize(new VectorAboveSweeplineCustomization());
        }
    }
}
