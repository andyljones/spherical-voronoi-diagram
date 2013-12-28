using System.Linq;
using Ploeh.AutoFixture;

namespace SphericalVoronoiDiagramTests.FixtureCustomizations
{
    public class SitesOrderedByZCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var sites = fixture.CreateMany<Site>();
            var orderedSites = sites.OrderByDescending(site => site.Position.z).ToList();
            fixture.Inject(orderedSites);
        }
    }
}
