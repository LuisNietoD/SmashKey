using System;
using LTX.Singletons;

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
    public Action<int> OnBulletShot;
    public Action<IWeapon, int> OnWeaponShot;
    public Action<int> OnKeyTapped;

    #endregion
    
    #region WorldStats

    public Action<float> OnDistanceTravelled;
    public Action<float> OnTimePlayed;

    #endregion
}