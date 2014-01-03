using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;

namespace SphericalVoronoiDiagramTests.DataAttributes
{
    public class NSubstituteDataAttribute : AutoDataAttribute
    {
        public NSubstituteDataAttribute()
        {
            Fixture.Customize(new AutoNSubstituteCustomization());
        }
    }
}
