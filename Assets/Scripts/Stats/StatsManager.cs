using System;
using LTX.Singletons;
using Stats;

public class StatsManager : MonoSingleton<StatsManager>
{
    public EnemyStats EnemyStats;
    public PlayerStats PlayerStats;
    public WorldStats WorldStats;

    #region EnemiesStats
    
    public Action<float> OnEnemyDamageDealt;
    public Action<int> OnEnemySpawned;

    #endregion
    
    #region PlayerStats

    public Action<int> OnEnemyKilled; 
    public Action<float> OnPlayerDamageDealt;
    public Action<int> OnWeaponShot;
    public Action<int> OnKeyTapped;

    #endregion
    
    #region WorldStats

    public Action<float> OnDistanceTravelled;
    public Action<float> OnTimePlayed;

    #endregion

    public void GetPlayerInfos(DisplayOnEnable obj)
    {
        obj.stat = PlayerStats;
    }
    
    public void GetEnemiesInfos(DisplayOnEnable obj)
    {
        obj.stat = EnemyStats;
    }
    
    public void GetWorldInfos(DisplayOnEnable obj)
    {
        obj.stat = WorldStats;
    }
}