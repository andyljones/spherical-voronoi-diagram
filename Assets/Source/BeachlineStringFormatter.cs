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
                180 / Mathf.PI * arc.AzimuthOfLeftIntersection(),
                180 / Mathf.PI * arc.SiteEvent.Azimuth(),
                180 / Mathf.PI * arc.AzimuthOfRightIntersection());
        }

        return stringBuilder.ToString();
    }
}