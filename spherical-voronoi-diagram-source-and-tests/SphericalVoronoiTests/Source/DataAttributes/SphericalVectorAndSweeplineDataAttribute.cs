using Ploeh.AutoFixture.Xunit;
using SphericalVoronoiDiagramTests.FixtureCustomizations;

namespace SphericalVoronoiDiagramTests.DataAttributes
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
