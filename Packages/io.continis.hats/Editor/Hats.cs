using UnityEditor;

namespace Hats.Editor
{
    public class HatsMemory : ScriptableSingleton<HatsMemory>
    {
        public bool initialised;
        public string userName;
        public int activeTeamIndex;
    }
    
    public static class Hats
    {
        [InitializeOnLoadMethod]
        internal static void Initialise()
        {
            if (!HatsMemory.instance.initialised)
            {
                Teams.Initialise();
                Workspaces.Initialise();
            }
            else
            {
                Teams.AfterRecompile();
                Workspaces.AfterRecompile();
            }

            HatsMemory.instance.initialised = true;
        }

        public enum ChangeReason
        {
            EditorStartup,
            Recompile,
            UserAction,
        }
    }
}