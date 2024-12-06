using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Hats.Editor.Rules
{
    public class HighlightGizmosMemory : ScriptableSingleton<HighlightGizmosMemory>
    {
        public List<bool> gizmoStates;
        public List<bool> iconStates;
    }
    
    public class HighlightGizmos : RuleBase
    {
        public override string RuleName => "Highlight Gizmos";
    
        public override string RuleDescription =>
            "Highlights Gizmos in the scene view for certain Component types, by hiding all other Gizmos.  \n \n" +
            "Add the Component type names to the list using their full type name without spaces " +
            "and without the assembly name. For example: 'ReflectionProbe', 'Light', 'AudioSource', etc.";

        public List<string> componentTypes;
        
        public override bool OnBecameActive(Hats.ChangeReason reason)
        {
            if(componentTypes?.Count == 0)
            {
                Debug.LogWarning("(Hats) No Component types have been assigned to the Highlight Gizmos rule for the current Workspace.");
                return false;
            }
            
            HighlightGizmosMemory.instance.gizmoStates = new List<bool>();
            HighlightGizmosMemory.instance.iconStates = new List<bool>();
            
            GizmoInfo[] info = GizmoUtility.GetGizmoInfo();
            foreach (GizmoInfo gizmoInfo in info)
            {
                // Save the current state of the gizmo
                HighlightGizmosMemory.instance.gizmoStates.Add(gizmoInfo.gizmoEnabled);
                HighlightGizmosMemory.instance.iconStates.Add(gizmoInfo.iconEnabled);
                
                bool shouldBeEnabled = componentTypes!.Contains(gizmoInfo.name);
                
                gizmoInfo.iconEnabled = shouldBeEnabled;
                gizmoInfo.gizmoEnabled = shouldBeEnabled;
                GizmoUtility.ApplyGizmoInfo(gizmoInfo, false);
                
                //Debug.Log("Set: " + gizmoInfo.name + " Enabled: " + gizmoInfo.gizmoEnabled);
            }
            
            //Debug.Log("Found " + info.Length + " Gizmos");
            // Debug.Log("States: " + HighlightGizmosMemory.instance.gizmoStates.Count);
            // Debug.Log("Icons: " + HighlightGizmosMemory.instance.iconStates.Count);
            
            return true;
        }
    
        public override bool OnBecameInactive()
        {
            if(componentTypes?.Count == 0) return false;
            if(HighlightGizmosMemory.instance.gizmoStates?.Count == 0) return false;
            if(HighlightGizmosMemory.instance.iconStates?.Count == 0) return false;
            
            GizmoInfo[] info = GizmoUtility.GetGizmoInfo();
            for (int i = 0; i < info.Length; i++)
            {
                GizmoInfo gizmoInfo = info[i];
                
                // This defaults to true if previousState is not found
                bool previousIconState = HighlightGizmosMemory.instance.iconStates!.Count <= i ||
                                     HighlightGizmosMemory.instance.iconStates[i];
                
                bool previousGizmoState = HighlightGizmosMemory.instance.gizmoStates!.Count <= i ||
                                         HighlightGizmosMemory.instance.gizmoStates[i];

                //Debug.Log("Reset: " + gizmoInfo.name + " Gizmo Enabled: " + previousGizmoState + " Icon Enabled: " + previousIconState);
                
                gizmoInfo.iconEnabled = previousIconState;
                gizmoInfo.gizmoEnabled = previousGizmoState;
                
                GizmoUtility.ApplyGizmoInfo(gizmoInfo, false);
            }
            
            // Debug.Log("Found " + info.Length + " Gizmos");
            // Debug.Log("States: " + HighlightGizmosMemory.instance.gizmoStates.Count);
            // Debug.Log("Icons: " + HighlightGizmosMemory.instance.iconStates.Count);
            
            HighlightGizmosMemory.instance.gizmoStates = new List<bool>();
            HighlightGizmosMemory.instance.iconStates = new List<bool>();

            return true;
        }
    }
}