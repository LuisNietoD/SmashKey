using UnityEditor;
using UnityEngine;

namespace Hats.Editor.Rules
{
    public class SceneCameraToPosition : IRule
    {
        public string RuleName => "Scene Camera To Position";
        
        public string RuleDescription => "Brings the Scene View camera to a specific position and rotation.";

        [SerializeField] private  Vector3 position;
        [SerializeField] private  Quaternion rotation = Quaternion.identity;
        [SerializeField] private float distance;
        
        public bool OnBecameActive(Hats.ChangeReason reason)
        {
            if (reason == Hats.ChangeReason.Recompile) return true;

            // Delay the call to ensure the SceneView is ready
            EditorApplication.delayCall += () =>
            {
                SceneView sceneView = SceneView.lastActiveSceneView;
                if (sceneView == null) return;

                sceneView.LookAt(position, rotation, distance);
            };

            return true;
        }

        public bool OnBecameInactive() => true;
    }
}