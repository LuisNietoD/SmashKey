using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hats.Editor
{
    public class TeamsCatalog : ScriptableObject
    {
        [SerializeField] private Texture2D _defaultIcon;
        
        public List<Team> teamsList = new();

        public void AddTeam()
        {
            teamsList ??= new List<Team>();
            Team newTeam = CreateInstance<Team>();

            string defaultName = "Team X";
            List<string> usedNames = teamsList.Select(team => team.name).ToList();
            List<string> availableNames = DefaultTeamNames.FindAll(item => !usedNames.Contains(item));
            if (availableNames.Count > 0)
            {
                int randomIndex = Random.Range(0, availableNames.Count);
                defaultName = availableNames[randomIndex];
            }

            newTeam.name = newTeam.identifier = defaultName;
            teamsList.Add(newTeam);

            AssetDatabase.AddObjectToAsset(newTeam, this);
            AssetDatabase.SaveAssets();
            
            Teams.InvokeTeamCatalogChanged();
        }

        public void RemoveTeam(Team team)
        {
            if (teamsList.Contains(team))
            {
                if(Teams.IsTeamActive(team)) Teams.LeaveAllTeams();
                
                teamsList.Remove(team);
                DestroyImmediate(team, true);
                AssetDatabase.SaveAssets();
                
                Teams.InvokeTeamCatalogChanged();
            }
        }

        private static readonly List<string> DefaultTeamNames = new()
        {
            "Programmers",
            "Designers",
            "Artists",
            "Producers",
            "QA",
            "Audio"
        };

        private static readonly List<Color> DefaultTeamColours = new()
        {
            new Color(0.83f, 0.43f, 0.25f),
            new Color(0.44f, 0.31f, 0.83f),
            new Color(0.55f, 0.83f, 0.27f),
            new Color(0.83f, 0.23f, 0.47f),
            new Color(0.3f, 0.72f, 0.8f),
            new Color(0.18f, 0.73f, 0.51f),
        };
    }
}