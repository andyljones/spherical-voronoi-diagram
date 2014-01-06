using Ploeh.AutoFixture.Xunit;
using SphericalVoronoiTests.FixtureCustomizations;

namespace SphericalVoronoiTests.DataAttributes
{
    public class VectorsAboveSweeplineAttribute : AutoDataAttribute
    {
        public VectorsAboveSweeplineAttribute()
        {
            Fixture.Customize(new SweeplineCustomization());
            Fixture.Customize(new VectorAboveSweeplineCustomization());
        }
    }
}
