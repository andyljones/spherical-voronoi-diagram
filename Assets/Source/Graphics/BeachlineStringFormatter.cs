using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class BeachlineStringFormatter
{
    public static String ConvertToString(Beachline beachline)
    {
        var arcs = beachline.ToList();

        var stringBuilder = new StringBuilder();
        foreach (var arc in arcs)
        {
            stringBuilder.AppendFormat(
                "({0,3:N0},{1,3:N0},{2,3:N0})", 
                180 / Mathf.PI * MathUtils.AzimuthOf(arc.DirectionOfLeftIntersection),
                180 / Mathf.PI * MathUtils.AzimuthOf(arc.SiteEvent.Position),
                180 / Mathf.PI * MathUtils.AzimuthOf(arc.DirectionOfRightIntersection));
        }

        return stringBuilder.ToString();
    }
}