using UnityEngine;

public class CharacterRotator : MonoBehaviour, IRotatable
{
    [SerializeField] [Range(0f, 100f)] private float _rotationSpeed = 15f;

    private InputManager _inputManager;

    public void Initialize()
    {
        _inputManager = InputManager.Instance;
    }

    public void Rotate()
    {
        Vector2 rotationInput = _inputManager.GetRotationInput();
        float rotationAmount = rotationInput.x * _rotationSpeed * Time.deltaTime;

        RotateCharacter(rotationAmount);
    }

    private void RotateCharacter(float rotationAmount)
    {
        transform.Rotate(Vector3.up, rotationAmount);
    }
}