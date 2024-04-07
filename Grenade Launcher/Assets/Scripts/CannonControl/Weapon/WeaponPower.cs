using UnityEngine;

public class WeaponPower : MonoBehaviour, IPowerable
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] [Range(10f, 40f)] private float _shotPower = 20f;

    public Transform FirePoint => _firePoint;
    public float ShotPower => _shotPower;

    public void SetPower(float power)
    {
        _shotPower = power;
    }
}