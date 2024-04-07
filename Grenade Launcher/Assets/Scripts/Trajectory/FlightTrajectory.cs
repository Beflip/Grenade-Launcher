using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FlightTrajectory : MonoBehaviour
{
    [SerializeField] private GameObject _landingPoint;
    [SerializeField] [Min(0)] private int _numPoints = 20;
    [SerializeField] [Min(0)] private float _timeStep = 1f;
    [SerializeField] private WeaponPower _weaponPower;
    [SerializeField] private LayerMask _collisionLayers;

    private InputManager _inputManager;
    private LineRenderer _lineRenderer;
    private TrajectoryCalculator _trajectoryCalculator;

    private void Start()
    {
        _inputManager = InputManager.Instance;
        _inputManager.OnShotPowerSliderChanged += SetShotPower;

        _lineRenderer = GetComponent<LineRenderer>();
        _trajectoryCalculator = new TrajectoryCalculator(transform, _landingPoint, _weaponPower.ShotPower, _numPoints, _timeStep, _collisionLayers);
    }

    private void OnEnable()
    {
        if(_inputManager != null)
            _inputManager.OnShotPowerSliderChanged += SetShotPower;
    }

    private void OnDisable()
    {
        _inputManager.OnShotPowerSliderChanged -= SetShotPower;
    }

    private void SetShotPower(float value)
    {
        _trajectoryCalculator.SetVelocity(value);
    }

    private void LateUpdate()
    {
        _trajectoryCalculator.DrawTrajectory(_lineRenderer);
    }
}