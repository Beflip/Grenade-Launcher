using UnityEngine;

public class TrajectoryCalculator
{
    private Transform _transform;
    private GameObject _landingPoint;
    private float _velocity;
    private int _numPoints;
    private float _timeStep;
    private LayerMask _collisionLayers;

    private Color _activeColor = new Color(0.886f, 0.204f, 0.204f, 0.792f);
    private Color _inactiveColor = new Color(0.2f, 0.2f, 0.2f, 0.792f);

    public TrajectoryCalculator(Transform transform, GameObject landingPoint, float velocity, int numPoints, float timeStep, LayerMask collisionLayers)
    {
        _transform = transform;
        _landingPoint = landingPoint;
        _velocity = velocity;
        _numPoints = numPoints;
        _timeStep = timeStep;
        _collisionLayers = collisionLayers;
    }

    public void SetVelocity(float velocity)
    {
        _velocity = velocity;
    }

    public void DrawTrajectory(LineRenderer lineRenderer)
    {
        Vector3[] points = new Vector3[_numPoints];
        Vector3 currentVelocity = _transform.forward * _velocity;
        Vector3 currentPosition = _transform.position;

        points[0] = currentPosition;

        bool collided = false;

        for (int i = 1; i < _numPoints; i++)
        {
            float time = i * _timeStep / _velocity;
            points[i] = currentPosition + currentVelocity * time + 0.5f * Physics.gravity * time * time;

            if (!collided)
            {
                RaycastHit hit;
                if (Physics.Raycast(points[i - 1], points[i] - points[i - 1], out hit, Vector3.Distance(points[i - 1], points[i]), _collisionLayers)) // Используем LayerMask для определения слоев
                {
                    collided = true;
                    currentPosition = hit.point;
                    currentVelocity = Vector3.Reflect(currentVelocity, hit.normal);
                    points[i] = currentPosition;
                    _landingPoint.transform.position = currentPosition;
                    _landingPoint.SetActive(true);
                    lineRenderer.startColor = _activeColor;
                    lineRenderer.endColor = _activeColor;
                }
            }
            else
            {
                points[i] = points[i - 1];
            }
        }

        lineRenderer.positionCount = _numPoints;
        lineRenderer.SetPositions(points);

        if (!collided)
        {
            _landingPoint.SetActive(false);
            lineRenderer.startColor = _inactiveColor;
            lineRenderer.endColor = _inactiveColor;
        }
    }
}