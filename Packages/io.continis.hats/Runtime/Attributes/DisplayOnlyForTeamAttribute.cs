using UnityEngine;

namespace Hats.Runtime.Attributes
{
    public class DisplayOnlyForTeamAttribute : PropertyAttribute
    {
        public readonly string[] identifiers;

        public DisplayOnlyForTeamAttribute(params string[] identifiers)
        {
            this.identifiers = identifiers;
        }
    }
}