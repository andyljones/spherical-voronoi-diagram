using System;
using System.Collections.Generic;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using SphericalVoronoiDiagramTests.FixtureCustomizations;
using Xunit.Extensions;

namespace SphericalVoronoiDiagramTests.DataAttributes
{
    public class BeachlineDataAttribute : AutoDataAttribute
    {
        public BeachlineDataAttribute(int count)
        {
            Fixture.RepeatCount = count;
            Fixture.Customize(new SweeplineCustomization());
            Fixture.Customize(new VectorAboveSweeplineCustomization());
            Fixture.Customize(new SitesOrderedByZCustomization());
        }
    }
}
