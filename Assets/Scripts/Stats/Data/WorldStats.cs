using System.Collections.Generic;

[System.Serializable]
public struct WorldStats : Stat
{
    public int totalDistanceTravelled;
    public int totalTimePlayed;
    
    public List<int> GetValues()
    {
        return new List<int>
        {
            totalDistanceTravelled,
            totalTimePlayed
        };
    }
}