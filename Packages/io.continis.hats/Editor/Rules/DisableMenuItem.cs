using UnityEditor;

namespace Hats.Editor.Rules
{
    public class DisableMenuItem : RuleBase
    {
        public override string RuleName => "Disable Menu Item (" + MenuPath + ")";
        
        public override string RuleDescription =>
            "Disable menu items. \n \n" +
            "To use this rule, you need to copy this script to the Assets folder and edit it," +
            " because the path to the menu needs to be pre-compiled and can't be a field." +
            " You can then add multiple menu items to the same script to disable them all at once (add a validation method for each)." +
            " Keep in mind that not all menu items can be disabled (i.e. for instance the ones under File or Edit).";

        // To customise: Rename the path below to match the path of the menu item you want to disable.
        // This one for instance disables the Package Manager item under Window.
        // Also, duplicate this block as many times as needed to disable multiple menu items.
        
        // -- Start of customisable block
        private const string MenuPath = "Window/Package Manager";
        
        [MenuItem(MenuPath, true)]
        private static bool ValidateMenuItem() => !_itemIsActive;

        // -- End of customisable block
        
        private static bool _itemIsActive;
        
        public override bool OnBecameActive(Hats.ChangeReason reason)
        {
            _itemIsActive = true;
            return true;
        }

        public override bool OnBecameInactive()
        {
            _itemIsActive = false;
            return true;
        }
    }
}