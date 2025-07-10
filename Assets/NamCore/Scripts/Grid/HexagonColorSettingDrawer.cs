#if UNITY_EDITOR
using NamCore;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HexagonColorSetting))]
public class HexagonColorSettingDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty colorPaletteProp = property.FindPropertyRelative("colorPalette");
        SerializedProperty colorIDProp = property.FindPropertyRelative("colorID");
        SerializedProperty actualColorProp = property.FindPropertyRelative("actualColor");

        float totalWidth = position.width;
        float fieldHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        Rect colorPaletteRect = new Rect(position.x, position.y, totalWidth, fieldHeight);
        EditorGUI.PropertyField(colorPaletteRect, colorPaletteProp, new GUIContent("Color Palette Ref"));

        Rect enumRect = new Rect(position.x, position.y + fieldHeight + spacing, totalWidth * 0.6f - spacing, fieldHeight);
        Rect colorDisplayRect = new Rect(position.x + totalWidth * 0.6f, position.y + fieldHeight + spacing, totalWidth * 0.4f, fieldHeight);

        EditorGUI.PropertyField(enumRect, colorIDProp, GUIContent.none);

        ColorPaletteSO colorPaletteSO = colorPaletteProp.objectReferenceValue as ColorPaletteSO;
        Color displayedColor = Color.white;

        if (colorPaletteSO != null)
        {
            ColorID selectedID = (ColorID)colorIDProp.enumValueIndex;
            displayedColor = colorPaletteSO.GetColorByID(selectedID);
        }
        else
        {
            GUI.color = Color.yellow;
            EditorGUI.LabelField(colorDisplayRect, "Assign Palette!");
            GUI.color = Color.white;
        }

        EditorGUI.DrawRect(colorDisplayRect, displayedColor);

        actualColorProp.colorValue = displayedColor;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (EditorGUIUtility.singleLineHeight * 2) + EditorGUIUtility.standardVerticalSpacing;
    }
}
#endif