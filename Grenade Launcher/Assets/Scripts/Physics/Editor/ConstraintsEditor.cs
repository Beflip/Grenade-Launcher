using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FreezePosition))]
[CustomPropertyDrawer(typeof(FreezeRotation))]
public class ConstraintsEditor : PropertyDrawer
{
    private const float FieldOffset = 17f;
    private const float LabelOffset = 13.5f;
    private const float LabelWidth = 30f;
    private const float PropertyFieldWidth = 10f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        Rect fieldRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        fieldRect.width = LabelWidth;

        EditorGUIUtility.labelWidth = PropertyFieldWidth;

        DrawPropertyFieldWithLabel(property.FindPropertyRelative("_x"), "X", ref fieldRect);
        DrawPropertyFieldWithLabel(property.FindPropertyRelative("_y"), "Y", ref fieldRect);
        DrawPropertyFieldWithLabel(property.FindPropertyRelative("_z"), "Z", ref fieldRect);

        EditorGUI.EndProperty();
    }

    private void DrawPropertyFieldWithLabel(SerializedProperty property, string labelText, ref Rect position)
    {
        EditorGUI.PropertyField(position, property, GUIContent.none);
        position.x += FieldOffset;
        EditorGUI.LabelField(position, new GUIContent(labelText));
        position.x += LabelOffset;
    }
}