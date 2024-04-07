using UnityEngine;

[System.Serializable]
public class PaintBrush
{
    [SerializeField] [Range(0, 1)] private float _brushScale;
    [SerializeField] private Texture _brushTexture;

    private float _rotationAngle;
    private Color _brushColor;
    private float _randomBrushScale;

    public float BrushScale => _brushScale * _randomBrushScale;
    public Texture BrushTexture => _brushTexture;
    public float RotationAngle => _rotationAngle;
    public Color BrushColor => _brushColor;

    public PaintBrush()
    {
        _brushScale = 0.5f;
        _brushTexture = null;
        _rotationAngle = 0f;
        _brushColor = new Color();
    }

    public PaintBrush(Texture texture, float scale, Color color)
    {
        _brushTexture = texture;
        _brushScale = Mathf.Clamp01(scale);
        _brushColor = color;
    }

    public void SetBrushColor(Color color)
    {
        _brushColor = color;
    }

    public void GenerateRandomRotationAngle()
    {
        _rotationAngle = Random.Range(0f, 360f);
    }

    public void GenerateRandomBrushScale()
    {
        _randomBrushScale = Random.Range(0.75f, 1.2f);
    }
}