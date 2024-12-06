using UnityEditor;
using UnityEngine;

// Check this: https://support.unity.com/hc/en-us/articles/208298636-How-to-modify-the-Editor-Application-Layout-from-a-script

namespace Hats.Editor.Rules
{
    public class LoadLayout : RuleBase
    {
        public DefaultAsset layoutFile;
        
        public override string RuleName => "Load Layout";

        public override string RuleDescription =>
            "Loads a window layout when the Workspace becomes active. \n \nTo setup, save a window layout using Unity's menu in the top-right corner of the editor (or Window > Layouts > Save Layout to File...). Once the .wlt file has been saved, reference it in this rule.";

        public override bool OnBecameActive(Hats.ChangeReason reason)
        {
            if(layoutFile == null)
            {
                Debug.LogWarning("(Hats) No layout file has been assigned to the Load Layout rule for the current Workspace.");
                return false;
            }

            string assetPath = AssetDatabase.GetAssetPath(layoutFile);
            if (!assetPath.EndsWith(".wlt"))
            {
                Debug.LogWarning("(Hats) The file you are trying to load is not a valid Unity Layout file (.wlt).");
                return false;
            }
            
            EditorApplication.delayCall += () => EditorUtility.LoadWindowLayout(assetPath);
            // load an Overlay file
            
            return true;
        }

        public override bool OnBecameInactive() => true;
    }
}