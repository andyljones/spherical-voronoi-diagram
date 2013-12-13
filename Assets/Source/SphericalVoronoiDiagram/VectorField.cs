using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulator.ShallowFluid
{
    /// <summary>
    /// Represents a mapping of objects of type T to Vector3s.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VectorField<T> : Dictionary<T, Vector3>
    {
        /// <summary>
        /// Creates an empty vector field.
        /// </summary>
        public VectorField() : base()
        {
        }

        /// <summary>
        /// Interprets the dictionary as a vector field.
        /// </summary>
        /// <param name="dictionary"></param>
        public VectorField(Dictionary<T, Vector3> dictionary) : base(dictionary)
        {       
        }

        /// <summary>
        /// Creates a vector field using the given enumerable as keys. Values are initialized to (0f, 0f, 0f).
        /// </summary>
        /// <param name="enumerable"></param>
        public VectorField(IEnumerable<T> enumerable) : base(enumerable.ToDictionary(obj => obj, obj => default(Vector3)))
        {
        }
    }
}
