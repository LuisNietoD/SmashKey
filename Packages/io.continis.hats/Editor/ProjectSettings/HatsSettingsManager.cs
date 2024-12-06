using Hats.Editor.Utilities;
using UnityEditor;
using UnityEditor.SettingsManagement;

namespace Hats.Editor.ProjectSettings
{
    public class HatsSettingsManager
    {
        private static Settings instance;

        internal static Settings settings
        {
            get
            {
                if (instance == null)
                    instance = new Settings(Constants.PackageName, "Settings");

                return instance;
            }
        }
    }

    public class PackageSetting<T> : UserSetting<T>
    {
        public PackageSetting(string key, T value)
            : base(HatsSettingsManager.settings, key, value, SettingsScope.Project) { }
    }

    public class UserPref<T> : UserSetting<T>
    {
        public UserPref(string key, T value)
            : base(HatsSettingsManager.settings, key, value, SettingsScope.User) { }
    }
}