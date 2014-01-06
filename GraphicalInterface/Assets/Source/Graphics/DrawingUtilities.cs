using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graphics
{
    public static class DrawingUtilities
    {
        public static IEnumerable<float> AzimuthsInRange(float leftAzimuth, float rightAzimuth, int pointsPerRange)
        {
            if (leftAzimuth <= rightAzimuth)
            {
                var distance = rightAzimuth - leftAzimuth;
                return Enumerable.Range(0, pointsPerRange + 1).Select(i => leftAzimuth + i * distance / pointsPerRange);
            }
            else
            {
                var distanceFromLeftToZero = Mathf.Abs(2 * Mathf.PI - leftAzimuth);
                var distanceFromRightTo2Pi = Mathf.Abs(rightAzimuth);
                var distance = distanceFromLeftToZero + distanceFromRightTo2Pi;

                var numberOfpointsFromLeft = Mathf.FloorToInt(pointsPerRange * distanceFromLeftToZero / distance);
                var numberOfpointsFromRight = Mathf.CeilToInt(pointsPerRange * distanceFromRightTo2Pi / distance);

                var pointsFromLeft =
                    Enumerable.Range(0, numberOfpointsFromLeft + 1)
                        .Select(i => leftAzimuth + i * distanceFromLeftToZero / numberOfpointsFromLeft);

                var pointsFromRight =
                    Enumerable.Range(0, numberOfpointsFromRight + 1)
                        .Select(i => i * distanceFromRightTo2Pi / numberOfpointsFromRight);

                return pointsFromLeft.Concat(pointsFromRight);
            }
        }

        public static GameObject CreateLineObject(String name, Vector3[] points, String materialName)
        {
            var gameObject = new GameObject(name);
            var meshFilter = gameObject.AddComponent<MeshFilter>();
            var renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material = Resources.Load(materialName, typeof(Material)) as Material;

            meshFilter.mesh.vertices = points;
            meshFilter.mesh.SetIndices(
                Enumerable.Range(0, points.Count()).ToArray(),
                MeshTopology.LineStrip,
                0);

            meshFilter.mesh.RecalculateNormals();
            meshFilter.mesh.uv = Enumerable.Repeat(new Vector2(0, 0), points.Count()).ToArray();

            return gameObject;
        }

        public static void UpdateLineMesh(Mesh mesh, Vector3[] vertices)
        {
            mesh.SetIndices(Enumerable.Range(0, 0).ToArray(), MeshTopology.LineStrip, 0);
            mesh.vertices = vertices;
            mesh.SetIndices(Enumerable.Range(0, mesh.vertexCount).ToArray(), MeshTopology.LineStrip, 0);
            mesh.RecalculateNormals();
            mesh.uv = Enumerable.Repeat(new Vector2(0, 0), mesh.vertexCount).ToArray();
        }
    }
}
