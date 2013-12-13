using System;
using System.Collections.Generic;
using Assets.Source.SphericalVoronoiDiagram.Events;
using UnityEngine;

namespace Events
{
    public class AzimuthalArcComparer : IComparer<IArc>
    {
        public int Compare(IArc newArc, IArc beachlineArc)
        {
            if (ReferenceEquals(newArc, beachlineArc))
            {
                return 0;
            }

            var a = beachlineArc.LeftArc.Site.Position;
            var b = beachlineArc.Site.Position;
            var p = newArc.Site.Position;

            if (a == b)
            {
                var azimuthOfBeachlineSite = Mathf.Atan2(-a.y, a.x);
                var azimuthOfNewSite = Mathf.Atan2(-p.y, p.x);

                var newSiteIsRightOfBeachlineSite = azimuthOfNewSite > azimuthOfBeachlineSite;

                if (newSiteIsRightOfBeachlineSite)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }

            var alpha = p.x * (p.x - a.x) + p.y * (p.y - a.y);
            var beta = p.x * (p.x - b.x) + p.y * (p.y - b.y);

            var newArcIsRightOfLeftIntersection = alpha * Mathf.Abs(b.z - p.z) >= beta * Mathf.Abs(a.z - p.z);

            if (newArcIsRightOfLeftIntersection)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}