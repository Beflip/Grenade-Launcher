using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public abstract class MeshGenerator : MonoBehaviour, IGeneratable
{
    protected MeshFilter _meshFilter;
    protected MeshRenderer _meshRenderer;
    protected Material _material;
    protected Vector3[] _vertices;

    public Material Material => _material;

    public virtual void Initialize()
    {
        if (_meshFilter == null)
            _meshFilter = GetComponent<MeshFilter>();
        if (_meshRenderer == null)
            _meshRenderer = GetComponent<MeshRenderer>();
    }

    public virtual void Generate()
    {
        _vertices = CalculateVertices();

        int[] triangles = GenerateTriangles();
        Vector3[] normals = CalculateNormals();
        Vector2[] uvs = GenerateUVs();

        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = _vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uvs;

        _meshFilter.mesh = mesh;
        _meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _meshRenderer.receiveShadows = false;
        _material = MaterialGenerator.GenerateRandomMaterial();
        _meshRenderer.material = _material;
    }

    public abstract void GenerateRandomProperties();

    protected abstract Vector3[] CalculateVertices();

    protected abstract Vector3[] CalculateNormals();

    protected abstract Vector2[] GenerateUVs();

    protected abstract int[] GenerateTriangles();
}