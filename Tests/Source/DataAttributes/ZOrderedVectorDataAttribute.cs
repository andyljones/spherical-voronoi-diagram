using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;
using SphericalVoronoiTests.FixtureCustomizations;

namespace SphericalVoronoiTests.DataAttributes
{
    public class ZOrderedVectorDataAttribute : AutoDataAttribute
    {
        public ZOrderedVectorDataAttribute()
        {
            Fixture.Customize(new ZOrderedVectorCustomization());
            Fixture.Customize(new AutoNSubstituteCustomization());
        }
    }
}
