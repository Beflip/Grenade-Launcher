using UnityEngine;

[RequireComponent(typeof(CameraRotator))]
public class CharacterCamera : MonoBehaviour
{
    private IRotatable _rotatable;

    private void Start()
    {
        _rotatable = GetComponent<CameraRotator>();
        _rotatable.Initialize();
    }

    private void Update()
    {
        _rotatable.Rotate();
    }
}