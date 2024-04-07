using UnityEngine;

public class CustomCollider : MonoBehaviour
{
    [SerializeField] private bool _isTrigger = false;
    [SerializeField] private CustomPhysicMaterial _material;
    [SerializeField] private Vector3 _center = Vector3.zero;
    [SerializeField] [Min(0)] private float _radius = 1f;

    private bool _collision = true;

    public bool IsTrigger => _isTrigger;
    public CustomPhysicMaterial Material => _material;
    public Vector3 Center => _center;
    public float Radius => _radius;
    public bool Collision => _collision;

    public void SetRadius(float radius)
    {
        if (radius >= 0)
            _radius = radius;
    }

    public void SetMaterial(CustomPhysicMaterial material)
    {
        _material = material;
    }

    public void SetCenter(Vector3 center)
    {
        _center = center;
    }

    private void OnEnable()
    {
        _collision = true;
    }

    private void OnDisable()
    {
        _collision = false;
    }

    public CollisionResult CheckCollision(Vector3 direction)
    {
        if (!_collision)
            return new CollisionResult(false);

        Vector3 rayOrigin = transform.position + _center;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, direction, out hit, _radius))
        {
            Vector3 collisionPoint = hit.point - direction * _radius - _center;
            return new CollisionResult(true, collisionPoint, hit);
        }

        return new CollisionResult(false);
    }
}