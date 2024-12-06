using System;
using System.Collections.Generic;
using UnityEditor.SettingsManagement;

namespace Hats.Editor.ProjectSettings
{
    public static class HatsSettings
    {
        internal static Action<int> SwitcherUITypeChanged;
         
        [UserSetting] public static UserPref<List<Workspace>> PersonalWorkspaces = new("general.personalWorkspaces", new List<Workspace>());
        [UserSetting] public static UserPref<int> ActiveWorkspaceIndex = new("general.activeWorkspaceIndex", -1);
        [UserSetting] public static UserPref<int> SelectedSwitcherUIType = new("general.selectedSwitcherUIType", 0);
        [UserSetting] public static UserPref<bool> DisplayInToolbar = new("general.displayInToolbar", true);
    }
}