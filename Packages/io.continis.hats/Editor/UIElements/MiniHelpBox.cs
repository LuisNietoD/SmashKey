using System.IO;
using Hats.Editor.Utilities;
using UnityEditor;
using UnityEngine.UIElements;

namespace Hats.Editor.UIElements
{
    [UxmlElement("MiniHelpBox")]
    public partial class MiniHelpBox : HelpBox
    {
        public MiniHelpBox() { }

        public MiniHelpBox(string text, HelpBoxMessageType messageType)
        {
            this.text = text;
            this.messageType = messageType;

            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Constants.USSPath));
        }
    }
}