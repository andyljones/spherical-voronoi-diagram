using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Site
{
    public readonly Vector3 Position;

    public Site(Vector3 position)
    {
        Position = position.normalized;
    }

    public override string ToString()
    {
        return Position.ToString();
    }
}