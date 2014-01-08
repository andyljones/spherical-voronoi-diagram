using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CyclicalSkipList;

namespace Generator
{
    public class Beachline : IEnumerable<IArc>
    {
        private readonly Skiplist<IArc> _arcs;
        private readonly Sweepline _sweepline;
        private int _count;

        public Beachline()
        {
            _sweepline = new Sweepline {Z = 1};
            var orderer = new ArcOrderer(_sweepline);
            _arcs = new Skiplist<IArc> {InOrder = orderer.AreInOrder};
            _count = 0;
        }

        public void Insert(SiteEvent site)
        {
            if (_count == 0)
            {
                InsertFirstSite(site);
            }
            else if (_count == 1)
            {
                InsertSecondSite(site);
            }
            else
            {
                InsertSite(site);
            }
        }

        private void InsertFirstSite(SiteEvent site)
        {
            var newArc = new Arc { Site = site, LeftNeighbour = site };

            _arcs.Insert(newArc);
            _count++;
        }

        private void InsertSecondSite(SiteEvent site)
        {
            var oldArc = _arcs.First();
            var newArc = new Arc { Site = site, LeftNeighbour = oldArc.Site };
            oldArc.LeftNeighbour = site;

            _arcs.Insert(newArc);
            _count++;
        }

        private void InsertSite(SiteEvent site)
        {
            var newArcA = new Arc {Site = site, LeftNeighbour = site};
            var newArcB = new Arc {Site = site, LeftNeighbour = site};

            _arcs.Insert(newArcA);
            _arcs.Insert(newArcB);

        }

        public IEnumerator<IArc> GetEnumerator()
        {
            return _arcs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            var arcStrings =_arcs.Select(arc => StringOfArc(arc)).ToArray();

            return String.Join("", arcStrings);
        }

        private string StringOfArc(IArc arc)
        {
            var azimuthOfIntersection = arc.LeftIntersection(_sweepline).SphericalCoordinates().Azimuth;
            var leftIntersectionString = String.Format("{0,3:N0}", azimuthOfIntersection);
            var arcString = arc.ToString();

            return leftIntersectionString + arcString;
        }
    }
}
