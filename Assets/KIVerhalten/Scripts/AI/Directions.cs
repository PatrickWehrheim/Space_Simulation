

using System.Collections.Generic;
using UnityEngine;

public static class Directions
{
    public static List<Vector3> EightDirectionsCircle = new List<Vector3>
    {
        new Vector3(0, 0, 1).normalized,
        new Vector3(1, 0, 1).normalized,
        new Vector3(1, 0, 0).normalized,
        new Vector3(1, 0, -1).normalized,
        new Vector3(0, 0, -1).normalized,
        new Vector3(-1, 0, -1).normalized,
        new Vector3(-1, 0, 0).normalized,
        new Vector3(-1, 0, 1).normalized,
    };

    public static List<Vector3> SixDirectionsSphere = new List<Vector3>
    {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back
    };
}
