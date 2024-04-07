using UnityEngine;

public class MeshGeneratorSphere : MeshGenerator
{
    [SerializeField] [Min(0f)] private float _radius = 0.5f;
    [SerializeField] [Min(0f)] private int _segments = 12;

    protected override Vector3[] CalculateNormals()
    {
        Vector3[] normals = new Vector3[_vertices.Length];

        for (int i = 0; i < _vertices.Length; i++)
        {
            normals[i] = _vertices[i].normalized;
        }

        return normals;
    }

    protected override Vector3[] CalculateVertices()
    {
        Vector3[] vertices = new Vector3[(_segments + 1) * (_segments + 1)];

        for (int i = 0; i <= _segments; i++)
        {
            float normalizedY = (float)i / _segments;
            float y = Mathf.Lerp(-_radius, _radius, normalizedY);
            float circumference = Mathf.Sqrt(Mathf.Pow(_radius, 2) - Mathf.Pow(y, 2));
            for (int j = 0; j <= _segments; j++)
            {
                float normalizedX = (float)j / _segments;
                float x = circumference * Mathf.Cos(normalizedX * Mathf.PI * 2);
                float z = circumference * Mathf.Sin(normalizedX * Mathf.PI * 2);
                Vector3 vertex = new Vector3(x, y, z);
                vertices[i * (_segments + 1) + j] = vertex;
            }
        }

        return vertices;
    }

    protected override int[] GenerateTriangles()
    {
        int[] triangles = new int[_segments * _segments * 6];
        int index = 0;

        for (int i = 0; i < _segments; i++)
        {
            for (int j = 0; j < _segments; j++)
            {
                int vertexIndex = i * (_segments + 1) + j;
                triangles[index++] = vertexIndex;
                triangles[index++] = vertexIndex + _segments + 1;
                triangles[index++] = vertexIndex + 1;
                triangles[index++] = vertexIndex + 1;
                triangles[index++] = vertexIndex + _segments + 1;
                triangles[index++] = vertexIndex + _segments + 2;
            }
        }

        return triangles;
    }

    protected override Vector2[] GenerateUVs()
    {
        Vector2[] uvs = new Vector2[_vertices.Length];

        for (int i = 0; i < _vertices.Length; i++)
        {
            Vector3 normalizedVertex = _vertices[i].normalized;
            uvs[i] = new Vector2(
                Mathf.Atan2(normalizedVertex.z, normalizedVertex.x) / (2 * Mathf.PI),
                Mathf.Acos(normalizedVertex.y) / Mathf.PI
            );
        }

        return uvs;
    }

    public override void GenerateRandomProperties()
    {
        _radius = Random.Range(0.2f, 0.4f);
        _segments = Random.Range(4, 8);
    }
}