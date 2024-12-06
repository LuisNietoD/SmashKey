using Hats.Editor.ProjectSettings;
using Hats.Editor.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hats.Editor.UIElements
{
    [UxmlElement("WorkspaceSwitcherUI")]
    public partial class WorkspaceSwitcherUI : VisualElement
    {
        private readonly bool _inToolbar;
        private VisualElement _currentSwitcherUI;

        public WorkspaceSwitcherUI() : this(false) { }

        public WorkspaceSwitcherUI(bool inToolbar)
        {
            _inToolbar = inToolbar;
            
            HatsSettings.SwitcherUITypeChanged += ShowUI;
            Workspaces.AvailableWorkspacesChanged += EnableDisable;

            if (_inToolbar)
            {
                styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Constants.USSPath));
                style.marginLeft = 4;
            }
            
            ShowUI(HatsSettings.SelectedSwitcherUIType.value);
            EnableDisable();
        }

        private void EnableDisable()
        {
            _currentSwitcherUI?.SetEnabled(Workspaces.GetAvailableWorkspaces.Count > 0);
        }

        private void ShowUI(int chosenUIIndex)
        {
            _currentSwitcherUI?.RemoveFromHierarchy();

            _currentSwitcherUI = chosenUIIndex switch
            {
                0 => new WorkspaceSwitcherDropdown(_inToolbar),
                _ => new WorkspaceSwitcherButtonGroup(_inToolbar)
            };
            Add(_currentSwitcherUI);
        }
    }
}