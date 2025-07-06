#if UNITY_EDITOR
using NamCore;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HexagonColorSetting))]
public class HexagonColorSettingDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Bỏ label cha
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none);

        // Chia đôi thành 2 phần
        float half = position.width / 2;
        Rect leftRect = new Rect(position.x, position.y, half - 5, position.height);
        Rect rightRect = new Rect(position.x + half + 5, position.y, half - 5, position.height);

        // Vẽ ColorID Enum và Color
        EditorGUI.PropertyField(leftRect, property.FindPropertyRelative("colorID"), GUIContent.none);
        EditorGUI.PropertyField(rightRect, property.FindPropertyRelative("color"), GUIContent.none);

        EditorGUI.EndProperty();
    }
}
#endif
