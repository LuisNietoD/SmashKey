using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable CS0414 // Field is assigned but its value is never used

namespace Hats.Editor.Rules
{
    public class OpenScenes : RuleBase
    {
        public List<SceneAsset> scenesToOpen;
        
        public override string RuleName => "Open Scenes";

        public override string RuleDescription =>
            "Open the specified scene(s) when this Workspace becomes active. The first scene becomes the active scene.";

        public override bool OnBecameActive(Hats.ChangeReason reason)
        {
            if (reason == Hats.ChangeReason.Recompile) return true;
            
            if(scenesToOpen.Count == 0)
            {
                Debug.LogWarning("(Hats) No scenes to open. This workspace rule couldn't execute.");
                return false;
            }
            
            // Offer to save changes if there are any
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                Debug.LogWarning("(Hats) Scenes couldn't be opened as a result of Workspace switching, " +
                                 "because there are unsaved changes.");
                return false;
            }
            
            // Check if the first scene is already open
            int i = 0;
            string firstScenePath = AssetDatabase.GetAssetPath(scenesToOpen[0]);
            if(SceneManager.GetActiveScene() == SceneManager.GetSceneByPath(firstScenePath)) i = 1;

            // Open all other scenes
            for (; i < scenesToOpen.Count; i++)
            {
                SceneAsset sceneAsset = scenesToOpen[i];
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneAsset),
                    i == 0 ? OpenSceneMode.Single : OpenSceneMode.Additive);
            }
            return true;
        }

        public override bool OnBecameInactive() => true;
    }
}