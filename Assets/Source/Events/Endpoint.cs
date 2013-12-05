using UnityEngine;

namespace Events
{
    public class Endpoint
    {
        public Vector3 Position;

        public float x
        {
            get
            {
                return Position.x;
            }
        }

        public float y
        {
            get
            {
                return Position.y;
            }
        }

        public float z
        {
            get
            {
                return Position.z;
            }
        }

        public Endpoint()
        {
        }

        public Endpoint(Vector3 position)
        {
            Position = position;
        }

        public Endpoint(float x, float y, float z) : this(new Vector3(x, y, z))
        {
        }
    }
}
