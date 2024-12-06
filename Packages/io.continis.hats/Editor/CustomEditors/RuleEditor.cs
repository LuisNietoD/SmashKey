using Hats.Editor.Rules;
using Hats.Editor.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hats.Editor.CustomEditors
{
    [CustomEditor(typeof(IRule), true)]
    public class RuleEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();
            
            MiniHelpBox helpBox = new(((IRule)serializedObject.targetObject).RuleDescription, HelpBoxMessageType.Info){name = "Explanations"};
            root.Add(helpBox);
            
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            
            return root;
        }
    }
}