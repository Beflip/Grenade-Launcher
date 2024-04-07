using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomCollider))]
public class CustomColliderEditor : Editor
{
    private const float EDIT_ICON_HEIGHT_MULTIPLIER = 1.3f;
    private const float EDIT_ICON_WIDTH_MULTIPLIER = 1.9f;

    private readonly Color _green = new Color(0f, 0.9f, 0f, 1f);
    private readonly Color _lightGreen = new Color(0.5f, 1f, 0.5f, 1f);
    private readonly Color _darkGreen = new Color(0.1f, 0.5f, 0.1f, 1f);
    private readonly Color _transparentGreen = new Color(0.2f, 0.6f, 0.2f, 0.5f);
    private readonly Color _lightWhite = new Color(1f, 1f, 1f);
    private readonly Color _darkWhite = new Color(0.4f, 0.4f, 0.4f);

    private bool _buttonPressed = false;
    private Texture2D _editColliderIcon;
    private CustomCollider _customCollider;

    private void OnEnable()
    {
        _customCollider = (CustomCollider)target;
        _editColliderIcon = EditorGUIUtility.Load("Icons/EditCollider.png") as Texture2D;
    }

    private void OnSceneGUI()
    {
        if (_buttonPressed)
            DrawColliderHandles();

        DrawColliderDiscs();
        CheckForChanges();
    }

    private void DrawColliderHandles()
    {
        EditorGUI.BeginChangeCheck();
        float newRadius = Handles.RadiusHandle(Quaternion.identity, _customCollider.transform.TransformPoint(_customCollider.Center), _customCollider.Radius);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_customCollider, "Change Collider");
            _customCollider.SetRadius(newRadius);
        }
    }

    private void DrawColliderDiscs()
    {
        if (_buttonPressed)
            Handles.color = _customCollider.enabled ? _green : _darkGreen;
        else
            Handles.color = _customCollider.enabled ? _lightGreen : _transparentGreen;

        Vector3 center = _customCollider.transform.TransformPoint(_customCollider.Center);
        Handles.DrawWireDisc(center, _customCollider.transform.right, _customCollider.Radius);
        Handles.DrawWireDisc(center, _customCollider.transform.up, _customCollider.Radius);
        Handles.DrawWireDisc(center, _customCollider.transform.forward, _customCollider.Radius);
    }

    private void CheckForChanges()
    {
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI()
    {
        DrawEditColliderButton();
        GUILayout.Space(5);
        base.OnInspectorGUI();
    }

    private void DrawEditColliderButton()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Edit Collider", GUILayout.Width(EditorGUIUtility.labelWidth));

        Color buttonColor = _buttonPressed ? _darkWhite : _lightWhite;
        GUI.backgroundColor = buttonColor;

        if (GUILayout.Button(_editColliderIcon, GUILayout.Height(EditorGUIUtility.singleLineHeight * EDIT_ICON_HEIGHT_MULTIPLIER), GUILayout.Width(EditorGUIUtility.singleLineHeight * EDIT_ICON_WIDTH_MULTIPLIER)))
        {
            _buttonPressed = !_buttonPressed;
            SceneView.RepaintAll();
        }

        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndHorizontal();
    }
}
