using System;
using System.Linq;
using Generator;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Graphics
{
    public class SweeplineDrawer
    {
        public static int NumberOfVertices = 1000;

        private GameObject _gameObject;
        private readonly Sweepline _sweepline;

        public SweeplineDrawer(Sweepline sweepline)
        {
            _sweepline = sweepline;
            _gameObject = InitializeSweeplineObject(sweepline);
        }

        private static GameObject InitializeSweeplineObject(Sweepline sweepline)
        {
            var vertices = SweeplineVertices(sweepline);
            var gameObject = DrawingUtilities.CreateLineObject("Sweepline", vertices, "SweeplineMaterial");

            return gameObject;
        }

        public void Update()
        {
            var mesh = _gameObject.GetComponent<MeshFilter>().mesh;
            var vertices = SweeplineVertices(_sweepline);
            DrawingUtilities.UpdateLineMesh(mesh, vertices);
        }

        private static Vector3[] SweeplineVertices(Sweepline sweepline)
        {
            if (Math.Abs(sweepline.Z) >= 1)
            {
                return new Vector3[0];
            }

            var azimuths = DrawingUtilities.AzimuthsInRange(0, 2 * Mathf.PI, NumberOfVertices);
            var vertices = azimuths.Select(azimuth => VertexOnSweepline(sweepline, azimuth)).ToArray();

            return vertices;
        }

        private static Vector3 VertexOnSweepline(Sweepline sweepline, float azimuth)
        {
            var z = (float) sweepline.Z;

            var x = (float) Math.Sqrt(1 - z*z)*Mathf.Cos(azimuth);
            var y = (float) -Math.Sqrt(1 - z*z)*Mathf.Sin(azimuth);

            return new Vector3(x, y, z);
        }
    }
}
