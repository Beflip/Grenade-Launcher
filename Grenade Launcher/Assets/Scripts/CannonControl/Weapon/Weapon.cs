using UnityEngine;

[RequireComponent(typeof(WeaponShoot))]
public class Weapon : MonoBehaviour
{
    private IShootable _shootable;

    private void Start()
    {
        _shootable = GetComponent<WeaponShoot>();
        _shootable.Initialize();
    }
}