using System.Collections.Generic;
using System.Linq;
using C5;
using UnityEngine;

namespace Graphics
{
    public class CircleEventsDrawer
    {
        public static int NumberOfVerticesPerCircle = 1000;

        private GameObject _parentObject;
        private Dictionary<CircleEvent, GameObject> _gameObjects;
        private readonly IPriorityQueue<CircleEvent> _circleEvents;

        public CircleEventsDrawer(IPriorityQueue<CircleEvent> circleEvents)
        {
            _circleEvents = circleEvents;
            _parentObject = new GameObject("Circle Events");
            _gameObjects = new Dictionary<CircleEvent, GameObject>();
        }

        public void Update()
        {
            var newCircleEvents = _circleEvents.Except(_gameObjects.Keys);
            //Debug.Log(_circleEvents.Count());

            foreach (var circleEvent in newCircleEvents)
            {
                var vertices = CircleEventVertices(circleEvent);
                var gameObject = DrawingUtilities.CreateLineObject("Circle " + circleEvent, vertices,
                    "CircleEventMaterial");
                gameObject.transform.parent = _parentObject.transform;

                _gameObjects.Add(circleEvent, gameObject);
            }
        }

        private Vector3[] CircleEventVertices(CircleEvent circle)
        {
            var angles = DrawingUtilities.AzimuthsInRange(0, 2*Mathf.PI, NumberOfVerticesPerCircle);

            var points = angles.Select(angle => VertexOfCircleEvent(circle, angle)).ToArray();

            return points;
        }

        private Vector3 VertexOfCircleEvent(CircleEvent circle, float angle)
        {
            var a = circle.OriginalLeftNeighbour.Position;
            var b = circle.OriginalSiteEvent.Position;
            var c = circle.OriginalRightNeighbour.Position;

            var n = Vector3.Cross(a - b, c - b).normalized;

            var s = a - Vector3.Dot(a, n)*n;
            var t = Vector3.Cross(s, n);

            var vertex = Vector3.Dot(a, n)*n + Mathf.Cos(angle)*s + Mathf.Sin(angle)*t;

            return vertex;
        }
    }
}
