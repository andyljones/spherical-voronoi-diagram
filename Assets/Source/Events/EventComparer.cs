using System;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class EventComparer : IComparer<IEvent>
    {
        public int Compare(IEvent lhs, IEvent rhs)
        {
            var directionOfLHS = lhs.Position.normalized;
            var directionOfRHS = rhs.Position.normalized;

            if (directionOfLHS == directionOfRHS)
            {
                return 0;
            }

            if (directionOfLHS.z != directionOfRHS.z)
            {
                return Math.Sign(directionOfLHS.z - directionOfRHS.z);
            }

            var azimuthOfLHS = Mathf.Atan2(-directionOfLHS.y, directionOfLHS.x);
            var azimuthOfRHS = Mathf.Atan2(-directionOfRHS.y, directionOfRHS.x);

            return Math.Sign(azimuthOfLHS - azimuthOfRHS);
        }
    }
}