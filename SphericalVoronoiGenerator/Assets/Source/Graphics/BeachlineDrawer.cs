using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graphics
{
    public class BeachlineDrawer
    {
        public static int NumberOfVerticesPerArc = 100;

        private GameObject _gameObject;
        private readonly Beachline _beachline;

        public BeachlineDrawer(Beachline beachline)
        {
            _beachline = beachline;
            _gameObject = InitializeBeachlineObject(beachline);
        }

        private static GameObject InitializeBeachlineObject(Beachline beachline)
        {
            var vertices = BeachlineVertices(beachline);
            var gameObject = DrawingUtilities.CreateLineObject("Beachline", vertices, "BeachlineMaterial");

            return gameObject;
        }

        public void Update()
        {
            _beachline.Sweepline.Z = _beachline.Sweepline.Z - 0.0000001f;
            var mesh = _gameObject.GetComponent<MeshFilter>().mesh;
            var vertices = BeachlineVertices(_beachline);
            DrawingUtilities.UpdateLineMesh(mesh, vertices);
            _beachline.Sweepline.Z = _beachline.Sweepline.Z + 0.0000001f;

        }

        private static Vector3[] BeachlineVertices(Beachline beachline)
        {
            var vertices = beachline.SelectMany(arc => ArcVertices(arc));

            return vertices.ToArray();
        }

        private static IEnumerable<Vector3> ArcVertices(Arc arc)
        {
            IEnumerable<float> azimuths;
            if (arc.LeftNeighbour == arc.SiteEvent && arc.SiteEvent == arc.RightNeighbour)
            {
                azimuths = DrawingUtilities.AzimuthsInRange(0, 2 * Mathf.PI, NumberOfVerticesPerArc);
            }
            else
            {
                var leftLimit = MathUtils.AzimuthOf(arc.DirectionOfLeftIntersection);
                var rightLimit = MathUtils.AzimuthOf(arc.DirectionOfRightIntersection);
                azimuths =  DrawingUtilities.AzimuthsInRange(leftLimit, rightLimit, NumberOfVerticesPerArc);
            }

            var vertices = azimuths.Select(azimuth => PointOnEllipse(arc, azimuth)).ToList();

            return vertices;
        }

        public static Vector3 PointOnEllipse(Arc arc, float azimuth)
        {
            var p = arc.SiteEvent.Position;
            var Z = arc.Sweepline.Z;

            var tx = Mathf.Cos(azimuth);
            var ty = -Mathf.Sin(azimuth);

            var tz = (Z - p.z) / (p.x * tx + p.y * ty - Mathf.Sqrt(1 - Z * Z));

            var x = 1/Mathf.Sqrt(1 + 1/(tz*tz))*tx;
            var y = 1/Mathf.Sqrt(1 + 1/(tz*tz))*ty;
            var z = Mathf.Sign(tz)*1/Mathf.Sqrt(1 + tz*tz);

            return new Vector3(x, y, z).normalized;
        }

    }
}