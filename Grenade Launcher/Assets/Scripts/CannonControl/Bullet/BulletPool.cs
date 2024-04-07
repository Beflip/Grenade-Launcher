using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private CustomPhysicMaterial _physicMaterial;
    [SerializeField] private ParticleSystem _particleExplosion;
    [SerializeField] private PaintBrush _paintBrush;

    private List<Bullet> _poolObjects = new List<Bullet>();

    public Bullet GetObject()
    {
        foreach (Bullet bullet in _poolObjects)
        {
            if (!bullet.gameObject.activeInHierarchy)
                return bullet;
        }

        Bullet newBullet = CreateBullet();
        _poolObjects.Add(newBullet);

        return newBullet;
    }

    private Bullet CreateBullet()
    {
        GameObject bulletObject = new GameObject("Bullet");
        ParticleSystem particleExplosion = Instantiate(_particleExplosion);
        particleExplosion.gameObject.SetActive(false);

        if (_physicMaterial != null)
        {
            CustomCollider customCollider = bulletObject.AddComponent<CustomCollider>();
            customCollider.SetMaterial(_physicMaterial);
        }

        Bullet bullet = bulletObject.AddComponent<Bullet>();
        bullet.SetParticleExplosion(particleExplosion);
        bullet.SetPaintBrush(_paintBrush);

        return bullet;
    }
}