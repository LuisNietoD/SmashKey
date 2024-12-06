// Code partly taken from Santiago Perez's repository: https://github.com/Sammmte/unity-toolbar-extender-ui-toolkit (MIT License)

using System;
using System.Reflection;
using Hats.Editor.ProjectSettings;
using Hats.Editor.Utilities;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hats.Editor.UIElements
{
    [InitializeOnLoad]
    public static class MainToolbarHelper
    {
        private const string ToolbarRootElementFieldName = "m_Root";
        private const string ToolbarCenterContainerName = "ToolbarZonePlayMode";
        private const string ToolbarLeftContainerName = "ToolbarZoneLeftAlign";
        private const string ToolbarRightContainerName = "ToolbarZoneRightAlign";
        private const string ToolbarPlayButtonName = "Play";

        private static readonly Type ToolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");
        private static ScriptableObject _innerToolbarObject;

        public static VisualElement UnityToolbarRoot { get; private set; }

        public static VisualElement LeftContainer { get; private set; }
        public static VisualElement CenterContainer { get; private set; }
        public static VisualElement RightContainer { get; private set; }
        public static VisualElement PlayModeButtonsContainer { get; private set; }

        private static bool _initialized;
        private static WorkspaceSwitcherUI _switcherUI;

        static MainToolbarHelper()
        {
            EditorApplication.update += OnUpdate;
        }

        private static void WrapNativeToolbar()
        {
            FindUnityToolbar();
            if (_innerToolbarObject == null) return;
            
            CacheNativeToolbarContainers();
            
            _switcherUI = new WorkspaceSwitcherUI(true);
            if(HatsSettings.DisplayInToolbar.value) ToggleSwitcherUI(true);
            
            _initialized = true;
        }

        public static void ToggleSwitcherUI(bool value)
        {
            if (value) RightContainer.Insert(0, _switcherUI);
            else RightContainer.Remove(_switcherUI);
        }

        private static void FindUnityToolbar()
        {
            var toolbars = Resources.FindObjectsOfTypeAll(ToolbarType);
            _innerToolbarObject = toolbars.Length > 0 ? (ScriptableObject)toolbars[0] : null;
        }

        private static void CacheNativeToolbarContainers()
        {
            FieldInfo unityToolbarRootFieldInfo = _innerToolbarObject.GetType().GetField(ToolbarRootElementFieldName, 
                BindingFlags.NonPublic | BindingFlags.Instance);
            UnityToolbarRoot = unityToolbarRootFieldInfo!.GetValue(_innerToolbarObject) as VisualElement;

            LeftContainer = UnityToolbarRoot.Q(ToolbarLeftContainerName);
            CenterContainer = UnityToolbarRoot.Q(ToolbarCenterContainerName);
            RightContainer = UnityToolbarRoot.Q(ToolbarRightContainerName);
            PlayModeButtonsContainer = CenterContainer.Q(ToolbarPlayButtonName).parent;
        }

        private static void OnUpdate()
        {
            if (!_initialized) WrapNativeToolbar(); 
        }
    }
}