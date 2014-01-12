using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Foam
{
    /// <summary>
    /// Creates a copy of a Foam structure, with the copy represented as a set of dictionaries mapping the original objects to their copies.
    /// </summary>
    public class FoamCopier
    {
        public Dictionary<Vertex, Vertex> VertexDictionary;
        public Dictionary<Edge, Edge> EdgeDictionary;
        public Dictionary<Face, Face> FaceDictionary; 

        /// <summary>
        /// Create a copy of a Foam structure from a list of Faces.
        /// </summary>
        /// <param name="oldFaces"></param>
        public FoamCopier(List<Face> oldFaces)
        {
            ConstructCopyDictionaries(oldFaces);
        }

        // This deep copy method returns a dictionary relating the old faces to their copies.
        private void ConstructCopyDictionaries(List<Face> oldFaces)
        {
            var oldVertices = oldFaces.SelectMany(face => face.Vertices).Distinct();
            var oldEdges = oldFaces.SelectMany(face => face.Edges).Distinct();

            // We associate each old polyhedron element with a new one by using the old element as a key.
            //TODO: Update it so it doesn't copy position? Keeps things consistent.
            VertexDictionary = oldVertices.ToDictionary(oldVertex => oldVertex, oldVertex => new Vertex { Position = oldVertex.Position });
            EdgeDictionary = oldEdges.ToDictionary(oldEdge => oldEdge, oldEdge => new Edge());
            FaceDictionary = oldFaces.ToDictionary(oldFace => oldFace, oldFace => new Face());

            Link(VertexDictionary, EdgeDictionary);
            Link(EdgeDictionary, FaceDictionary);
            Link(FaceDictionary, VertexDictionary);
        }

        // Suppose T is a Vertex and U is a Edge. This function links all the new Vertices stored in tDictionary.Values to 
        // all the new Edges stored in uDictionary.Values in the same way that the old Vertices in tDictionary.Keys are 
        // linked to the old Edges in uDictionary.Keys.
        //
        // Because it's a generic method, it can be reused for linking any two kinds of polyhedron element.
        private static void Link<T, U>(Dictionary<T, T> tDictionary, Dictionary<U, U> uDictionary)
        {
            // TODO: This will have issues if there's more than one field of a given type in a polyhedron element. Mark the relevant field with an attribute?
            var uPropertyInT = typeof(T).GetFields().Single(fieldInfo => fieldInfo.FieldType == typeof(List<U>));
            var tPropertyInU = typeof(U).GetFields().Single(fieldInfo => fieldInfo.FieldType == typeof(List<T>));

            foreach (var uPair in uDictionary)
            {
                var oldU = uPair.Key;
                var newU = uPair.Value;

                var tListInOldU = tPropertyInU.GetValue(oldU) as List<T>;
                var tListInNewU = tPropertyInU.GetValue(newU) as List<T>;

                foreach (var oldT in tListInOldU)
                {
                    var newT = tDictionary[oldT];
                    var uListInNewT = uPropertyInT.GetValue(newT) as List<U>;

                    tListInNewU.Add(newT);
                    uListInNewT.Add(newU);
                }
            }
        }
    }
}
