using UnityEngine;

[CreateAssetMenu(fileName = "CustomPhysicMaterial", menuName = "Physics/CustomPhysicMaterial")]
public class CustomPhysicMaterial : ScriptableObject
{
    [SerializeField] [Min(0)] private float _dynamicFriction = 0.6f;
    [SerializeField] [Min(0)] private float _staticFriction = 0.6f;
    [SerializeField] [Range(0f, 1f)] private float _bounciness = 0f;

    public float DynamicFriction => _dynamicFriction;
    public float StaticFriction => _staticFriction;
    public float Bounciness => _bounciness;
}