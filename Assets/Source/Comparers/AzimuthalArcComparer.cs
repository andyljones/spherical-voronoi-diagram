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

            var leftAzimuthOfA = AzimuthOfLeftEndpointOf(a);
            var leftAzimuthOfB = AzimuthOfLeftEndpointOf(b);

            var differenceInLeftAzimuth = leftAzimuthOfA - leftAzimuthOfB;

            if (differenceInLeftAzimuth != 0)
            {
                return Math.Sign(differenceInLeftAzimuth);
            }
            else
            {
                var rightAzimuthOfA = AzimuthOfRightEndpointOf(a);
                var rightAzimuthOfB = AzimuthOfRightEndpointOf(b);

                var differenceInRightazimuth = rightAzimuthOfA - rightAzimuthOfB;

                if (differenceInRightazimuth != 0)
                {
                    return Math.Sign(differenceInRightazimuth);
                }
                else
                {
                    return 1; //Arbitrary but necessary to stop treeset deduplicating
                }
            }
        }

        private float AzimuthOfLeftEndpointOf(Arc a)
        {
            var leftAzimuth = Mathf.Atan2(-a.LeftEndpoint.y, a.LeftEndpoint.x);

            return MathMod(leftAzimuth, 2 * Mathf.PI);
        }

        private float AzimuthOfRightEndpointOf(Arc a)
        {
            var leftAzimuth = Mathf.Atan2(-a.RightEndpoint.y, a.RightEndpoint.x);

            return MathMod(leftAzimuth, 2 * Mathf.PI);
        }

        private float MathMod(float x, float m)
        {
            return ((x%m) + m)%m;
        }
    }
}