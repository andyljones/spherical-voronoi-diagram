﻿using System;
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

            InsertArc(newArc);
        }

        private void InsertSecondSite(SiteEvent site)
        {
            var oldArc = _arcs.First();
            var newArc = new Arc { Site = site, LeftNeighbour = oldArc.Site };
            oldArc.LeftNeighbour = site;

            InsertArc(newArc);
        }

        private void InsertSite(SiteEvent site)
        {
            var newArcA = new Arc {Site = site, LeftNeighbour = site};
            var newArcB = new Arc {Site = site, LeftNeighbour = site};

            InsertArc(newArcA);
            InsertArc(newArcB);
            var neighbourhood = FindNeighbourhoodOf(newArcA);

            neighbourhood[2].Site = neighbourhood[0].Site;

            neighbourhood[1].LeftNeighbour = neighbourhood[0].Site;
            neighbourhood[2].LeftNeighbour = neighbourhood[1].Site;
            neighbourhood[3].LeftNeighbour = neighbourhood[2].Site;
        }

        private List<IArc> FindNeighbourhoodOf(IArc arc)
        {
            var node = _arcs.FetchNode(arc);

            if (node.Left.Key.Site != arc.Site)
            {
                return new List<IArc> {node.Left.Key, node.Key, node.Right.Key, node.Right.Right.Key};
            }
            else if (node.Right.Key.Site != arc.Site)
            {
                return new List<IArc> {node.Left.Left.Key, node.Left.Key, node.Key, node.Right.Key};
            }
            else
            {
                throw new ArgumentException("Arc's node does not have a neighbour with the same site!");
            }
        }

        private void InsertArc(IArc arc)
        {
            _sweepline.Z = arc.Site.Position.Z;
            _arcs.Insert(arc);
            _count++;
        }
        #endregion

        #region Remove methods
        public void Remove(Arc arc)
        {
            var node = _arcs.FetchNode(arc);
            node.Right.Key.LeftNeighbour = node.Left.Key.Site;

            _arcs.Remove(arc);
            _count--;
        }
        #endregion

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
            var azimuthOfIntersection = arc.LeftIntersection(_sweepline).SphericalCoordinates().Azimuth;
            var leftIntersectionString = String.Format("{0,3:N0}", azimuthOfIntersection);
            var arcString = arc.ToString();

            return leftIntersectionString + arcString;
        }
        #endregion
    }
}
