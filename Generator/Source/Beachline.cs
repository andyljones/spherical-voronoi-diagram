using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CyclicalSkipList;

namespace Generator
{
    public class Beachline : IEnumerable<IArc>
    {
        private readonly Skiplist<IArc> _arcs;
        private int Count;

        public Beachline()
        {
            var sweepline = new Sweepline {Z = 1};
            var orderer = new ArcOrderer(sweepline);
            _arcs = new Skiplist<IArc> {InOrder = orderer.AreInOrder};
        }

        public void Insert(SiteEvent site)
        {
            if (Count == 0)
            {
                var newArc = new Arc { Site = site, LeftNeighbour = site };

                _arcs.Insert(newArc);
                Count++;
            }
            if (Count == 1)
            {
                var oldArc = _arcs.First();
                var newArc = new Arc { Site = site, LeftNeighbour = oldArc.Site};
                oldArc.LeftNeighbour = site;

                _arcs.Insert(newArc);
                Count++;
            }
        }

        public IEnumerator<IArc> GetEnumerator()
        {
            return _arcs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
