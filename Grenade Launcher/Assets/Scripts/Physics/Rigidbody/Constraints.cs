using UnityEngine;

[System.Serializable]
public class Constraints
{
    [SerializeField] private FreezePosition _freezePosition;
    [SerializeField] private FreezeRotation _freezeRotation;

    public FreezePosition FreezePosition => _freezePosition;
    public FreezeRotation FreezeRotation => _freezeRotation;

    public Constraints()
    {
        _freezePosition = new FreezePosition();
        _freezeRotation = new FreezeRotation();
    }
}