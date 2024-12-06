using Hats.Editor.Rules;
using Hats.Editor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hats.Editor.CustomEditors.Rules
{
    [CustomPropertyDrawer(typeof(HighlightGizmos))]
    public class HighlightGizmosPropertyDrawer : RulePropertyDrawer
    {
        protected override void AddExtraElements(Foldout foldout)
        {
            VisualElement extras = new();
            foldout.Add(extras);
            
            MiniHelpBox helpBox = new MiniHelpBox("Use the buttons below to quickly enable/disable Gizmos. " +
                                                  "They are just helpers, and not related to the execution of this Rule.",
                HelpBoxMessageType.Info);
            extras.Add(helpBox);

            VisualElement row = new VisualElement { style = { flexDirection = FlexDirection.Row} };
            extras.Add(row);

            Button enableAll = new Button(() => EnableAllGizmos(true))
            {
                text = "Enable all Gizmos", style = { flexGrow = 1 }
            };
            row.Add(enableAll);
            
            Button disableAll = new Button(() => EnableAllGizmos(false))
            {
                text = "Disable all Gizmos", style = { flexGrow = 1 }
            };
            row.Add(disableAll);
        }

        private void EnableAllGizmos(bool shouldBeEnabled)
        {
            GizmoInfo[] info = GizmoUtility.GetGizmoInfo();
            foreach (GizmoInfo gizmoInfo in info)
            {
                gizmoInfo.iconEnabled = shouldBeEnabled;
                gizmoInfo.gizmoEnabled = shouldBeEnabled;
                GizmoUtility.ApplyGizmoInfo(gizmoInfo, false);
            }
        }
    }
}