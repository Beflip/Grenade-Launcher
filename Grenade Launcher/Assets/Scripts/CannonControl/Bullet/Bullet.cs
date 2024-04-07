using UnityEngine;

[RequireComponent(typeof(CustomRigidbody))]
[RequireComponent(typeof(RandomMeshGenerator))]
public class Bullet : MonoBehaviour
{
    private const int MaxCollisions = 2;

    private PaintBrush _paintBrush;
    private CustomRigidbody _customRigidbody;
    private RandomMeshGenerator _randomMeshGenerator;
    private Renderer _particleExplosion;
    private int _collisionCount = 0;

    private void Awake()
    {
        _customRigidbody = GetComponent<CustomRigidbody>();
        _randomMeshGenerator = GetComponent<RandomMeshGenerator>();
        _randomMeshGenerator.Initialize();
    }

    private void OnEnable()
    {
        _customRigidbody.OnCollisionEnter += OnBulletCollisionEnter;
        _randomMeshGenerator.Generate();
    }

    private void OnDisable()
    {
        _customRigidbody.OnCollisionEnter -= OnBulletCollisionEnter;
        _collisionCount = 0;

        if (_particleExplosion != null)
        {
            _particleExplosion.transform.position = transform.position;
            _particleExplosion.material = _randomMeshGenerator.Material;
            _particleExplosion.gameObject.SetActive(true);
        }
    }

    public void AddImpulse(float impulseMagnitude)
    {
        Vector3 impulseDirection = transform.forward.normalized;
        _customRigidbody.AddForce(impulseDirection * impulseMagnitude, ForceMode.Impulse);
    }

    public void SetParticleExplosion(ParticleSystem particleExplosion)
    {
        _particleExplosion = particleExplosion.GetComponent<Renderer>();
    }

    public void SetPaintBrush(PaintBrush paintBrush)
    {
        _paintBrush = paintBrush;
    }

    private void OnBulletCollisionEnter(RaycastHit raycastHit)
    {
        TexturePainter texturePainter = raycastHit.collider.GetComponent<TexturePainter>();

        _collisionCount++;

        if (texturePainter != null)
        {
            _paintBrush.SetBrushColor(_randomMeshGenerator.Color);
            _paintBrush.GenerateRandomRotationAngle();
            _paintBrush.GenerateRandomBrushScale();
            texturePainter.Paint(_paintBrush, raycastHit.point);
        }

        if (_collisionCount >= MaxCollisions)
        {
            gameObject.SetActive(false);
            return;
        }
    }
}