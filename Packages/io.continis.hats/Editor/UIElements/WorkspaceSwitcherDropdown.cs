using System.Collections.Generic;
using System.IO;
using Hats.Editor.Utilities;
using Hats.Editor;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hats.Editor.UIElements
{
    [UxmlElement("WorkspaceSwitcherDropdown")]
    internal partial class WorkspaceSwitcherDropdown : DropdownField
    {
        private readonly bool _inToolbar;
        private VisualElement _colorBand;

        public WorkspaceSwitcherDropdown() : this(false) { }
        
        public WorkspaceSwitcherDropdown(bool inToolbar)
        {
            _inToolbar = inToolbar;
            
            Workspaces.WorkspaceChanged += UpdateSelection;
            Workspaces.AvailableWorkspacesChanged += UpdateChoices;

            UpdateChoices();
            this.RegisterValueChangedCallback(OnValueChanged);

            _colorBand = new VisualElement { name = "ColorBand" };
            Add(_colorBand); 

            EditorApplication.delayCall += ()=> UpdateSelection(Workspaces.GetActiveWorkspaceIndex());

            if (_inToolbar)
            {
                // Editor Toolbar CSS fixes
                RemoveFromClassList("unity-base-field");
                RemoveFromClassList("unity-popup-field");
                AddToClassList("toolbar-dropdown");
                this[0].RemoveFromClassList("unity-base-field__input");
            }
        }

        private void UpdateChoices()
        {
            List<string> workspaceNames = Workspaces.GetWorkspaceNames();
            workspaceNames.Insert(0, Constants.DefaultWorkspaceIdentifier);
            choices = workspaceNames;
        }

        private void OnValueChanged(ChangeEvent<string> evt)
        {
            int i = evt.newValue == Constants.DefaultWorkspaceIdentifier ? -1 : index-1; 
            Workspaces.ChangeWorkspace(i);
            UpdateSelection(i);
        }

        private void UpdateSelection(int i)
        {
            Workspace activeWorkspace = Workspaces.GetWorkspace(i);
            SetValueWithoutNotify(activeWorkspace.identifier);
            _colorBand.style.backgroundColor = activeWorkspace.color;

            Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(Constants.UIImagesFolderPath, i == -1 ? "DashIcon.png" : "PersonasIcon.png"));
            style.backgroundImage = new StyleBackground(icon);
        }
    }
}