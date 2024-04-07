using UnityEngine;
using System;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] [Range(0f, 5000f)] private float _sensitivity = 2000f;

    [Header("Input")]
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private TouchField _touchField;
    [SerializeField] private ExtendedButton _jumpButton;
    [SerializeField] private ExtendedButton _shootButton;
    [SerializeField] private Slider _shotPowerSlider;
    [SerializeField] private Slider _weaponHeightAngleSlider;

    public event Action<float> OnShotPowerSliderChanged;
    public event Action<float> OnWeaponHeightAngleSliderChanged;

    public event Action OnJump;
    public event Action OnShoot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        _jumpButton.OnButtonDownEvent += PerformJumpAction;
        _shootButton.OnButtonHoldEvent += PerformShootAction;
        _shotPowerSlider.onValueChanged.AddListener(OnShotPowerSliderValueChanged);
        _weaponHeightAngleSlider.onValueChanged.AddListener(OnWeaponHeightAngleSliderValueChanged);
    }

    private void OnDisable()
    {
        _jumpButton.OnButtonDownEvent -= PerformJumpAction;
        _shootButton.OnButtonHoldEvent -= PerformShootAction;
        _shotPowerSlider.onValueChanged.RemoveListener(OnShotPowerSliderValueChanged);
        _weaponHeightAngleSlider.onValueChanged.RemoveListener(OnWeaponHeightAngleSliderValueChanged);
    }

    private void PerformJumpAction()
    {
        OnJump?.Invoke();
    }

    private void PerformShootAction()
    {
        OnShoot?.Invoke();
    }

    public void SetShotPower(float value)
    {
        if (_shotPowerSlider != null)
            _shotPowerSlider.value = value;
    }

    public void SetWeaponHeightAngle(float value)
    {
        if (_weaponHeightAngleSlider != null)
            _weaponHeightAngleSlider.value = value;
    }

    public Vector2 GetMovementInput()
    {
        return new Vector2(_joystick.Horizontal, _joystick.Vertical);
    }

    public Vector2 GetRotationInput()
    {
        return _touchField.TouchDistance * _sensitivity;
    }

    private void OnShotPowerSliderValueChanged(float value)
    {
        OnShotPowerSliderChanged?.Invoke(value);
    }

    private void OnWeaponHeightAngleSliderValueChanged(float value)
    {
        OnWeaponHeightAngleSliderChanged?.Invoke(value);
    }
}