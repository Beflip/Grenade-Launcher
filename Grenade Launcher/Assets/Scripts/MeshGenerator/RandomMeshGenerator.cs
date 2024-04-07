using UnityEngine;

public class RandomMeshGenerator : MonoBehaviour, IGeneratable
{
    private MeshGenerator[] _meshGenerators;
    private MeshGeneratorSphere _spherePrefab;
    private MeshGeneratorCube _cubePrefab;
    private MeshGeneratorPyramid _pyramidPrefab;
    private int _randomIndex;

    public Color Color => _meshGenerators[_randomIndex].Material.color;
    public Material Material => _meshGenerators[_randomIndex].Material;

    public void Initialize()
    {
        _spherePrefab = gameObject.AddComponent<MeshGeneratorSphere>();
        _cubePrefab = gameObject.AddComponent<MeshGeneratorCube>();
        _pyramidPrefab = gameObject.AddComponent<MeshGeneratorPyramid>();

        _meshGenerators = new MeshGenerator[] {
            _spherePrefab,
            _cubePrefab,
            _pyramidPrefab
        };
    }

    public void Generate()
    {
        _randomIndex = Random.Range(0, _meshGenerators.Length);
        MeshGenerator selectedGenerator = _meshGenerators[_randomIndex];
        selectedGenerator.Initialize();
        selectedGenerator.GenerateRandomProperties();
        selectedGenerator.Generate();
    }
}
