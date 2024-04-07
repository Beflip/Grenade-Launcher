using UnityEngine;

public class WeaponHeight : MonoBehaviour, IHeightable
{
    [SerializeField] private Transform _weaponHeight;
    [SerializeField] [Range(0f, 45f)] private float _heightAngle = 0f;

    public float HeightAngle => _heightAngle;

    public void SetHeight(float height)
    {
        float weaponHeightAngle = height * -1f;
        _weaponHeight.localRotation = Quaternion.Euler(weaponHeightAngle, 0f, 0f);
    }

    public float GetHeight()
    {
        return _weaponHeight.localRotation.eulerAngles.x;
    }
}