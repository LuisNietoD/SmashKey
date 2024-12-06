using System.Collections.Generic;
using UnityEngine;

namespace Hats.Editor
{
    public class Team : ScriptableObject
    {
        public string identifier;
        public List<string> members = new();
        public Workspace workspace; 

        private void OnValidate()
        {
            name = identifier;
        } 

        internal bool IsCurrentlyActive() => Teams.GetActiveTeam() == this;
    }
}