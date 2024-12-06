using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hats.Editor.Attributes
{
    public class AssetsPathAttribute : PropertyAttribute { }
    
    [CustomPropertyDrawer(typeof(AssetsPathAttribute))]
    public class AssetsPathAttributeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            TextField textField = new(property.displayName);
            textField.BindProperty(property);
            
            Label assetsPrefix = new("Assets/") { style = { opacity = .5f, unityTextAlign = TextAnchor.MiddleCenter } };
            textField.Insert(1, assetsPrefix);
            
            return textField;
        }
    }
}