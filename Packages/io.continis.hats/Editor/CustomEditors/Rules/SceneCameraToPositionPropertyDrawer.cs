using Hats.Editor.Rules;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hats.Editor.CustomEditors.Rules
{
    [CustomPropertyDrawer(typeof(SceneCameraToPosition))]
    public class SceneCameraToPositionPropertyDrawer : RulePropertyDrawer
    {
        private Button _recordCameraButton;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement propertyElement = base.CreatePropertyGUI(property);

            _recordCameraButton = new Button(() => RecordCamera(property))
            {
                text = "Record Camera Position"
            };

            return propertyElement;
        }

        protected override void AddExtraElements(Foldout foldout)
        {
            foldout.Add(_recordCameraButton);
        }

        private void RecordCamera(SerializedProperty property)
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null) return;
            
            SerializedProperty positionProperty = property.FindPropertyRelative("position");
            SerializedProperty rotationProperty = property.FindPropertyRelative("rotation");
            SerializedProperty distanceProperty = property.FindPropertyRelative("distance");
            
            positionProperty.vector3Value = sceneView.camera.transform.position + sceneView.camera.transform.forward * sceneView.cameraDistance;
            rotationProperty.quaternionValue = sceneView.camera.transform.rotation;
            distanceProperty.floatValue = sceneView.cameraDistance * .5f;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}