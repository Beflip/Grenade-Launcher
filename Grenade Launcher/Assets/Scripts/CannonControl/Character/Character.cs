using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterRotator))]
[RequireComponent(typeof(CharacterJump))]
public class Character : MonoBehaviour
{
    private IMovable _movable;
    private IRotatable _rotatable;
    private IJumpable _jumpable;

    private void Start()
    {
        _movable = GetComponent<CharacterMovement>();
        _rotatable = GetComponent<CharacterRotator>();
        _jumpable = GetComponent<CharacterJump>();

        _movable.Initialize();
        _rotatable.Initialize();
        _jumpable.Initialize();
    }

    private void Update()
    {
        _movable.Move();
        _rotatable.Rotate();
    }
}