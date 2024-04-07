using UnityEngine;

[System.Serializable]
public class FreezePosition
{
    [SerializeField] private bool _x;
    [SerializeField] private bool _y;
    [SerializeField] private bool _z;

    public bool X => _x;
    public bool Y => _y;
    public bool Z => _z;

    public FreezePosition()
    {
        _x = false;
        _y = false;
        _z = false;
    }
}