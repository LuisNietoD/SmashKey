using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hats.Editor.UIElements;
using Hats.Editor.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Hats.Editor.ProjectSettings
{
    public static class HatsSettingsProvider
    {
        const string SettingsPath = "Project/Hats";
        
        private static List<Workspace> _personalWorkspaces;
        
        private static ListView _workspacesList;
        private static Button _teamButton;
        private static Label _teamLabel;
        private static VisualElement _teamMiniIcon;

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path.Combine(Constants.UITemplatesFolderPath, "HatsProjectSettings.uxml"));

            SettingsProvider provider = new SettingsProvider(SettingsPath, SettingsScope.Project)
            {
                label = "Hats",
                activateHandler = (searchContext, root) =>
                {
                    root.Add(visualTree.CloneTree());

                    ScrollView scrollView = root.Q<ScrollView>("MainScrollView");

                    // Workspaces UI
                    
                    _personalWorkspaces = HatsSettings.PersonalWorkspaces.value;
                    
                    Toggle displayInToolbarToggle = root.Q<Toggle>("DisplayInToolbarToggle");
                    displayInToolbarToggle.SetValueWithoutNotify(HatsSettings.DisplayInToolbar.value);
                    displayInToolbarToggle.RegisterValueChangedCallback(evt =>
                    {
                        MainToolbarHelper.ToggleSwitcherUI(evt.newValue);
                        HatsSettings.DisplayInToolbar.value = evt.newValue;
                    });
                    
                    DropdownField switcherTypeDropdown = root.Q<DropdownField>("SwitcherType");
                    switcherTypeDropdown.SetValueWithoutNotify(GetSwitcherDropdownValue());
                    switcherTypeDropdown.RegisterValueChangedCallback(OnSwitcherTypeChanged);
                    
                    _workspacesList = root.Q<ListView>("PersonalWorkspaces");
                    _workspacesList.makeItem = () => new ObjectField();
                    _workspacesList.bindItem = (element, i) =>
                    {
                        ObjectField objectField = (ObjectField) element;
                        objectField.objectType = typeof(Workspace);
                        objectField.value = _personalWorkspaces[i];
                        int index = i;
                        objectField.RegisterValueChangedCallback(evt => OnWorkspaceItemChanged(evt, index));
                    };
                    _workspacesList.unbindItem = (element, i) =>
                    {
                        ObjectField objectField = (ObjectField) element;
                        objectField.UnregisterValueChangedCallback(evt => OnWorkspaceItemChanged(evt, i));
                    };
                    _workspacesList.itemsAdded += ints => SaveWorkspaces();
                    _workspacesList.itemsRemoved += OnWorkspaceRemoved;
                    _workspacesList.itemIndexChanged += OnWorkspacesReordered;
                    _workspacesList.itemsSource = _personalWorkspaces;
                    
                    // Teams UI
                    
                    _teamLabel = root.Q<Label>("ActiveTeamLabel");
                    _teamMiniIcon = root.Q<VisualElement>("TeamMiniIcon");
                    _teamButton = root.Q<Button>("ActiveTeamButton");
                    _teamButton.clicked += Teams.LeaveAllTeams;
                    
                    InitialiseTeamUI(); // This will work after recompilation
                    Teams.TeamsInitialised += InitialiseTeamUI; // This will work on first load
                    Teams.TeamChanged += _ => InitialiseTeamUI();
                    
                    root.Q<ObjectField>("IndexObjectField").SetValueWithoutNotify(Teams.TeamsCatalog);
                    
                    InspectorElement inspectorElement = new(new SerializedObject(Teams.TeamsCatalog));
                    scrollView.Add(inspectorElement);
                    inspectorElement.Q<MiniHelpBox>("MoreInfoBox").RemoveFromHierarchy();
                    inspectorElement.PlaceBehind(root.Q<VisualElement>("FinalSpacer"));
                },

                keywords = new HashSet<string>(new[] { "Workspace", "Team", "Rule", "Profile", "User" })
            };

            return provider;
        }

        private static void InitialiseTeamUI()
        {
            bool inTeam = Teams.GetActiveTeam() != null;
            _teamLabel.text = Teams.GetActiveTeamIdentifier();
            _teamLabel.style.unityFontStyleAndWeight = inTeam ? FontStyle.Bold : FontStyle.Normal;
            _teamButton.SetEnabled(inTeam);
            _teamButton.text = inTeam ? "Leave Team" : "No Team active";
            _teamMiniIcon.style.opacity = inTeam ? 1f : 0.25f;
        }

        private static void OnWorkspaceItemChanged(ChangeEvent<Object> evt, int index)
        {
            _personalWorkspaces[index] = (Workspace) evt.newValue;
            SaveWorkspaces();
        }
        
        private static void OnWorkspaceRemoved(IEnumerable<int> ints)
        {
            // Switch to the Default workspace if the current one is removed
            if (ints.Any(i => Workspaces.GetActiveWorkspaceIndex() == i))
            {
                Workspaces.ChangeWorkspace(-1);
            }
            SaveWorkspaces();
        }

        private static void OnWorkspacesReordered(int i, int i1)
        {
            int activeIndex = Workspaces.GetActiveWorkspaceIndex();
            if (activeIndex >= i && activeIndex <= i1)
            {
                activeIndex = activeIndex == i ? i1 : activeIndex + 1;
            }
            else if (activeIndex >= i1 && activeIndex <= i)
            {
                activeIndex = activeIndex == i ? i1 : activeIndex - 1;
            }
            
            Workspaces.SetActiveWorkspaceIndex(activeIndex);
            SaveWorkspaces();
        }

        private static void SaveWorkspaces()
        {
            EditorApplication.delayCall +=
                () =>
                {
                    HatsSettings.PersonalWorkspaces.SetValue(_personalWorkspaces, true);
                    Workspaces.InvokeAvailableWorkspacesChanged();
                };
        }

        private static void OnSwitcherTypeChanged(ChangeEvent<string> evt)
        {
            HatsSettings.SelectedSwitcherUIType.value = evt.newValue switch
            {
                "Dropdown" => 0,
                _ => 1
            };
            HatsSettings.SwitcherUITypeChanged?.Invoke(HatsSettings.SelectedSwitcherUIType.value); // Notifies the Switcher UI
        }

        private static string GetSwitcherDropdownValue()
        {
            return HatsSettings.SelectedSwitcherUIType.value switch
            {
                0 => "Dropdown",
                _ => "Buttons with Icons"
            };
        }
        
        internal static void ForceRepaintWorkspacesList()
        {
            _workspacesList?.Rebuild();
        }
    }
}