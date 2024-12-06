using System.IO;
using Hats.Editor.Attributes;
using Hats.Editor.Rules;
using UnityEditor;
using UnityEngine;

namespace Hats.Editor
{
    public class SelectFolder : RuleBase
    {
        public override string RuleName => "Select Folder";
        
        public override string RuleDescription => "Selects a folder in the Project window.";

        [AssetsPath] public string folderPath;
        
        public override bool OnBecameActive(Hats.ChangeReason reason)
        {
            if (reason == Hats.ChangeReason.Recompile) return true;
            
            if(string.IsNullOrEmpty(folderPath))
            {
                Debug.LogWarning("(Hats) Select Folder: No folder path specified.");
                return false;
            }
            
            string actualPath = $"Assets/{folderPath}";
            
            if(!AssetDatabase.IsValidFolder(actualPath))
            {
                Debug.LogWarning($"(Hats) Select Folder: Folder path {actualPath} is not a folder.");
                return false;
            }
            
            Object folderObj = AssetDatabase.LoadMainAssetAtPath(actualPath);
            Selection.activeObject = folderObj;
            
            // TODO: Would love to avoid the flicker caused by the delay, but just opening the folder doesn't seem to work.
            EditorApplication.delayCall += () => AssetDatabase.OpenAsset(folderObj);
            
            return true;
        }

        public override bool OnBecameInactive() => true;
    }
}
