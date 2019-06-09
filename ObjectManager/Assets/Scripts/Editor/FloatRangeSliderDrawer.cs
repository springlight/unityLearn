using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(FloatRangeSliderAttribute))]
public class FloatRangeSliderDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int oriIndentLv = EditorGUI.indentLevel;
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        EditorGUI.indentLevel = 0;
        EditorGUI.BeginProperty(position, label, property);
        SerializedProperty minP = property.FindPropertyRelative("min");
        SerializedProperty maxP = property.FindPropertyRelative("max");
        float minValue = minP.floatValue;
        float maxValue = maxP.floatValue;
        float filedWidth = position.width / 4f -4f;
        float sliderWidth = position.width / 2f;

        position.width = filedWidth;
    
        minValue = EditorGUI.FloatField(position, minValue);
        position.x += filedWidth +4f ;
        position.width = sliderWidth;
        FloatRangeSliderAttribute limit = attribute as FloatRangeSliderAttribute;

        EditorGUI.MinMaxSlider(position,  ref minValue, ref maxValue, limit.Min, limit.Max);
        position.x += sliderWidth +4f;
        position.width = filedWidth;
        maxValue = EditorGUI.FloatField(position, maxValue);
        if (minValue < limit.Min)
        {
            minValue = limit.Min;
        }
        if (maxValue < minValue)
        {
            maxValue = minValue;
        }
        else if (maxValue > limit.Max)
        {
            maxValue = limit.Max;
        }
        minP.floatValue = minValue;
        maxP.floatValue = maxValue;
        EditorGUI.EndProperty();
        EditorGUI.indentLevel = oriIndentLv;
    }
}
