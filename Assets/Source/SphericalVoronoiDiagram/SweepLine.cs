using UnityEngine;

namespace SphericalVoronoiDiagram
{
    public class SweepLine
    {
        public Vector3 PointOnSweepline
        {
            get
            {
                return _pointOnSweepline;
            }
            set
            {
                _pointOnSweepline = value.normalized;
            }
        }
        private Vector3 _pointOnSweepline;


        public float PolarAngle
        {
            get
            {
                return Mathf.Acos(PointOnSweepline.normalized.z);
            }
        }
    }
}