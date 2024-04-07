using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshUtility
{
    private Mesh _mesh;
    private int[] _triangles;
    private Vector3[] _vertices;
    private Vector2[] _uvCoordinates;

    public MeshUtility(Mesh mesh)
    {
        _mesh = mesh;
        _triangles = _mesh.triangles;
        _vertices = _mesh.vertices;
        _uvCoordinates = _mesh.uv;
    }

    public bool MapLocalPointToUV(Vector3 localPoint, Matrix4x4 modelViewProjectionMatrix, out Vector2 uv)
    {
        int vertexIndex1;
        int vertexIndex2;
        int vertexIndex3;
        Vector3 triangleVertex1;
        Vector3 triangleVertex2;
        Vector3 triangleVertex3;
        Vector3 point = localPoint;

        for (int i = 0; i < _triangles.Length; i += 3)
        {
            vertexIndex1 = i;
            vertexIndex2 = i + 1;
            vertexIndex3 = i + 2;

            triangleVertex1 = _vertices[_triangles[vertexIndex1]];
            triangleVertex2 = _vertices[_triangles[vertexIndex2]];
            triangleVertex3 = _vertices[_triangles[vertexIndex3]];

            if (!GeometryUtility.PointExistsInPlane(point, triangleVertex1, triangleVertex2, triangleVertex3))
                continue;
            if (!GeometryUtility.PointExistsOnTriangleEdge(point, triangleVertex1, triangleVertex2, triangleVertex3) && !GeometryUtility.PointExistsInTriangle(point, triangleVertex1, triangleVertex2, triangleVertex3))
                continue;

            var uv1 = _uvCoordinates[_triangles[vertexIndex1]];
            var uv2 = _uvCoordinates[_triangles[vertexIndex2]];
            var uv3 = _uvCoordinates[_triangles[vertexIndex3]];
            uv = GeometryUtility.CalculateTextureCoordinate(point, triangleVertex1, uv1, triangleVertex2, uv2, triangleVertex3, uv3, modelViewProjectionMatrix);

            return true;
        }
        uv = default(Vector3);
        return false;
    }

    public Vector3 FindNearestLocalSurfacePoint(Vector3 localPoint)
    {
        var point = localPoint;
        var triangleVertices = GeometryUtility.GetNearestVerticesOfTriangle(point, _vertices, _triangles);
        var projectedPoints = new List<Vector3>();
        for (int i = 0; i < triangleVertices.Length; i += 3)
        {
            var vertexIndex1 = i;
            var vertexIndex2 = i + 1;
            var vertexIndex3 = i + 2;
            projectedPoints.Add(GeometryUtility.ProjectPointOntoTriangle(point, triangleVertices[vertexIndex1], triangleVertices[vertexIndex2], triangleVertices[vertexIndex3]));
        }
        return projectedPoints.OrderBy(t => Vector3.Distance(point, t)).First();
    }
}