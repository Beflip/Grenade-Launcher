using UnityEngine;

public class CameraRotator : MonoBehaviour, IRotatable
{
    [SerializeField] [Range(0f, 100f)] private float _rotationSpeed = 5f;

    private InputManager _inputManager;
    private float _xRotation = 0f;

    public void Initialize()
    {
        _inputManager = InputManager.Instance;
    }

    public void Rotate()
    {
        Vector2 rotationInput = _inputManager.GetRotationInput();
        float rotationAmount = rotationInput.y * _rotationSpeed * Time.deltaTime;

        _xRotation -= rotationAmount;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        RotateCamera(_xRotation);
    }

    private void RotateCamera(float xRotation)
    {
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}