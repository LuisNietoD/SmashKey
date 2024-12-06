using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
#pragma warning disable CS0414 // Field is assigned but its value is never used

namespace Hats.Editor.Rules
{
    public class LockObjectsWithTag : RuleBase
    {
        public string tag;
        public bool disablePicking = true;
        public bool hide = false;
        
        public override string RuleName => "Lock Objects with Tag";
        
        public override string RuleDescription =>
            "Disable picking and/or visibility on GameObjects with the specified Tag," +
            " when this Workspace becomes active. Note: disabled GameObjects will not be affected.";
        
        public override bool OnBecameActive(Hats.ChangeReason reason)
        {
            if(reason != Hats.ChangeReason.Recompile && 
               !LockObjects())
            {
                Debug.LogWarning("(Hats) Tag is empty. This Workspace rule couldn't execute.");
                return false;
            }
            
            EditorSceneManager.sceneOpened += LockObjectsInOpenedScene;
            
            return true;
        }

        private void LockObjectsInOpenedScene(Scene scene, OpenSceneMode mode) => LockObjects();

        private bool LockObjects()
        {
            if(string.IsNullOrEmpty(tag)) return false;
            
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag(tag))
            {
                if(hide) SceneVisibilityManager.instance.Hide(gameObject, true);
                if(disablePicking) SceneVisibilityManager.instance.DisablePicking(gameObject, true);
            }
            return true;
        }

        public override bool OnBecameInactive()
        {
            if(string.IsNullOrEmpty(tag))
            {
                Debug.LogWarning("(Hats) Tag is empty. This Workspace rule couldn't execute.");
                return false;
            }
            
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag(tag))
            {
                if(hide) SceneVisibilityManager.instance.Show(gameObject, true);
                if(disablePicking) SceneVisibilityManager.instance.EnablePicking(gameObject, true);
            }
            
            EditorSceneManager.sceneOpened -= LockObjectsInOpenedScene;
            
            return true;
        }
    }
}