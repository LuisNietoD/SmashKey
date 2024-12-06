using Hats.Editor.Rules;
using UnityEngine;

namespace Hats.Samples.CustomRule
{
    public class DebugLogRule : RuleBase
    {
        // This appears in the Rule Property Drawer
        public override string RuleName => "Debug Log Rule";
        
        // This appears when hovering over the ? icon
        public override string RuleDescription => "This rule logs a message to the console.";
        
        public override bool OnBecameActive(Hats.Editor.Hats.ChangeReason reason)
        {
            // Sometimes it makes sense not to execute a rule again when code is recompiled.
            // Some other times, it's necessary to re-execute the rule because code compilation undid it.
            // The reason can be used to determine if the rule should be executed again.
            if (reason == Hats.Editor.Hats.ChangeReason.Recompile)
            {
                // An early exit should still return true.
                return true;
            }
            
            // This is the actual rule logic
            Debug.Log("Debug Log Rule became active.");
            
            // Returning true means the rule was successfully activated
            // Returning false means the rule failed somehow, so following rules shouldn't be executed
            return true;
        }

        public override bool OnBecameInactive()
        {
            Debug.Log("Debug Log Rule became inactive.");

            return true;
        }
    }
}