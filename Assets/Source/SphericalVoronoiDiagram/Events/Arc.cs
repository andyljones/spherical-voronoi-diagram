using System;
using System.Collections.Generic;
using Assets.Source.SphericalVoronoiDiagram.Events;
using C5;
using UnityEngine;
using Diag = System.Diagnostics;

namespace Events
{
    public class Arc : IArc
    {
        public IArc LeftArc { get; set; }
        public IArc RightArc { get; set; }

        public SiteEvent Site { get; set; }
        public IPriorityQueueHandle<IEvent> CircleEventHandle { get; set; }

        public Arc()
        {
            LeftArc = this;
            RightArc = this;
        }

        public bool Contains(IArc arc)
        {
            var a = LeftArc.Site.Position;
            var b = this.Site.Position;
            var c = RightArc.Site.Position;
            var p = arc.Site.Position;

            var alpha = p.x*(p.x - a.x) + p.y*(p.y - a.y);
            var beta = p.x*(p.x - b.x) + p.y*(p.y - b.y);
            var gamma = p.x*(p.x - c.x) + p.y*(p.y - c.y);

            var arcIsRightOfLeftIntersection = alpha*Mathf.Abs(b.z - p.z) >= beta*Mathf.Abs(a.z - p.z);
            var arcIsLeftOfRightIntersection = beta* Mathf.Abs(c.z - p.z) <= gamma*Mathf.Abs(b.z - p.z);

            return arcIsRightOfLeftIntersection && arcIsLeftOfRightIntersection;
        }

        public List<IArc> SplitAt(IArc splittingArc)
        {
            var results = new List<IArc>();

            if (!this.Contains(splittingArc))
            {
                throw new ArgumentException("Tried to split arc using arc that is not nested in it!");
            }
            else
            {
                var newLeftPart = new Arc {Site = this.Site};
                LeftArc.ConnectToLeftOf(newLeftPart);
                newLeftPart.ConnectToLeftOf(splittingArc);
                results.Add(newLeftPart);

                var newRightPart = new Arc {Site = this.Site};
                RightArc.ConnectToRightOf(newRightPart);
                newRightPart.ConnectToRightOf(splittingArc);
                results.Add(newRightPart);
            }

            return results;
        }

        public void ConnectToLeftOf(IArc otherArc)
        {
            this.RightArc = otherArc;
            otherArc.LeftArc = this;
        }

        public void ConnectToRightOf(IArc otherArc)
        {
            this.LeftArc = otherArc;
            otherArc.RightArc = this;
        }
    }
}
