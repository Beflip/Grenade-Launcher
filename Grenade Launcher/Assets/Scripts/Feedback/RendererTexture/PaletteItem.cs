using UnityEngine;

public class PaletteItem
{
    private Material _material;
    private Texture _texture;
    private RenderTexture _renderTexture;
    private int _index;

    public PaletteItem(Material material)
    {
        _material = material;
        _texture = null;
        _renderTexture = null;
        _index = 0;
    }

    public Material Material
    {
        get { return _material; }
        set { _material = value; }
    }

    public Texture Texture
    {
        get { return _texture; }
        set { _texture = value; }
    }

    public RenderTexture RenderTexture
    {
        get { return _renderTexture; }
        set { _renderTexture = value; }
    }

    public int Index
    {
        get { return _index; }
        set { _index = value; }
    }
}