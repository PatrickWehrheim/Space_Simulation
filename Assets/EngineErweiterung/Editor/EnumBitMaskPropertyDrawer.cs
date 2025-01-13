using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BitMaskAttribute))]
public class EnumBitMaskPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        BitMaskAttribute typeAttribute = (BitMaskAttribute)attribute;

        if (typeAttribute == null)
        {
            return;
        }

        property.intValue = DrawBitMaskField(position, label, property.intValue, typeAttribute.PropType);
    }

    private int DrawBitMaskField(Rect position, GUIContent label, int mask, System.Type type)
    {
        string[] itemNames = System.Enum.GetNames(type);

        return EditorGUI.MaskField(position, label, mask, itemNames);
    }
}
