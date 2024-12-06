using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hats.Editor.Utilities;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace Hats.Editor
{
    public static class Teams
    {
        public static event Action<string> UserNameReady;
        public static event Action TeamsInitialised;
        public static event Action<int> TeamChanged;
        public static event Action TeamCatalogChanged;
        
        public static bool IsUserNameReady => !string.IsNullOrEmpty(_username);
        
        private static TeamsCatalog _catalog;
        private static string _username;
        private static int _activeTeamIndex = -1;

        internal static TeamsCatalog TeamsCatalog
        {
            get
            {
                if(_catalog == null) SetupTeamsCatalog();
                if(_catalog.teamsList.Count == 0) _catalog.AddTeam();
                return _catalog;
            }
        }
        
        internal static void Initialise()
        {
            SetupTeamsCatalog();
            EditorCoroutineUtility.StartCoroutineOwnerless(WaitUntilUserReady());
        }

        private static IEnumerator WaitUntilUserReady()
        {
            float time = 0f;
            float maxTime = 3f;
            
            while (string.IsNullOrEmpty(CloudProjectSettings.userName) ||
                     CloudProjectSettings.userName == "anonymous")
            {
                time += 0.02f;
                if (time < maxTime)
                {
                    yield return new EditorWaitForSeconds(0.02f);
                }
                else
                {
                    Debug.LogError("(Hats) It was impossible to fetch the Unity ID username. Ensure you're logged in, and restart the Editor.");
                    yield break;
                }
            }
            
            _username = CloudProjectSettings.userName;
            HatsMemory.instance.userName = _username;
            UserNameReady?.Invoke(_username);
            
            int newTeamIndex = FindUsersTeam(_username);
            ChangeTeam(newTeamIndex);
            
            TeamsInitialised?.Invoke();
        }

        private static void ChangeTeam(int newIndex, Hats.ChangeReason reason = Hats.ChangeReason.UserAction)
        {
            //Debug.Log("Changing Teams to index " + newIndex);
            Team team;
            
            // Execute rules for Team about to be made inactive
            if (_activeTeamIndex != -1)
            {
                team = TeamsCatalog.teamsList[_activeTeamIndex];
                if (team != null && team.workspace != null && team.workspace.HasRules)
                {
                    team.workspace.ExecuteRules(false, reason);
                }
            }
            
            _activeTeamIndex = newIndex;
            HatsMemory.instance.activeTeamIndex = _activeTeamIndex;
            
            // Execute rules for Team becoming active
            if (_activeTeamIndex != -1)
            {
                team = TeamsCatalog.teamsList[_activeTeamIndex];
                if (team != null && team.workspace != null && team.workspace.HasRules)
                {
                    team.workspace.ExecuteRules(true, reason);
                }
            }
            
            TeamChanged?.Invoke(_activeTeamIndex);
        }

        internal static void AfterRecompile() 
        {
            _username = HatsMemory.instance.userName;
            _activeTeamIndex = HatsMemory.instance.activeTeamIndex;
            
            ChangeTeam(_activeTeamIndex, Hats.ChangeReason.Recompile); 
            
            UserNameReady?.Invoke(_username);
            TeamsInitialised?.Invoke();
        }
        
        private static void SetupTeamsCatalog()
        {
            string[] catalogAssetsFound = AssetDatabase.FindAssets($"t:{nameof(TeamsCatalog)}");
            if (catalogAssetsFound.Length == 0)
            {
                if (!Directory.Exists(Constants.CatalogFolderPath)) Directory.CreateDirectory(Constants.CatalogFolderPath);
                _catalog = ScriptableObject.CreateInstance<TeamsCatalog>();
                AssetDatabase.CreateAsset(_catalog, Path.Combine(Constants.CatalogFolderPath, $"{nameof(TeamsCatalog)}.asset"));
                AssetDatabase.SaveAssets();
            }
            else
            {
                string path = AssetDatabase.GUIDToAssetPath(catalogAssetsFound[0]);
                _catalog = AssetDatabase.LoadAssetAtPath<TeamsCatalog>(path);
            }
            
            TeamCatalogChanged?.Invoke();
        }
        
        internal static void InvokeTeamCatalogChanged() => TeamCatalogChanged?.Invoke();
        
        internal static bool IsUserPartOfTeam(string userName, Team team)
        {
            return team != null && team.members.Contains(userName);
        }

        private static int FindUsersTeam(string userName)
        {
            return _catalog.teamsList.FindIndex(team => IsUserPartOfTeam(userName, team));
        }
        
#region Public Methods

        public static string GetActiveTeamIdentifier()
        {
            return GetActiveTeam() == null ? Constants.NoTeamIdentifier : GetActiveTeam()?.identifier;
        }
        
        public static int GetActiveTeamIndex() => _activeTeamIndex;

        public static Team GetActiveTeam()
        {
            if (_activeTeamIndex < 0 || _activeTeamIndex >= TeamsCatalog.teamsList.Count) return null;
            return TeamsCatalog.teamsList[_activeTeamIndex];    
        }

        public static bool IsTeamActive(Team team) => team != null && team.IsCurrentlyActive();

        public static bool IsTeamActive(string teamIdentifier) => GetActiveTeam()?.identifier == teamIdentifier;

        public static void LeaveAllTeams()
        {
            int oldTeamIndex = FindUsersTeam(_username);
            if (oldTeamIndex != -1)
            {
                Team oldTeam = TeamsCatalog.teamsList[oldTeamIndex];
                oldTeam.members.Remove(_username);
                EditorUtility.SetDirty(oldTeam);
                AssetDatabase.SaveAssets();
            }
            
            ChangeTeam(-1);
        }
        
        public static void JoinTeam(Team newTeam)
        {
            int oldTeamIndex = FindUsersTeam(_username);
            if (oldTeamIndex != -1)
            {
                Team oldTeam = TeamsCatalog.teamsList[oldTeamIndex];
                oldTeam.members.Remove(_username);
                EditorUtility.SetDirty(oldTeam);
            }
            
            newTeam.members.Add(_username);
            EditorUtility.SetDirty(newTeam);
            AssetDatabase.SaveAssets();
            
            ChangeTeam(_catalog.teamsList.IndexOf(newTeam));
        }
        
        public static bool IsWorkspaceUsedByAnyTeam(Workspace workspace, out List<Team> byTeams)
        {
            byTeams = new List<Team>();
            
            foreach (Team team in TeamsCatalog.teamsList)
            {
                if (team.workspace == workspace) byTeams.Add(team);
            }

            return byTeams.Count > 0;
        }
        
        public static bool IsWorkspaceActiveBecauseOfTeam(Workspace workspace)
        {
            return GetActiveTeam()?.workspace == workspace;
        }
        
        public static List<string> GetTeamNames()
        {
            List<string> teamNames = new();
            teamNames.AddRange(TeamsCatalog.teamsList.Select(team => team.name));
            return teamNames;
        }

#endregion
    }
}