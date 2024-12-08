using System.Collections.Generic;

[System.Serializable]
public struct EnemyStats : Stat
{
    public int totalDamageDealt;
    public int totalEnemiesSpawned;
    
    public List<int> GetValues()
    {
        return new List<int>
        {
            totalDamageDealt,
            totalEnemiesSpawned
        };
    }
}