using System;
using System.Collections.Generic;
using System.Linq;
using Generator;
using MathNet.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

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
            var arcs = beachline.ToList();
            var vertices = new List<Vector3>();
            for (int i = 0; i < arcs.Count-1; i++)
            {
                var arc = arcs[i];
                var nextArc = arcs[i + 1];
                vertices.AddRange(ArcVertices(arc, nextArc, beachline.Sweepline));
            }
            if (arcs.Count > 0)
            {
                vertices.AddRange(ArcVertices(arcs[arcs.Count - 1], arcs[0], beachline.Sweepline));
            }

            return vertices.ToArray();
        }

        private static IEnumerable<Vector3> ArcVertices(IArc arc, IArc nextArc, Sweepline sweepline)
        {
            IEnumerable<float> azimuths;
            if (arc.LeftNeighbour == arc.Site)
            {
                azimuths = DrawingUtilities.AzimuthsInRange(0, 2 * Mathf.PI, NumberOfVerticesPerArc);
            }
            else
            {
                var leftLimit = DrawingUtilities.AzimuthOf(arc.LeftIntersection(sweepline));
                var rightLimit = DrawingUtilities.AzimuthOf(nextArc.LeftIntersection(sweepline));
                azimuths =  DrawingUtilities.AzimuthsInRange(leftLimit, rightLimit, NumberOfVerticesPerArc);
            }

            var vertices = azimuths.Select(azimuth => PointOnEllipse(arc, azimuth, sweepline)).ToList();

            return vertices;
        }

        public static Vector3 PointOnEllipse(IArc arc, float azimuth, Sweepline sweepline)
        {
            var direction = new Generator.Vector3(Trig.Cosine(azimuth), Trig.Sine(azimuth), 0);
            
            return arc.PointAt(direction, sweepline).ToUnityVector3();
        }

    }
}