using System.Collections.Generic;
using UnityEngine;

public class TexturePainter : MonoBehaviour, IPaintable
{
    private const string MainTexProperty = "_MainTex";
    private const string PaintMainMaterial = "PaintMain";

    private readonly int _paintUVPropertyID = Shader.PropertyToID("_PaintUV");
    private readonly int _brushTexturePropertyID = Shader.PropertyToID("_Brush");
    private readonly int _brushScalePropertyID = Shader.PropertyToID("_BrushScale");
    private readonly int _brushRotatePropertyID = Shader.PropertyToID("_BrushRotate");
    private readonly int _brushColorPropertyID = Shader.PropertyToID("_ControlColor");

    private List<PaletteItem> _paletteItems = new List<PaletteItem>();
    private Camera _renderCamera;
    private MeshUtility _meshUtility;
    private Material _paintMaterial;

    private void Start()
    {
        Initialize();
    }

    private void OnDestroy()
    {
        ReleaseRenderTexture();
    }

    public void Initialize()
    {
        InitializeVisualization();
        InitializePaintMaterial();
    }

    public void Paint(PaintBrush paintBrush, Vector3 position)
    {
        Vector3 localPosition = transform.InverseTransformPoint(position);
        Matrix4x4 modelViewProjectionMatrix = _renderCamera.projectionMatrix * _renderCamera.worldToCameraMatrix * transform.localToWorldMatrix;
        Vector2 uv;

        if (_meshUtility.MapLocalPointToUV(localPosition, modelViewProjectionMatrix, out uv))
            PaintUVDirectly(paintBrush, uv);
        else
            PaintNearestTriangleSurface(paintBrush, position);
    }

    private void PaintUVDirectly(PaintBrush paintBrush, Vector2 uv)
    {
        foreach (var paletteItem in _paletteItems)
        {
            var mainPaintConditions = paintBrush.BrushTexture != null && paletteItem.RenderTexture != null && paletteItem.RenderTexture.IsCreated();

            if (mainPaintConditions)
            {
                var mainPaintTextureBuffer = RenderTexture.GetTemporary(paletteItem.RenderTexture.width, paletteItem.RenderTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
                SetPaintMainData(paintBrush, uv);
                Graphics.Blit(paletteItem.RenderTexture, mainPaintTextureBuffer, _paintMaterial);
                Graphics.Blit(mainPaintTextureBuffer, paletteItem.RenderTexture);
                RenderTexture.ReleaseTemporary(mainPaintTextureBuffer);
            }
        }
    }

    private void PaintNearestTriangleSurface(PaintBrush paintBrush, Vector3 worldPos)
    {
        var localPosition = transform.worldToLocalMatrix.MultiplyPoint(worldPos);
        var nearestSurfacePoint = _meshUtility.FindNearestLocalSurfacePoint(localPosition);

        Paint(paintBrush, transform.localToWorldMatrix.MultiplyPoint(nearestSurfacePoint));
    }

    private void InitializeVisualization()
    {
        var meshFilter = GetComponent<MeshFilter>();
        var skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        if (meshFilter != null)
            _meshUtility = new MeshUtility(meshFilter.sharedMesh);
        else if (skinnedMeshRenderer != null)
            _meshUtility = new MeshUtility(skinnedMeshRenderer.sharedMesh);

        _renderCamera = Camera.main;
    }

    private void InitializePaintMaterial()
    {
        _paintMaterial = new Material(Resources.Load<Material>(PaintMainMaterial));
        var renderer = GetComponent<Renderer>();
        var materials = renderer.materials;

        foreach (var material in materials)
        {
            var paletteItem = new PaletteItem(material);
            paletteItem.Index = Shader.PropertyToID(MainTexProperty);

            if (paletteItem.Material.HasProperty(paletteItem.Index))
            {
                paletteItem.Texture = paletteItem.Material.GetTexture(paletteItem.Index);
                paletteItem.RenderTexture = SetupRenderTexture(paletteItem.Texture, paletteItem.Index, paletteItem.Material);
            }

            _paletteItems.Add(paletteItem);
        }
    }

    private void SetPaintMainData(PaintBrush brush, Vector2 uv)
    {
        _paintMaterial.SetVector(_paintUVPropertyID, uv);
        _paintMaterial.SetTexture(_brushTexturePropertyID, brush.BrushTexture);
        _paintMaterial.SetFloat(_brushScalePropertyID, brush.BrushScale);
        _paintMaterial.SetFloat(_brushRotatePropertyID, brush.RotationAngle);
        _paintMaterial.SetVector(_brushColorPropertyID, brush.BrushColor);

        foreach (var keyword in _paintMaterial.shaderKeywords)
            _paintMaterial.DisableKeyword(keyword);
    }

    private void ReleaseRenderTexture()
    {
        foreach (var paletteItem in _paletteItems)
        {
            if (paletteItem.RenderTexture != null && paletteItem.RenderTexture.IsCreated())
                paletteItem.RenderTexture.Release();
        }
    }

    private RenderTexture SetupRenderTexture(Texture baseTexture, int propertyID, Material material)
    {
        var renderTexture = new RenderTexture(baseTexture.width, baseTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        renderTexture.filterMode = baseTexture.filterMode;
        Graphics.Blit(baseTexture, renderTexture);
        material.SetTexture(propertyID, renderTexture);
        return renderTexture;
    }
}