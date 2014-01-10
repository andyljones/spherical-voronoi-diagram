using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CyclicalSkipList;
using MathNet.Numerics;

namespace Generator
{
    public class Beachline : IEnumerable<IArc>
    {
        public readonly Sweepline Sweepline;        
        public List<CircleEvent> PotentialCircleEvents { get; private set; }

        private readonly Skiplist<IArc> _arcs;
        private int _count;

        public Beachline()
        {
            Sweepline = new Sweepline {Z = 1};
            
            var orderer = new ArcOrderer(Sweepline);
            _arcs = new Skiplist<IArc> {InOrder = orderer.AreInOrder};
            _count = 0;

            PotentialCircleEvents = new List<CircleEvent>();
        }

        #region Insert methods
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

            InsertIntoSkiplist(newArc);
        }

        private void InsertSecondSite(SiteEvent site)
        {
            var oldArc = _arcs.First();
            var newArc = new Arc { Site = site, LeftNeighbour = oldArc.Site };
            oldArc.LeftNeighbour = site;

            InsertIntoSkiplist(newArc);
        }

        private void InsertSite(SiteEvent site)
        {
            var newArcA = new Arc {Site = site, LeftNeighbour = site};
            var newArcB = new Arc {Site = site, LeftNeighbour = site};

            InsertIntoSkiplist(newArcA);
            InsertIntoSkiplist(newArcB);
            var neighbourhood = FindNeighbourhoodOf(newArcA);

            neighbourhood[3].Site = neighbourhood[1].Site;

            neighbourhood[1].LeftNeighbour = neighbourhood[0].Site;
            neighbourhood[2].LeftNeighbour = neighbourhood[1].Site;
            neighbourhood[3].LeftNeighbour = neighbourhood[2].Site;
            neighbourhood[4].LeftNeighbour = neighbourhood[3].Site;

            PotentialCircleEvents.Add(new CircleEvent(neighbourhood[0], neighbourhood[1], neighbourhood[2]));
            PotentialCircleEvents.Add(new CircleEvent(neighbourhood[2], neighbourhood[3], neighbourhood[4]));
        }

        private List<IArc> FindNeighbourhoodOf(IArc arc)
        {
            var node = _arcs.FetchNode(arc);

            if (node.Right.Key.Site == arc.Site)
            {
                return new List<IArc> {node.Left.Left.Key, node.Left.Key, node.Key, node.Right.Key, node.Right.Right.Key};
            }
            else if (node.Left.Key.Site == arc.Site)
            {
                return new List<IArc> {node.Left.Left.Left.Key, node.Left.Left.Key, node.Left.Key, node.Key, node.Right.Key};
            }
            else
            {
                throw new ArgumentException("Arc's node does not have a neighbour with the same site!");
            }
        }

        private void InsertIntoSkiplist(IArc arc)
        {
            Sweepline.Z = arc.Site.Position.Z;
            _arcs.Insert(arc);
            _count++;
        }
        #endregion

        #region Remove methods
        public void Remove(CircleEvent circleEvent)
        {
            Sweepline.Z = circleEvent.Priority - 1;
            Remove(circleEvent.MiddleArc);
        }

        public void Remove(IArc arc)
        {
            var node = _arcs.FetchNode(arc);
            var successfullyRemoved = _arcs.Remove(arc);
            if (!successfullyRemoved)
            {
                throw new DataException("Failed to remove arc!");
            }
            node.Right.Key.LeftNeighbour = node.Left.Key.Site;
            _count--;

            PotentialCircleEvents.Add(new CircleEvent(node.Right.Right.Key, node.Right.Key, node.Left.Key));
            PotentialCircleEvents.Add(new CircleEvent(node.Right.Key, node.Left.Key, node.Left.Left.Key));
        }
        #endregion

        public void ClearPotentialCircleEventList()
        {
            PotentialCircleEvents = new List<CircleEvent>();
        }

        #region IEnumerator<IArc> methods
        public IEnumerator<IArc> GetEnumerator()
        {
            return _arcs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region ToString methods
        public override string ToString()
        {
            var arcStrings =_arcs.Select(arc => StringOfArc(arc)).ToArray();

            return String.Join("", arcStrings);
        }

        private string StringOfArc(IArc arc)
        {
            var azimuthOfIntersection = arc.LeftIntersection(Sweepline).SphericalCoordinates().Azimuth;
            var leftIntersectionString = String.Format("{0,3:N0}", Trig.RadianToDegree(azimuthOfIntersection));
            var arcString = arc.ToString();

            return leftIntersectionString + arcString;
        }
        #endregion
    }
}
