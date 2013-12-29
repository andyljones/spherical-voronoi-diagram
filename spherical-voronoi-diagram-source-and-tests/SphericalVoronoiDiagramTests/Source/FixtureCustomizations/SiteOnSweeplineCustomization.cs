using Ploeh.AutoFixture;
using UnityEngine;
using Random = System.Random;

namespace SphericalVoronoiDiagramTests.FixtureCustomizations
{
    public class SiteOnSweeplineCustomization : ICustomization
    {
        private readonly Random _random;

        public SiteOnSweeplineCustomization()
        {
            _random = new Random();
        }

        public void Customize(IFixture fixture)
        {
            fixture.Register(() => CreateSiteOnSweepline(fixture));
        }

        private Site CreateSiteOnSweepline(IFixture fixture)
        {
            var sweepline = fixture.Create<Sweepline>();
            var z = sweepline.Z;

            var azimuth = (float)(2 * Mathf.PI * _random.NextDouble());
            var x = Mathf.Sqrt(1 - z * z) * Mathf.Cos(azimuth);
            var y = -Mathf.Sqrt(1 - z * z) * Mathf.Sin(azimuth);

            return new Site(new Vector3(x, y, z));
        }
    }
}
