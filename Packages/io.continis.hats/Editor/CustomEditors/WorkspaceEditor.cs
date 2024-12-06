using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hats.Editor.ProjectSettings;
using Hats.Editor.Rules;
using Hats.Editor.UIElements;
using Hats.Editor.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hats.Editor.CustomEditors
{
    [CustomEditor(typeof(Workspace))]
    public class WorkspaceEditor : UnityEditor.Editor
    {
        private VisualElement _root;
        private VisualElement _warningMessage;

        public override VisualElement CreateInspectorGUI()
        {
            _root = new();
            InspectorElement.FillDefaultInspector(_root, serializedObject, this);

            _root.Q<PropertyField>("PropertyField:rules").RegisterCallback<GeometryChangedEvent>(AddCallbackOnRuleAdd);
            
            _root.Add(new Label("Workspace usage"){style = { unityFontStyleAndWeight = FontStyle.Bold }}); 
            
            _warningMessage = new VisualElement() { name = "WarningMessage" };
            _root.Add(_warningMessage);
            
            WarnIfWorkspaceNotEnabled();
            Workspaces.AvailableWorkspacesChanged += WarnIfWorkspaceNotEnabled;
            
            return _root;
        }

        private void AddCallbackOnRuleAdd(GeometryChangedEvent evt)
        {
            VisualElement evtTarget = ((VisualElement)evt.target);
            evtTarget.UnregisterCallback<GeometryChangedEvent>(AddCallbackOnRuleAdd);
            ListView rulesList = evtTarget.Q<ListView>();
            if(rulesList == null) return; // Might happen after recompile
            rulesList.overridingAddButtonBehavior = OnAddRule;
        }

        private void WarnIfWorkspaceNotEnabled()
        {
            if(_warningMessage.childCount != 0) _warningMessage.Clear();

            Workspace workspace = (Workspace)serializedObject.targetObject;
            bool isEnabled = Workspaces.IsWorkspacePartOfAvailableOnes(workspace);
            if (isEnabled)
            {
                _warningMessage.Add(new MiniHelpBox("This workspace is one of the available Workspaces.", HelpBoxMessageType.Info));
            }
            else
            {
                _warningMessage.Add(new MiniHelpBox("Not part of available Workspaces.", HelpBoxMessageType.Info));
            }

            bool isUsedByTeam = Teams.IsWorkspaceUsedByAnyTeam(workspace, out List<Team> byTeams);
            string also = isEnabled ? "also " : "";
            if (isUsedByTeam)
            {
                foreach (Team team in byTeams)
                {
                    _warningMessage.Add(new MiniHelpBox($"This workspace is {also}used by Team {team.name}.", HelpBoxMessageType.Info));
                }
            }
            else
            {
                _warningMessage.Add(new MiniHelpBox($"This workspace is not used by any Team.", HelpBoxMessageType.Info));
            }
            
            VisualElement buttonsContainer = new VisualElement() {style = { flexDirection = FlexDirection.Row}};
            
            Button addButton = new(EnableWorkspace) { text = "Add to available", style = { flexGrow = 1 } };
            addButton.SetEnabled(!isEnabled);
            buttonsContainer.Add(addButton);
            
            Button settingsButton = new() { text = "Open Hats Settings", style = { flexGrow = 1 }};
            settingsButton.clicked += () => SettingsService.OpenProjectSettings("Project/Hats");
            buttonsContainer.Add(settingsButton);
            
            _warningMessage.Add(buttonsContainer);
        }

        private void EnableWorkspace()
        {
            Workspaces.AddWorkspaceToAvailable((Workspace)serializedObject.targetObject);
            HatsSettingsProvider.ForceRepaintWorkspacesList();
            
            // Redraw the warning 
            WarnIfWorkspaceNotEnabled();
        }

        private void OnAddRule(BaseListView arg1, Button arg2)
        {
            GenericMenu menu = new();
            Workspace workspace = (Workspace)target;
            
            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies()
                         .SelectMany(s => s.GetTypes())
                         .Where(p => (typeof(IRule).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract) && p.BaseType != typeof(ScriptableObject)))
            {
                string typeName = ObjectNames.NicifyVariableName(type.Name);
                menu.AddItem(new GUIContent(typeName), false, () =>
                {
                    IRule rule = (IRule)Activator.CreateInstance(type);
                    workspace.rules ??= new List<IRule>();
                    workspace.rules.Add(rule);
                    EditorUtility.SetDirty(target);
                });
            }
            
            menu.DropDown(arg2.worldBound);
        }
    }
}