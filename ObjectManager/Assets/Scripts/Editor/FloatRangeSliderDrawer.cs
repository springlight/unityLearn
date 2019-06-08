using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(FloatRangeSliderAttribute))]
public class FloatRangeSliderDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        SerializedProperty minP = property.FindPropertyRelative("min");
        SerializedProperty maxP = property.FindPropertyRelative("max");
        EditorGUI.EndProperty();
    }
}
