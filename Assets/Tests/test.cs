using NUnit.Framework;
using Simulator.ShallowFluid;
using SphericalVoronoiDiagram;
using UnityEngine;

namespace Tests
{
    [TestFixture]
    class test
    {
        //[Test]
        public void Run()
        {
            var field = new VectorField<int> {{0, new Vector3(0, .1f, 1)}, {1, new Vector3(1, 0, 0)}, {2, new Vector3(0, -1, 0)}};

            var gen = new VoronoiGenerator<int>(field);
        }
    }
}
