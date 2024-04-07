using UnityEngine;

public struct CollisionResult
{
    private bool _hasCollision;
    private Vector3 _collisionPosition;
    private RaycastHit _raycastHit;

    public bool HasCollision => _hasCollision;
    public Vector3 CollisionPosition => _collisionPosition;
    public RaycastHit RaycastHit => _raycastHit;

    public CollisionResult(bool hasCollision, Vector3 collisionPosition, RaycastHit raycastHit)
    {
        _hasCollision = hasCollision;
        _collisionPosition = collisionPosition;
        _raycastHit = raycastHit;
    }

    public CollisionResult(bool hasCollision)
    {
        _hasCollision = hasCollision;
        _collisionPosition = Vector3.zero;
        _raycastHit = default(RaycastHit);
    }
}