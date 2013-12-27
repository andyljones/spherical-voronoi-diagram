using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using UnityEngine;

namespace SphericalVoronoiDiagramTests.FixtureCustomizations
{
    public class SphericalVectorSpecimenBuilder : ISpecimenBuilder
    {
        private readonly System.Random _random;

        public SphericalVectorSpecimenBuilder()
        {
            _random = new System.Random();
        }

        public object Create(object request, ISpecimenContext context)
        {
            var typedRequest = request as Type;
            if (typedRequest != typeof(Vector3))
            {
                return new NoSpecimen(request);
            }

            var z = (float)(-1.0f + 2*_random.NextDouble());
            var longitude = (float) (2*Mathf.PI * _random.NextDouble());

            var x = Mathf.Sqrt(1 - z*z)*Mathf.Cos(longitude);
            var y = -Mathf.Sqrt(1 - z*z)*Mathf.Sin(longitude);

            return new Vector3(x, y, z);
        }
    }
}
