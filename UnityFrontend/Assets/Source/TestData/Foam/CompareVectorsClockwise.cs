using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foam
{
    /// <summary>
    /// Comparer for ordering vectors according to which is further clockwise around a given center, judged from a 
    /// given baseline. Uses a left-hand coordinate system.
    /// </summary>
    public class CompareVectorsClockwise : IComparer<Vector3>
    {
        private Vector3 _localX;
        private Vector3 _localY;
        private Vector3 _localZ;

        private readonly Vector3 _center;
        private readonly Vector3 _baseline;

        //TODO: Add errors if the center or baseline are degenerate.
        public CompareVectorsClockwise(Vector3 center, Vector3 baseline)
        {
            _center = center;
            _baseline = baseline;

            SetLocalCoordinateSystem();
        }

        private void SetLocalCoordinateSystem()
        {
            _localZ = _center.normalized;
            _localY = Vector3.Cross(_baseline.normalized, _center);
            _localX = Vector3.Cross(_localZ, _localY); // This is the baseline projected onto the plane perpendicular to _center.
        }

        /// <summary>
        /// Calculates whether the first argument is further around clockwise from the baseline than the second 
        /// </summary>
        /// <param name="u">First vector</param>
        /// <param name="v">Second vector</param>
        /// <returns>Returns 1 if u > v, 0 if u == v, -1 if v > u</returns>
        public int Compare(Vector3 u, Vector3 v)
        {
            // This is all doable with crossproducts, but projecting onto the local coordinate system and calculating 
            // angles is conceptually simpler.
            float u_x = Vector3.Dot(u, _localX);
            float u_y = Vector3.Dot(u, _localY);
            float uAngleFromLocalX = MathMod(Mathf.Atan2(u_y, u_x), 2 * Mathf.PI);

            float v_x = Vector3.Dot(v, _localX);
            float v_y = Vector3.Dot(v, _localY);

            float vAngleFromLocalX = MathMod(Mathf.Atan2(v_y, v_x), 2*Mathf.PI);

            // vAngle & uAngle are calculated ~anticlockwise~ from the local X axis 'cause that's how Atan2 works.
            // So we take vAngle - uAngle to get the clockwise angle from v to u.
            return Math.Sign(vAngleFromLocalX - uAngleFromLocalX);
        }

        // Calculates x mod m. The built-in % operator is the remainder operator, not the modulo operator.
        private float MathMod(float x, float m)
        {
            return ((x % m) + m) % m;
        }
    }
}
