using System;
using System.Collections.Generic;
using UnityEngine;
using Diag = System.Diagnostics;

namespace Events
{
    public class Arc
    {
        public Endpoint LeftEndpoint;
        public Endpoint RightEndpoint;

        public SiteEvent Site;
        public CircleEvent CircleEvent;

        public Arc()
        {
        }

        public Arc(Endpoint left, Endpoint right)
        {
            LeftEndpoint = left;
            RightEndpoint = right;
        }

        public Arc(Endpoint left, Endpoint right, SiteEvent site) 
            : this(left, right)
        {
            Site = site;
        }

        public Arc(SiteEvent site)
            : this(new Endpoint(site.Position), new Endpoint(site.Position))
        {
            Site = site;
        }

        public Arc(Vector3 left, Vector3 right)
            : this(new Endpoint(left), new Endpoint(right))
        {
        }

        public Arc(Vector3 left, Vector3 right, SiteEvent site)
            : this(new Endpoint(left), new Endpoint(right), site)
        {
        }

        public bool Contains(Arc otherArc)
        {
            var azimuthOfLeftOfThis = AzimuthOf(this.LeftEndpoint);
            var azimuthOfRightOfThis = AzimuthOf(this.RightEndpoint);
            var thisStraddlesPi = azimuthOfRightOfThis < azimuthOfLeftOfThis;

            var azimuthOfLeftOfOther = AzimuthOf(otherArc.LeftEndpoint);
            var azimuthOfRightOfOther = AzimuthOf(otherArc.RightEndpoint);
            var otherStraddlesPi = azimuthOfRightOfOther < azimuthOfLeftOfOther;

            if (thisStraddlesPi == otherStraddlesPi)
            {
                return (azimuthOfLeftOfThis <= azimuthOfLeftOfOther) && (azimuthOfRightOfThis >= azimuthOfRightOfOther);
            }
            else if (thisStraddlesPi && !otherStraddlesPi)
            {
                return (azimuthOfLeftOfThis <= azimuthOfLeftOfOther) || (azimuthOfRightOfThis >= azimuthOfRightOfOther);
            }
            else
            {
                return false;
            }
        }

        private float AzimuthOf(Endpoint point)
        {
            return Mathf.Atan2(-point.Position.y, point.Position.x);
        }

        public List<Arc> SplitAt(Arc anotherArc)
        {
            var results = new List<Arc>();

            if (!this.Contains(anotherArc))
            {
                throw new ArgumentException("Tried to split arc using arc that is not nested in it!");
            }
            else
            {
                results.Add(new Arc(LeftEndpoint, anotherArc.LeftEndpoint, Site));
                results.Add(new Arc(anotherArc.RightEndpoint, RightEndpoint, Site));
            }

            return results;
        }

        public void ConnectToLeftOf(Arc anotherArc)
        {
            RightEndpoint = anotherArc.LeftEndpoint;
        }
    }
}
