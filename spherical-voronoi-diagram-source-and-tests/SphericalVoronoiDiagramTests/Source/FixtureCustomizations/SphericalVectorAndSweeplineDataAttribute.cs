using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace SphericalVoronoiDiagramTests.FixtureCustomizations
{
    public class SphericalVectorAndSweeplineDataAttribute : AutoDataAttribute
    {
        public SphericalVectorAndSweeplineDataAttribute()
        {
            Fixture.Customize(new SweeplineCustomization());
            Fixture.Customizations.Add(new SphericalVectorSpecimenBuilder(Fixture.Create<Sweepline>()));
        }
    }
}
