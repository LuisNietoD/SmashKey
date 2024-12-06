using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hats.Editor.CustomEditors
{
    [CustomEditor(typeof(Team))]
    public class TeamEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _template;
        
        private ListView _rulesField;
        private Button _joinButton;
        private Foldout _foldout;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement inspector = _template.Instantiate();
            
            _foldout = inspector.Q<Foldout>("TeamFoldout");
            _foldout.text = ((Team)target).identifier;
            
            _joinButton = inspector.Q<Button>("JoinButton");
            _joinButton.clicked += OnJoinButtonClicked;
            
            TextField identifierField = inspector.Q<TextField>("IdentifierField");
            identifierField.RegisterValueChangedCallback(evt => _foldout.text = evt.newValue);
            
            Teams.TeamChanged += OnTeamChanged;
            if(Teams.IsUserNameReady)
                InitialiseJoinButton(CloudProjectSettings.userName);
            else
                Teams.UserNameReady += InitialiseJoinButton;
            
            return inspector;
        }

        private void OnJoinButtonClicked()
        {
            Teams.JoinTeam((Team)target);
        }

        private void OnTeamChanged(int teamIndex)
        {
            bool inTeam = teamIndex != -1 && (Team)target == Teams.TeamsCatalog.teamsList[teamIndex];
            SetJoinButton(inTeam);
        }

        private void InitialiseJoinButton(string userName)
        {
            bool inTeam = Teams.IsUserPartOfTeam(userName, (Team)target);
            SetJoinButton(inTeam);
        }

        private void SetJoinButton(bool inTeam)
        {
            _joinButton.text = inTeam ? "Your Team" : "Join Team";
            _joinButton.SetEnabled(!inTeam);
        }
    }
}