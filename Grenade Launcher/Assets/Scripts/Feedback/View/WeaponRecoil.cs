using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponRecoil : MonoBehaviour
{
    private const string ShootTriggerName = "Shoot";

    [SerializeField] private WeaponShoot _weaponShoot;

    private Animator _weaponAnimator;

    private void Awake()
    {
        _weaponAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _weaponShoot.OnShoot += HandleShoot;
    }

    private void OnDisable()
    {
        _weaponShoot.OnShoot -= HandleShoot;
    }

    private void HandleShoot()
    {
        _weaponAnimator.SetTrigger(ShootTriggerName);
    }
}