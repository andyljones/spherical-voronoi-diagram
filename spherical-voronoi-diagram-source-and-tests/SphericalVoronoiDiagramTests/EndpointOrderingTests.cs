using System;
using System.Security.Policy;
using Ploeh.AutoFixture.Xunit;
using UnityEngine;
using Xunit;
using Xunit.Extensions;
using Debug = System.Diagnostics.Debug; 

namespace SphericalVoronoiDiagramTests
{
    public class EndpointOrderingTests
    {
        [Theory]
        [AutoData]
        public void Test()
        {
            // Fixture setup
            var siteA = new Site(new Vector3(1, 0, 1));
            var siteB = new Site(new Vector3(-1, 0, 2));

            Debug.WriteLine(180/Mathf.PI * EndpointOrdering.FindIntersection(siteA, siteB, 0));

            // Exercise system

            // Verify outcome
            Assert.True(false, "Test not implemented");

            // Teardown
        }
    }
}
