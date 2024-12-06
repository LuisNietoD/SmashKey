using System.Linq;
using Hats.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Hats.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(HideForTeamAttribute))]
    public class HideForTeamAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            HideForTeamAttribute hideForTeamAttribute = attribute as HideForTeamAttribute;

            if (hideForTeamAttribute!.identifiers.Any(Teams.IsTeamActive)) return;
            EditorGUI.PropertyField(position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            HideForTeamAttribute hideForTeamAttribute = attribute as HideForTeamAttribute;

            return hideForTeamAttribute!.identifiers.Any(Teams.IsTeamActive)
                ? -EditorGUIUtility.standardVerticalSpacing
                : EditorGUI.GetPropertyHeight(property, label);
        }
    }
}