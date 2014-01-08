using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CyclicalSkipList;

namespace Generator
{
    public class Beachline : IEnumerable<Arc>
    {
        private readonly Skiplist<Arc> _arcs;
        private int Count;

        public Beachline()
        {
            var sweepline = new Sweepline {Colatitude = 0};
            var orderer = new ArcOrderer(sweepline);
            _arcs = new Skiplist<Arc> {InOrder = orderer.AreInOrder};
        }

        public void Insert(SiteEvent site)
        {
            if (Count == 0)
            {
                var newArc = new Arc(site) { LeftNeighbour = site };

                _arcs.Insert(newArc);       
            }
            if (Count == 1)
            {
                var oldArc = _arcs.First();
                var newArc = new Arc(site) { LeftNeighbour = oldArc.Site};
                oldArc.LeftNeighbour = site;

                _arcs.Insert(newArc);
            }

        }

        public IEnumerator<Arc> GetEnumerator()
        {
            return _arcs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
