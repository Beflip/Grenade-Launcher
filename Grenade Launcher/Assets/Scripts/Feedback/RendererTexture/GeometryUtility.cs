using System;
using System.Collections.Generic;
using UnityEngine;

public static class GeometryUtility
{
    private const float Tolerance = 1E-2f;

    public static bool PointExistsInPlane(Vector3 point, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        var v1 = vertex2 - vertex1;
        var v2 = vertex3 - vertex1;
        var vectorToP = point - vertex1;

        var normalVector = Vector3.Cross(v1, v2);
        var dotProduct = Vector3.Dot(normalVector.normalized, vectorToP.normalized);
        if (-Tolerance < dotProduct && dotProduct < Tolerance)
            return true;
        return false;
    }

    public static bool PointExistsOnTriangleEdge(Vector3 point, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        if (PointExistsOnEdge(point, vertex1, vertex2) || PointExistsOnEdge(point, vertex2, vertex3) || PointExistsOnEdge(point, vertex3, vertex1))
            return true;
        return false;
    }

    public static bool PointExistsInTriangle(Vector3 point, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        var edge1 = Vector3.Cross(vertex1 - vertex3, point - vertex1).normalized;
        var edge2 = Vector3.Cross(vertex2 - vertex1, point - vertex2).normalized;
        var edge3 = Vector3.Cross(vertex3 - vertex2, point - vertex3).normalized;

        var dotEdge1Edge2 = Vector3.Dot(edge1, edge2);
        var dotEdge2Edge3 = Vector3.Dot(edge2, edge3);

        if (1 - Tolerance < dotEdge1Edge2 && 1 - Tolerance < dotEdge2Edge3)
            return true;
        return false;
    }

    public static Vector2 CalculateTextureCoordinate(Vector3 point, Vector3 vertex1, Vector2 vertex1UV, Vector3 vertex2, Vector2 vertex2UV, Vector3 vertex3, Vector2 vertex3UV, Matrix4x4 transformMatrix)
    {
        Vector4 p1_projected = transformMatrix * new Vector4(vertex1.x, vertex1.y, vertex1.z, 1);
        Vector4 p2_projected = transformMatrix * new Vector4(vertex2.x, vertex2.y, vertex2.z, 1);
        Vector4 p3_projected = transformMatrix * new Vector4(vertex3.x, vertex3.y, vertex3.z, 1);
        Vector4 p_projected = transformMatrix * new Vector4(point.x, point.y, point.z, 1);
        Vector2 p1_normalized = new Vector2(p1_projected.x, p1_projected.y) / p1_projected.w;
        Vector2 p2_normalized = new Vector2(p2_projected.x, p2_projected.y) / p2_projected.w;
        Vector2 p3_normalized = new Vector2(p3_projected.x, p3_projected.y) / p3_projected.w;
        Vector2 p_normalized = new Vector2(p_projected.x, p_projected.y) / p_projected.w;
        var s = 0.5f * ((p2_normalized.x - p1_normalized.x) * (p3_normalized.y - p1_normalized.y) - (p2_normalized.y - p1_normalized.y) * (p3_normalized.x - p1_normalized.x));
        var s1 = 0.5f * ((p3_normalized.x - p_normalized.x) * (p1_normalized.y - p_normalized.y) - (p3_normalized.y - p_normalized.y) * (p1_normalized.x - p_normalized.x));
        var s2 = 0.5f * ((p1_normalized.x - p_normalized.x) * (p2_normalized.y - p_normalized.y) - (p1_normalized.y - p_normalized.y) * (p2_normalized.x - p_normalized.x));
        var u = s1 / s;
        var v = s2 / s;
        var w = 1 / ((1 - u - v) * 1 / p1_projected.w + u * 1 / p2_projected.w + v * 1 / p3_projected.w);
        return w * ((1 - u - v) * vertex1UV / p1_projected.w + u * vertex2UV / p2_projected.w + v * vertex3UV / p3_projected.w);
    }

    public static Vector3[] GetNearestVerticesOfTriangle(Vector3 point, Vector3[] vertices, int[] triangles)
    {
        List<Vector3> nearestVertices = new List<Vector3>();

        int nearestIndex = triangles[0];
        float nearestDistance = Vector3.Distance(vertices[nearestIndex], point);

        for (int i = 0; i < vertices.Length; ++i)
        {
            float distance = Vector3.Distance(vertices[i], point);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestIndex = i;
            }
        }

        for (int i = 0; i < triangles.Length; ++i)
        {
            if (triangles[i] == nearestIndex)
            {
                var remainder = i % 3;
                int index0 = i, index1 = 0, index2 = 0;
                switch (remainder)
                {
                    case 0:
                        index1 = i + 1;
                        index2 = i + 2;
                        break;

                    case 1:
                        index1 = i - 1;
                        index2 = i + 1;
                        break;

                    case 2:
                        index1 = i - 1;
                        index2 = i - 2;
                        break;
                }
                nearestVertices.Add(vertices[triangles[index0]]);
                nearestVertices.Add(vertices[triangles[index1]]);
                nearestVertices.Add(vertices[triangles[index2]]);
            }
        }
        return nearestVertices.ToArray();
    }

    public static Vector3 ProjectPointOntoTriangle(Vector3 point, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        var centroid = (vertex1 + vertex2 + vertex3) / 3;
        var pa = vertex1 - point;
        var pb = vertex2 - point;
        var pc = vertex3 - point;
        var ga = vertex1 - centroid;
        var gb = vertex2 - centroid;
        var gc = vertex3 - centroid;

        var distancePa = pa.magnitude;
        var distancePb = pb.magnitude;
        var distancePc = pc.magnitude;

        var minDistance = Mathf.Min(Mathf.Min(distancePa, distancePb), distancePc);

        Func<float, float, float> k = (t, u) => (t - minDistance + u - minDistance) / 2;

        var A = k(distancePb, distancePc);
        var B = k(distancePc, distancePa);
        var C = k(distancePa, distancePb);
        var projectedPoint = centroid + (ga * A + gb * B + gc * C);
        return projectedPoint;
    }

    private static bool PointExistsOnEdge(Vector3 point, Vector3 vertex1, Vector3 vertex2)
    {
        return 1 - Tolerance < Vector3.Dot((vertex2 - point).normalized, (vertex2 - vertex1).normalized);
    }
}