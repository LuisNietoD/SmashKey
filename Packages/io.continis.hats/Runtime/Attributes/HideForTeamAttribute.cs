using UnityEngine;

namespace Hats.Runtime.Attributes
{
    public class HideForTeamAttribute : PropertyAttribute
    {
        public readonly string[] identifiers;

        public HideForTeamAttribute(params string[] identifiers)
        {
            this.identifiers = identifiers;
        }
    }
}