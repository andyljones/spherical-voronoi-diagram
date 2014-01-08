using System;
using Generator;
using MathNet.Numerics;
using Ploeh.AutoFixture;

namespace SphericalVoronoiTests.FixtureCustomizations
{
    public class ZOrderedVectorCustomization : ICustomization
    {
        private double _maxZ = 1.0;
        private readonly Random _random = new Random();

        public void Customize(IFixture fixture)
        {
            fixture.Register(CreateVectorBelowLast);
        }

        private Vector3 CreateVectorBelowLast()
        {
            var z = -1.0 + (1 + _maxZ)*_random.NextDouble();
            _maxZ = z;
           
            var azimuth = 2*Constants.Pi*_random.NextDouble();

            var x = Math.Sqrt(1 - z*z)*Trig.Cosine(azimuth);
            var y = Math.Sqrt(1 - z*z)*Trig.Sine(azimuth);

            return new Vector3(x, y, z);
        }
    }
}
