using Hats.Editor.Rules;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hats.Editor.CustomEditors
{
    [CustomPropertyDrawer(typeof(IRule), true)]
    public class RulePropertyDrawer : PropertyDrawer
    {
        private VisualElement _propertyDrawer;
        private VisualElement _objectLine;
        private PropertyField _propertyField;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            _propertyDrawer = new VisualElement {style = { marginBottom = 3 }};

            _objectLine = new VisualElement { style = { flexDirection = FlexDirection.Row, flexGrow = 1}};
            _propertyDrawer.Add(_objectLine);

            IRule rule = (IRule)property.boxedValue;
            
            _propertyField = new(property) { label = rule.RuleName, style = { flexGrow = 1}};
            _propertyField.RegisterCallback<GeometryChangedEvent>(PropertyFieldReady);
            _propertyField.BindProperty(property);
            _propertyField.AddToClassList("unity-property-field__inspector-field");
            _propertyField.AddToClassList("unity-property-field__aligned");
            _objectLine.Add(_propertyField);
            
            // Question mark icon with explanation tooltip
            VisualElement questionMark = new() { name = "QuestionMark", style = { width = 18, height = 18, marginTop = 1, marginLeft = -14, marginRight = 10, alignSelf = Align.FlexStart} };
            questionMark.Add(new Image { image = EditorGUIUtility.IconContent("_Help").image });
            questionMark.tooltip = rule.RuleDescription;
            _objectLine.Insert(0, questionMark);

            return _propertyDrawer;
        }

        private void PropertyFieldReady(GeometryChangedEvent evt)
        {
            _propertyField.UnregisterCallback<GeometryChangedEvent>(PropertyFieldReady);
            
            Foldout foldout = ((VisualElement)evt.target).Q<Foldout>();
            if(foldout == null) return;
            
            foldout.style.marginTop = 0; 
            EditorApplication.delayCall += () => AddExtraElements(foldout);
        }
        
        protected virtual void AddExtraElements(Foldout foldout) { }
    }
}