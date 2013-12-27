using System;
using Ploeh.AutoFixture;

namespace SphericalVoronoiDiagramTests.FixtureCustomizations
{
    public class SweeplineCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var sweeplinePosition = (float)(-1+2* new Random().NextDouble());
            fixture.Inject(new Sweepline(sweeplinePosition));
        }
    }
}
