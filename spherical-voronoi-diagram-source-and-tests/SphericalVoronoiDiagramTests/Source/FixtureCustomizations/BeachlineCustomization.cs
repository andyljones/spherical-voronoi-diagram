using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;

namespace SphericalVoronoiDiagramTests.FixtureCustomizations
{
    public class BeachlineCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var sweepline = fixture.Create<Sweepline>();
            var sites = fixture.CreateMany<Site>().ToList();
            var intersections = new List<Intersection>();
            for (int i = 0; i < sites.Count - 1; i++)
            {
                intersections.Add(new Intersection(sites[i], sites[i + 1], sweepline));
            }
            intersections.Add(new Intersection(sites[sites.Count - 1], sites[0], sweepline));

            var beachline = new Beachline(intersections);

            fixture.Inject(beachline);
        }
    }
}
