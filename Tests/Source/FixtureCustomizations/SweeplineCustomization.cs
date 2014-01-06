using System;
using Generator;
using MathNet.Numerics;
using Ploeh.AutoFixture;

namespace SphericalVoronoiTests.FixtureCustomizations
{
    public class SweeplineCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var randomZ = (-1.0 + 2.0*new Random().NextDouble());
            var randomColatitude = Trig.InverseCosine(randomZ);

            var sweepline = new Sweepline {Colatitude = randomColatitude};

            fixture.Inject(sweepline);
        }
    }
}
