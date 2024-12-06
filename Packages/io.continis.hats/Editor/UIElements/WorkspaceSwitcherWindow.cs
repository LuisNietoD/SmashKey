using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hats.Editor.UIElements
{
    public class WorkspaceSwitcherWindow : EditorWindow
    {
        [SerializeField] private StyleSheet _styles;

        [MenuItem("Window/Hats/Workspace Switcher Window")]
        public static void ShowWindow()
        {
            WorkspaceSwitcherWindow wnd = GetWindow<WorkspaceSwitcherWindow>();
            wnd.titleContent = new GUIContent("WorkspaceSwitcher");
        }

        public void CreateGUI()
        {
            WorkspaceSwitcherUI workspaceSwitcherUI = new()
            {
                style =
                {
                    marginTop = 10,
                    marginLeft = 10,
                }
            };
            rootVisualElement.Add(workspaceSwitcherUI);
            rootVisualElement.styleSheets.Add(_styles);
        }
    }
}
