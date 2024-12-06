using System.Collections.Generic;
using Hats.Editor.Rules;
using UnityEngine;

namespace Hats.Editor
{
    [CreateAssetMenu(fileName = "New Workspace", menuName = "Hats/Workspace")]
    public class Workspace : ScriptableObject
    {
        public string identifier;
        public Texture2D icon;
        public Color color;
        [SerializeReference] public List<IRule> rules;
        
        public bool HasRules => rules is { Count: > 0 };
        
        public void ExecuteRules(bool becomingActive, Hats.ChangeReason reason)
        {
            foreach (IRule rule in rules)
            {
                if (rule == null)
                {
                    Debug.LogWarning($"(Hats) Workspace {name} references a Null rule.", this);
                    continue;
                }

                if (becomingActive ? !rule.OnBecameActive(reason) : !rule.OnBecameInactive())
                {
                    Debug.LogWarning($"(Hats) Rule execution for Workspace {name} was interrupted by rule {rule.RuleName}. " +
                                   "Check its configuration and ensure all parameters are correctly setup.", this);
                    break;
                }
            }
        }
    }
}