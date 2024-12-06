using System.Collections.Generic;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hats.Editor.UIElements
{
    [UxmlElement("TeamPickerDropdown")]
    public partial class TeamPickerDropdown : MaskField
    {
        public TeamPickerDropdown() : this(true) { }
        
        public TeamPickerDropdown(bool allowNone)
        {
            label = "Teams";
            Teams.TeamCatalogChanged += UpdateChoices;
            UpdateChoices();
            TweakFirstTwoChoices(allowNone);
        }

        private void UpdateChoices()
        {
            choices = Teams.GetTeamNames();
        }

        private void TweakFirstTwoChoices(bool allowNone)
        {
            FieldInfo field = typeof(MaskField).GetField("m_Choices", BindingFlags.NonPublic | BindingFlags.Instance);
            List<string> internalChoices = (List<string>)field!.GetValue(this);
            internalChoices[0] = allowNone ? "No team" : "";
            internalChoices[1] = "All teams";
            field.SetValue(this, internalChoices);
        }
    }
}