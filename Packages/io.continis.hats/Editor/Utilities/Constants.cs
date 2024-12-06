using System.IO;

namespace Hats.Editor.Utilities
{
    public static class Constants
    {
        internal const string PackageName = "io.continis.hats";

        private static readonly string PackagePath = Path.Combine("Packages", PackageName);
        internal static readonly string PresetsFolderPath = Path.Combine(PackagePath, "Presets");
        internal static readonly string UITemplatesFolderPath = Path.Combine(PackagePath, "UI");
        internal static readonly string UIImagesFolderPath = Path.Combine(UITemplatesFolderPath, "Images");
        internal static readonly string USSPath = Path.Combine(UITemplatesFolderPath, "Styles.uss");
        
        internal static readonly string CatalogFolderPath = Path.Combine("Assets", "Settings", "Hats");
        internal static readonly string RulesFolder = Path.Combine(CatalogFolderPath, "Rules");
        
        internal const string DefaultWorkspaceFileName = "DefaultWorkspace";
        internal const string DefaultWorkspaceIdentifier = "Default";
        internal const string NoTeamIdentifier = "No Team";
    }
}