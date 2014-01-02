using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using UnityEngine;
using Random = System.Random;

namespace SphericalVoronoiDiagramTests.FixtureCustomizations
{
    public class VectorAboveSweeplineCustomization : ICustomization
    {
        private readonly Random _random;

        private const float MinimalSweeplineToVectorDistance = 0.01f;

        public VectorAboveSweeplineCustomization()
        {
            _random = new Random();
        }

        private Vector3 CreateSphericalVector(Sweepline sweepline)
        {
            var minimalZ = sweepline.Z + MinimalSweeplineToVectorDistance;
            var z = (float)(minimalZ + (1 - minimalZ)*_random.NextDouble());
            var azimuth = (float) (2*Mathf.PI * _random.NextDouble());

            var x = Mathf.Sqrt(1 - z*z)*Mathf.Cos(azimuth);
            var y = -Mathf.Sqrt(1 - z*z)*Mathf.Sin(azimuth);

            return new Vector3(x, y, z);
        }

        public void Customize(IFixture fixture)
        {
            fixture.Register(() => CreateSphericalVector(fixture.Create<Sweepline>()));
        }
    }
}
