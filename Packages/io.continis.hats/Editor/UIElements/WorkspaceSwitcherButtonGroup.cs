using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hats.Editor.UIElements
{
    [UxmlElement("WorkspaceSwitcherButtonGroup")]
    internal partial class WorkspaceSwitcherButtonGroup : ToggleButtonGroup
    {
        private readonly bool _inToolbar;

        public WorkspaceSwitcherButtonGroup() : this(false) { }
        
        public WorkspaceSwitcherButtonGroup(bool inToolbar)
        {
            _inToolbar = inToolbar;
            
            Workspaces.WorkspaceChanged += UpdateSelection;
            Workspaces.AvailableWorkspacesChanged += UpdateChoices;

            isMultipleSelection = false;
            allowEmptySelection = false;

            UpdateChoices(); 
        }
        
        private void UpdateChoices()
        {
            Clear();
            AddButton(Workspaces.NoWorkspace,0);
            List<Workspace> availableWorkspaces = Workspaces.GetAvailableWorkspaces;
            for (int i = 0; i < availableWorkspaces.Count; i++)
            {
                Workspace workspace = availableWorkspaces[i];
                AddButton(workspace, i+1);
            }
            UpdateSelection(Workspaces.GetActiveWorkspaceIndex());
        }

        private void AddButton(Workspace workspace, int buttonIndex)
        {
            Button button = new Button(() => OnButtonClicked(buttonIndex));
            button.tooltip = workspace.identifier;
            button.text = "";
            button.AddToClassList("workspace-switcher__button");
            if(_inToolbar) button.AddToClassList("toolbar-button");
            button.style.backgroundImage = workspace.icon;

            VisualElement colorBand = new VisualElement(){ name = "ColorBand" };
            button.Add(colorBand);

            Add(button);
        }

        private void OnButtonClicked(int buttonIndex)
        {
            Workspaces.ChangeWorkspace(buttonIndex-1);
            UpdateButtonVisuals(buttonIndex);
        }

        private void UpdateButtonVisuals(int currentButtonPressed)
        {
            int i = 0;
            foreach (VisualElement visualElement in Children())
            {
                Button button = (Button)visualElement;
                bool sel = i == currentButtonPressed;
                Color activeWorkspaceColor = Workspaces.GetActiveWorkspace().color;
                button.style.borderLeftColor = sel ? activeWorkspaceColor : StyleKeyword.Null;
                button.Q<VisualElement>("ColorBand").style.backgroundColor = sel ? activeWorkspaceColor : StyleKeyword.Null;
                i++;
            }
        }

        private void UpdateSelection(int i)
        {
            ToggleButtonGroupState state = value;
            state.ResetAllOptions();
            state[i+1] = true;
            value = state;
            
            UpdateButtonVisuals(i+1);
        }
    }
}