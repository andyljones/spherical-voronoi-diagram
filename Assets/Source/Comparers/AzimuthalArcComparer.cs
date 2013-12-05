using System;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class AzimuthalArcComparer : IComparer<Arc>
    {
        public int Compare(Arc a, Arc b)
        {
            if (ReferenceEquals(a, b))
            {
                return 0;
            }

            var azimuthOfA = AzimuthOfLeftEndpointOf(a);
            var azimuthOfB = AzimuthOfLeftEndpointOf(b);

            var differenceInAzimuth = azimuthOfA - azimuthOfB;

            if (differenceInAzimuth == 0)
            {
                return 1; // Arbitrary, but neccessary as Compare is used to deduplicate the TreeSet. Stupid mechanism.
            }
            else
            {
                return Math.Sign(differenceInAzimuth);
            }
        }

        private float AzimuthOfLeftEndpointOf(Arc a)
        {
            var leftAzimuth = Mathf.Atan2(-a.LeftEndpoint.y, a.LeftEndpoint.x);

            return MathMod(leftAzimuth, 2 * Mathf.PI);
        }

        private float MathMod(float x, float m)
        {
            return ((x%m) + m)%m;
        }
    }
}