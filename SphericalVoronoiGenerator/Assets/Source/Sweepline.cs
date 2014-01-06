using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Sweepline
{
    public float Z;

    public Sweepline(float z)
    {
        Z = z;
    }

    public float Colatitude()
    {
        return Mathf.Acos(Z);
    }

    public override string ToString()
    {
        return Z.ToString();
    }
}