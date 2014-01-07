using System;
using Generator;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Ploeh.AutoFixture;

namespace SphericalVoronoiTests.FixtureCustomizations
{
    public class Vector3AboveSweeplineCustomization : ICustomization
    {
        private readonly Random _random = new Random();

        public void Customize(IFixture fixture)
        {
            fixture.Register(() => CreateVectorAboveSweepline(fixture));
        }

        public Vector3 CreateVectorAboveSweepline(IFixture fixture)
        {
            var sweepline = fixture.Create<Sweepline>();
            var sweeplineZ = Trig.Cosine(sweepline.Colatitude);

            var randomZ = (sweeplineZ + (1 - sweeplineZ)*_random.NextDouble());
            var randomAzimuth = Constants.Pi*_random.NextDouble();

            var x = Math.Sqrt(1.0 - randomZ*randomZ)*Trig.Cosine(randomAzimuth);
            var y = Math.Sqrt(1.0 - randomZ*randomZ)*Trig.Sine(randomAzimuth);
            var z = randomZ;

            return new Vector3(x, y, z);
        }
    }
}
