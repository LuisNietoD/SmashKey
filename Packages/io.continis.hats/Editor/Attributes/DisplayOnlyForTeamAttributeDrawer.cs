using System.Linq;
using Hats.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Hats.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(DisplayOnlyForTeamAttribute))]
    public class DisplayOnlyForTeamAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DisplayOnlyForTeamAttribute displayForAttribute = attribute as DisplayOnlyForTeamAttribute;

            if (displayForAttribute!.identifiers.Length == 0) return;
            
            if (displayForAttribute.identifiers.Any(Teams.IsTeamActive))
                EditorGUI.PropertyField(position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            DisplayOnlyForTeamAttribute displayForAttribute = attribute as DisplayOnlyForTeamAttribute;

            return displayForAttribute!.identifiers.Any(Teams.IsTeamActive)
                ? EditorGUI.GetPropertyHeight(property, label)
                : -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}