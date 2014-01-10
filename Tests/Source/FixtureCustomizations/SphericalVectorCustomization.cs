using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generator;
using MathNet.Numerics;
using Ploeh.AutoFixture;

namespace SphericalVoronoiTests.Source.FixtureCustomizations
{
    public class SphericalVectorCustomization : ICustomization
    {
        private readonly Random _random = new Random();

        public void Customize(IFixture fixture)
        {
            fixture.Register(() => CreateVectorAboveSweepline(fixture));
        }

        public Vector3 CreateVectorAboveSweepline(IFixture fixture)
        {

            var randomZ = (-1 + 2*_random.NextDouble());
            var randomAzimuth = Constants.Pi*_random.NextDouble();

            var x = Math.Sqrt(1.0 - randomZ*randomZ)*Trig.Cosine(randomAzimuth);
            var y = Math.Sqrt(1.0 - randomZ*randomZ)*Trig.Sine(randomAzimuth);
            var z = randomZ;

            return new Vector3(x, y, z);
        }
    }
}
