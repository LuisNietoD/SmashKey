using Hats.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hats.Samples.CustomInspector.Editor
{
    [CustomEditor(typeof(TeamDependentComponent))]
    public class TeamDependentComponentEditor : UnityEditor.Editor
    {
        // Needs to be assigned on the .cs file
        [SerializeField] private Team _team;
        
        private VisualElement _inspector;

        private void OnEnable() => Teams.TeamChanged += OnTeamChanged;
        private void OnDisable() => Teams.TeamChanged -= OnTeamChanged;

        public override VisualElement CreateInspectorGUI()
        {
            _inspector = new VisualElement();
            SetupInspector();
            return _inspector;
        }

        private void SetupInspector()
        {
            HelpBox helpBox;
            
            if (_team == null)
            {
                helpBox = new HelpBox("Please assign a Team on the .cs file of the CustomInspector for this script.",
                    HelpBoxMessageType.Warning);
                _inspector.Add(helpBox);
                return;
            }

            if (Teams.IsTeamActive(_team))
                helpBox = new HelpBox($"This is what the {_team.identifier} team sees.", HelpBoxMessageType.Info);
            else
                helpBox = new HelpBox("You see this message when you're not part of a Team.", HelpBoxMessageType.Info);
            
            _inspector.Add(helpBox);
        }
        
        private void OnTeamChanged(int newTeamIndex)
        {
            _inspector.Clear();
            SetupInspector();
        }
    }
}