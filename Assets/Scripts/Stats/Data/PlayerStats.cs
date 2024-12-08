using System.Collections.Generic;

[System.Serializable]
public struct PlayerStats : Stat
{
    public int totalKilled;
    public int totalDamageDealt;
    public int weaponsShot;
    public int totalKeysTapped;
    
    public List<int> GetValues()
    {
        return new List<int>
        {
            totalKilled,
            totalDamageDealt,
            weaponsShot,
            totalKeysTapped
        };
    }
}