using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hats.Editor.ProjectSettings;
using Hats.Editor.Utilities;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Hats.Editor
{
    public static class Workspaces
    {
        private static int _activeWorkspaceIndex = -1;

        public static event Action<int> WorkspaceChanged;
        public static event Action AvailableWorkspacesChanged;

        private static Workspace _noWorkspace;
        internal static Workspace NoWorkspace
        {
            get
            {
                if(_noWorkspace == null)
                {
                    string noWorkspacePath = Path.Combine(Constants.PresetsFolderPath, $"{Constants.DefaultWorkspaceFileName}.asset");
                    _noWorkspace = AssetDatabase.LoadAssetAtPath<Workspace>(noWorkspacePath);
                }

                return _noWorkspace;
            }
        }
        
        internal static void Initialise()
        {
            ChangeWorkspace(HatsSettings.ActiveWorkspaceIndex.value, Hats.ChangeReason.EditorStartup);
        }
        
        public static void AfterRecompile()
        {
            ChangeWorkspace(HatsSettings.ActiveWorkspaceIndex.value, Hats.ChangeReason.Recompile);
        }
        
        internal static void InvokeAvailableWorkspacesChanged() => AvailableWorkspacesChanged?.Invoke();
        
        // To use only when reordering workspaces
        internal static void SetActiveWorkspaceIndex(int index) => _activeWorkspaceIndex = index;

        #region Public Methods
        
        public static void ChangeWorkspace(int newIndex, Hats.ChangeReason reason = Hats.ChangeReason.UserAction)
        {
            //Debug.Log("Changing Workspace to index " + newIndex);
            if (GetWorkspace(newIndex) == null) return;

            // Execute rules for Workspace about to be made inactive
            Workspace workspace = GetActiveWorkspace();
            if (reason != Hats.ChangeReason.EditorStartup &&
                workspace != null  &&
                !IsWorkspaceActiveBecauseOfTeam(workspace) &&
                workspace.HasRules)
            {
                workspace.ExecuteRules( false, reason);
            }
                
            _activeWorkspaceIndex = newIndex;
            
            // Execute rules for newly-active Workspace
            workspace = GetActiveWorkspace();
            if (workspace != null  &&
                !IsWorkspaceActiveBecauseOfTeam(workspace) &&
                workspace.HasRules)
            {
                workspace.ExecuteRules( true, reason);
            }
            
            HatsSettings.ActiveWorkspaceIndex.value = _activeWorkspaceIndex;
            WorkspaceChanged?.Invoke(_activeWorkspaceIndex);
        }

        private static bool IsWorkspaceActiveBecauseOfTeam(Workspace workspace)
        {
            bool isItActive = Teams.IsWorkspaceActiveBecauseOfTeam(workspace);
            if(isItActive) Debug.Log($"(Hats) Workspace {workspace.identifier} was not activated/deactivated because it's already assigned to team {Teams.GetActiveTeam().identifier}, which you are part of.");
            return isItActive;
        }

        public static Workspace GetWorkspace(int index)
        {
            if(index <= -2 || index >= HatsSettings.PersonalWorkspaces.value.Count) return null;
            return index == -1 ? NoWorkspace : HatsSettings.PersonalWorkspaces.value[index];
        }

        public static int GetActiveWorkspaceIndex() => _activeWorkspaceIndex;
        
        public static List<Workspace> GetAvailableWorkspaces => HatsSettings.PersonalWorkspaces.value.Where(workspace => workspace != null).ToList();

        public static List<string> GetWorkspaceNames() => HatsSettings.PersonalWorkspaces.value.Where(workspace => workspace != null).Select(workspace => workspace.identifier).ToList<string>();

        public static Workspace GetActiveWorkspace()
        {
            return _activeWorkspaceIndex == -1 ?
                NoWorkspace : HatsSettings.PersonalWorkspaces.value[_activeWorkspaceIndex];
        }
        
        public static bool IsWorkspacePartOfAvailableOnes(Workspace workspace) => GetAvailableWorkspaces.Contains(workspace);

        public static void AddWorkspaceToAvailable(Workspace newWorkspace)
        {
            if (newWorkspace == null) return;
            if (IsWorkspacePartOfAvailableOnes(newWorkspace)) return;
            
            // TODO: Will this break HatsSettingsProvider? It's modifying the setting directly, but HatsSettingsProvider has a copy in _personalWorkspaces
            HatsSettings.PersonalWorkspaces.value.Add(newWorkspace);
            HatsSettingsManager.settings.Save();
            
            AvailableWorkspacesChanged?.Invoke();
        }

        #endregion
        
        #region ShortcutMethods
        
        private const ShortcutModifiers DefaultShortcutModifiers = ShortcutModifiers.Alt | ShortcutModifiers.Shift;
        
        [Shortcut("Hats/Activate Default Workspace", KeyCode.Alpha1, DefaultShortcutModifiers)]
        public static void ActivateDefaultWorkspace() => ChangeWorkspace(-1);

        [Shortcut("Hats/Activate Workspace 1", KeyCode.Alpha2, DefaultShortcutModifiers)]
        public static void ActivateWorkspace0() => ChangeWorkspace(0);

        [Shortcut("Hats/Activate Workspace 2", KeyCode.Alpha3, DefaultShortcutModifiers)]
        public static void ActivateWorkspace1() => ChangeWorkspace(1);

        [Shortcut("Hats/Activate Workspace 3", KeyCode.Alpha4, DefaultShortcutModifiers)]
        public static void ActivateWorkspace2() => ChangeWorkspace(2);

        [Shortcut("Hats/Activate Workspace 4", KeyCode.Alpha5, DefaultShortcutModifiers)]
        public static void ActivateWorkspace3() => ChangeWorkspace(3);

        [Shortcut("Hats/Activate Workspace 5", KeyCode.Alpha6, DefaultShortcutModifiers)]
        public static void ActivateWorkspace4() => ChangeWorkspace(4);

        [Shortcut("Hats/Activate Workspace 6", KeyCode.Alpha7, DefaultShortcutModifiers)]
        public static void ActivateWorkspace5() => ChangeWorkspace(5);

        [Shortcut("Hats/Activate Workspace 7", KeyCode.Alpha8, DefaultShortcutModifiers)]
        public static void ActivateWorkspace6() => ChangeWorkspace(6);

        [Shortcut("Hats/Activate Workspace 8", KeyCode.Alpha9, DefaultShortcutModifiers)]
        public static void ActivateWorkspace7() => ChangeWorkspace(7);

        [Shortcut("Hats/Activate Workspace 9", KeyCode.Alpha0, DefaultShortcutModifiers)]
        public static void ActivateWorkspace8() => ChangeWorkspace(8);
        
        [Shortcut("Hats/Activate Next Workspace", KeyCode.RightArrow, DefaultShortcutModifiers)]
        public static void ActivateNextWorkspace() => ChangeWorkspace(GetActiveWorkspaceIndex() + 1);
        
        [Shortcut("Hats/Activate Previous Workspace", KeyCode.LeftArrow, DefaultShortcutModifiers)]
        public static void ActivatePreviousWorkspace() => ChangeWorkspace(GetActiveWorkspaceIndex() - 1);

        #endregion
    }
}