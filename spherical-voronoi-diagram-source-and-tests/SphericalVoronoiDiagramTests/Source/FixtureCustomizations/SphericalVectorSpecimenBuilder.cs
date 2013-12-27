using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using UnityEngine;

namespace SphericalVoronoiDiagramTests.FixtureCustomizations
{
    public class SphericalVectorSpecimenBuilder : ISpecimenBuilder
    {
        private readonly System.Random _random;
        private readonly Sweepline _sweepline;

        public SphericalVectorSpecimenBuilder(Sweepline sweepline)
        {
            _random = new System.Random();
            _sweepline = sweepline;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var typedRequest = request as Type;
            if (typedRequest != typeof(Vector3))
            {
                return new NoSpecimen(request);
            }

            var z = (float)(_sweepline.Height + (1-_sweepline.Height)*_random.NextDouble());
            var longitude = (float) (2*Mathf.PI * _random.NextDouble());

            var x = Mathf.Sqrt(1 - z*z)*Mathf.Cos(longitude);
            var y = -Mathf.Sqrt(1 - z*z)*Mathf.Sin(longitude);

            return new Vector3(x, y, z);
        }
    }
}
