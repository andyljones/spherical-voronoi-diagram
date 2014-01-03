using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IArc
{
    Vector3 DirectionOfLeftIntersection { get; }
    Vector3 DirectionOfRightIntersection { get; }
}