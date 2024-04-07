using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterJump : MonoBehaviour, IJumpable
{
    [SerializeField] [Range(0f, 25f)] private float _jumpForce = 5f;

    private CharacterController _characterController;
    private InputManager _inputManager;
    private bool _isGrounded = true;

    private void OnDisable()
    {
        if (_inputManager != null)
            _inputManager.OnJump -= Jump;
    }

    public void Initialize()
    {
        _characterController = GetComponent<CharacterController>();
        _inputManager = InputManager.Instance;
        _inputManager.OnJump += Jump;
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _isGrounded = false;
            StartCoroutine(JumpCoroutine());
        }
    }

    private IEnumerator JumpCoroutine()
    {
        float verticalVelocity = Mathf.Sqrt(_jumpForce * CustomGravity.Value);

        while (verticalVelocity > 0)
        {
            _characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
            verticalVelocity -= CustomGravity.Value * Time.deltaTime;
            yield return null;
        }

        while (!_characterController.isGrounded)
        {
            yield return null;
        }

        _isGrounded = true;
    }
}