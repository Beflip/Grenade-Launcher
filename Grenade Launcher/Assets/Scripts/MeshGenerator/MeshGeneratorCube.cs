using UnityEngine;

public class MeshGeneratorCube : MeshGenerator
{
    [SerializeField] [Min(0f)] private float _width = 1f;
    [SerializeField] [Min(0f)] private float _length = 1f;
    [SerializeField] [Min(0f)] private float _height = 1f;
    [SerializeField] [Min(0f)] private float _scatter = 0f;

    private Vector3[] _corners;

    public override void Generate()
    {
        _corners = CalculateCornerPositions();

        if (_scatter > 0)
            _corners = CalculateScatter();

        base.Generate();
    }

    private Vector3[] CalculateCornerPositions()
    {
        float halfWidth = _width * 0.5f;
        float halfLength = _length * 0.5f;
        float halfHeight = _height * 0.5f;

        Vector3[] corners = new Vector3[]
        {
            new Vector3(-halfWidth, -halfHeight, halfLength),
            new Vector3(halfWidth, -halfHeight, halfLength),
            new Vector3(halfWidth, -halfHeight, -halfLength),
            new Vector3(-halfWidth, -halfHeight, -halfLength),
            new Vector3(-halfWidth, halfHeight, halfLength),
            new Vector3(halfWidth, halfHeight, halfLength),
            new Vector3(halfWidth, halfHeight, -halfLength),
            new Vector3(-halfWidth, halfHeight, -halfLength)
        };

        return corners;
    }

    protected override Vector3[] CalculateVertices()
    {
        Vector3[] vertices = new Vector3[]
        {
            _corners[0], _corners[1], _corners[2], _corners[3],
            _corners[7], _corners[4], _corners[0], _corners[3],
            _corners[4], _corners[5], _corners[1], _corners[0],
            _corners[6], _corners[7], _corners[3], _corners[2],
            _corners[5], _corners[6], _corners[2], _corners[1],
            _corners[7], _corners[6], _corners[5], _corners[4]
        };

        return vertices;
    }

    protected override Vector3[] CalculateNormals()
    {
        Vector3[] normals = new Vector3[]
        {
            Vector3.down, Vector3.down, Vector3.down, Vector3.down,
            Vector3.left, Vector3.left, Vector3.left, Vector3.left,
            Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward,
            Vector3.back, Vector3.back, Vector3.back, Vector3.back,
            Vector3.right, Vector3.right, Vector3.right, Vector3.right,
            Vector3.up, Vector3.up, Vector3.up, Vector3.up
        };

        return normals;
    }

    protected override Vector2[] GenerateUVs()
    {
        Vector2[] uvs = new Vector2[]
        {
            new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(1f, 0f),
            new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(1f, 0f),
            new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(1f, 0f),
            new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(1f, 0f),
            new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(1f, 0f),
            new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(1f, 0f)
        };

        return uvs;
    }

    protected override int[] GenerateTriangles()
    {
        int[] triangles = new int[]
        {
            3, 1, 0, 3, 2, 1,
            7, 5, 4, 7, 6, 5,
            11, 9, 8, 11, 10, 9,
            15, 13, 12, 15, 14, 13,
            19, 17, 16, 19, 18, 17,
            23, 21, 20, 23, 22, 21
        };

        return triangles;
    }

    private Vector3[] CalculateScatter()
    {
        for (int i = 0; i < _corners.Length; i++)
        {
            _corners[i] += Random.insideUnitSphere * _scatter;
        }

        return _corners;
    }

    public override void GenerateRandomProperties()
    {
        _width = Random.Range(0.4f, 0.6f);
        _length = Random.Range(0.4f, 0.6f);
        _height = Random.Range(0.4f, 0.6f);
        _scatter = Random.Range(0f, 0.25f);
    }
}