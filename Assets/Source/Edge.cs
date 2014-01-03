using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Edge
{
    public Vector3 Endpoint;
    public Edge PreviousEdge;

    public Edge(Vector3 endpoint, Edge previousEdge)
    {
        Endpoint = endpoint;
        PreviousEdge = previousEdge;
    }
}