using System;
using UnityEngine;

[RequireComponent(typeof(BulletPool))]
[RequireComponent(typeof(WeaponHeight))]
[RequireComponent(typeof(WeaponPower))]
public class WeaponShoot : MonoBehaviour, IShootable
{
    [SerializeField] [Range(0f, 10f)] private float _rateOfFire = 0.5f;

    private InputManager _inputManager;
    private WeaponPower _weaponPower;
    private WeaponHeight _weaponHeight;
    private BulletPool _bulletPool;
    private float _nextFireTime;
    private bool _isReloading = false;
    private bool _isAmmoEmpty = false;

    public Action OnShoot;

    private void OnDisable()
    {
        if (_inputManager != null)
        {
            _inputManager.OnShoot -= Shoot;
            _inputManager.OnShotPowerSliderChanged -= SetShotPower;
            _inputManager.OnWeaponHeightAngleSliderChanged -= SetWeaponHeightAngle;
        }
    }

    public void Initialize()
    {
        _bulletPool = GetComponent<BulletPool>();
        _weaponHeight = GetComponent<WeaponHeight>();
        _weaponPower = GetComponent<WeaponPower>();

        _inputManager = InputManager.Instance;
        _inputManager.OnShoot += Shoot;
        _inputManager.OnShotPowerSliderChanged += SetShotPower;
        _inputManager.OnWeaponHeightAngleSliderChanged += SetWeaponHeightAngle;

        SetWeaponHeightAngle(_weaponHeight.HeightAngle);

        _inputManager.SetShotPower(_weaponPower.ShotPower);
    }

    public void SetReloading(bool reloading)
    {
        _isReloading = reloading;
    }

    public void SetAmmoEmpty(bool ammoEmpty)
    {
        _isAmmoEmpty = ammoEmpty;
    }

    public void Shoot()
    {
        if (!_isReloading && !_isAmmoEmpty && Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + _rateOfFire;
            SpawnBullet();
            OnShoot?.Invoke();

            CinemachineShake.Instance.ShakeCamera(2f, 0.2f);
        }
    }

    private void SpawnBullet()
    {
        Bullet bullet = _bulletPool.GetObject();
        bullet.transform.position = _weaponPower.FirePoint.position;
        bullet.transform.rotation = _weaponPower.FirePoint.rotation;
        bullet.AddImpulse(_weaponPower.ShotPower);
        bullet.gameObject.SetActive(true);
    }

    private void SetShotPower(float value)
    {
        _weaponPower.SetPower(value);
    }

    private void SetWeaponHeightAngle(float value)
    {
        _weaponHeight.SetHeight(value);
    }
}