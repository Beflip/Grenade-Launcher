using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour, IMovable
{
    [SerializeField] [Range(0f, 100f)] private float _moveSpeed = 10f;

    private CharacterController _characterController;
    private InputManager _inputManager;
    private Vector3 _velocity = Vector3.zero;

    public void Initialize()
    {
        _characterController = GetComponent<CharacterController>();
        _inputManager = InputManager.Instance;
    }

    public void Move()
    {
        Vector2 movementInput = _inputManager.GetMovementInput();
        Vector3 movementDirection = transform.forward * movementInput.y + transform.right * movementInput.x;
        Vector3 movement = movementDirection * _moveSpeed * Time.deltaTime;

        ApplyGravity();
        movement += _velocity * Time.deltaTime;

        MoveCharacter(movement);
    }

    private void MoveCharacter(Vector3 movement)
    {
        _characterController.Move(movement);
    }

    private void ApplyGravity()
    {
        _velocity.y -= CustomGravity.Value * Time.deltaTime;

        if (_characterController.isGrounded && _velocity.y < 0)
        {
            _velocity.y = 0f;
        }
    }
}