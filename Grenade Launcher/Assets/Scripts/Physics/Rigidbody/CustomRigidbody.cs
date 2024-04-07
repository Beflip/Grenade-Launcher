using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CustomCollider))]
public class CustomRigidbody : MonoBehaviour
{
    private const float MinCollisionVelocity = 2f;
    private const float DefaultBounciness = 0f;

    [SerializeField] [Min(0)] private float _mass = 1f;
    [SerializeField] [Min(0)] private float _drag = 0f;
    [SerializeField] [Min(0)] private float _angularDrag = 0.05f;
    [SerializeField] private bool _useGravity = true;
    [SerializeField] private bool _isKinematic = false;
    [SerializeField] private UpdateType _updateType = UpdateType.FixedUpdate;
    [SerializeField] private Constraints _constraints;

    private CustomCollider _customCollider;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _angularVelocity = Vector3.zero;

    public event Action<RaycastHit> OnCollisionEnter;

    public float Mass => _mass;
    public float Drag => _drag;
    public float AngularDrag => _angularDrag;
    public bool UseGravity => _useGravity;
    public bool IsKinematic => _isKinematic;
    public UpdateType RigidbodyUpdateType => _updateType;
    public Constraints Constraints => _constraints;

    private void Awake()
    {
        _customCollider = GetComponent<CustomCollider>();
        _constraints = new Constraints();
    }

    private void FixedUpdate()
    {
        if (_updateType == UpdateType.FixedUpdate)
            UpdateRigidbody();
    }

    private void Update()
    {
        if (_updateType == UpdateType.Update)
            UpdateRigidbody();
    }

    private void LateUpdate()
    {
        if (_updateType == UpdateType.LateUpdate)
            UpdateRigidbody();
    }

    public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
    {
        switch (mode)
        {
            case ForceMode.Force:
                _velocity += force / _mass;
                break;
            case ForceMode.Acceleration:
                _velocity += force * Time.deltaTime;
                break;
            case ForceMode.Impulse:
                _velocity += force / _mass;
                break;
            case ForceMode.VelocityChange:
                _velocity += force;
                break;
        }
    }

    private void UpdateRigidbody()
    {
        if (IsDynamic())
        {
            ApplyGravity();
            ApplyDrag();
            ApplyAngularDrag();
            CheckCollision();
            ApplyMovement();
            ApplyRotation();
        }
    }

    private void OnDisable()
    {
        _velocity = Vector3.zero;
        _angularVelocity = Vector3.zero;
    }

    private bool IsDynamic()
    {
        if (_isKinematic)
        {
            _velocity = Vector3.zero;
            _angularVelocity = Vector3.zero;
            return false;
        }
        return true;
    }

    private void CheckCollision()
    {
        if (_customCollider == null)
            return;
        else if (_customCollider.Collision == false)
            return;

        Vector3 movementDirection = _velocity.normalized;
        CollisionResult collisionResult = _customCollider.CheckCollision(movementDirection);

        CustomPhysicMaterial physicMaterial = _customCollider.Material;
        float bounciness = physicMaterial != null ? physicMaterial.Bounciness : DefaultBounciness;

        if (collisionResult.HasCollision)
        {
            OnCollisionEnter?.Invoke(collisionResult.RaycastHit);

            if (!_customCollider.IsTrigger)
            {
                if (_velocity.magnitude > MinCollisionVelocity)
                {
                    Vector3 rotationDirection = Vector3.Cross(-_velocity, collisionResult.RaycastHit.normal);
                    float rotationSpeed = _velocity.magnitude;

                    _velocity = Vector3.Reflect(_velocity, collisionResult.RaycastHit.normal) * bounciness;
                    _angularVelocity = rotationDirection * rotationSpeed;
                }
                else
                {
                    _velocity = Vector3.zero;
                    _angularVelocity = Vector3.zero;
                    transform.position = collisionResult.CollisionPosition;
                }
            }
        }
    }

    private void ApplyGravity()
    {
        if (_useGravity)
        {
            float gravity = CustomGravity.Value;
            _velocity += Vector3.down * gravity * Time.deltaTime;
        }
    }

    private void ApplyDrag()
    {
        if (_drag > 0)
            _velocity -= _velocity * _drag * Time.deltaTime;
    }

    private void ApplyAngularDrag()
    {
        if (_angularDrag > 0)
            _angularVelocity -= _angularVelocity * _angularDrag * Time.deltaTime;
    }

    private Vector3 ApplyPositionConstraints(Vector3 velocity)
    {
        if (_constraints.FreezePosition.X)
            velocity.x = 0f;

        if (_constraints.FreezePosition.Y)
            velocity.y = 0f;

        if (_constraints.FreezePosition.Z)
            velocity.z = 0f;

        return velocity;
    }

    private void ApplyMovement()
    {
        _velocity = ApplyPositionConstraints(_velocity);
        transform.position += _velocity * Time.deltaTime;
    }

    private Quaternion ApplyRotationConstraints(Quaternion rotation)
    {
        Vector3 constrainedEulerAngles = rotation.eulerAngles;

        if (_constraints.FreezeRotation.X)
            constrainedEulerAngles.x = transform.localRotation.eulerAngles.x;

        if (_constraints.FreezeRotation.Y)
            constrainedEulerAngles.y = transform.localRotation.eulerAngles.y;

        if (_constraints.FreezeRotation.Z)
            constrainedEulerAngles.z = transform.localRotation.eulerAngles.z;

        return Quaternion.Euler(constrainedEulerAngles);
    }

    private void ApplyRotation()
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion deltaRotation = Quaternion.AngleAxis(_angularVelocity.magnitude * Time.deltaTime, _angularVelocity.normalized);
        Quaternion newRotation = deltaRotation * currentRotation;
        Quaternion constrainedRotation = ApplyRotationConstraints(newRotation);
        transform.rotation = constrainedRotation;
    }
}