using UnityEngine;

namespace Stats
{
    public class StatsListener : MonoBehaviour
    {
        private void OnEnable()
        {
            StatsManager.Instance.OnEnemyDamageDealt += EnemyDamageDealt;
            StatsManager.Instance.OnEnemySpawned += EnemySpawned;

            StatsManager.Instance.OnEnemyKilled += EnemyKilled;
            StatsManager.Instance.OnPlayerDamageDealt += PlayerDamageDealt;
            StatsManager.Instance.OnWeaponShot += WeaponBulletShot;
            StatsManager.Instance.OnKeyTapped += KeyTapped;

            StatsManager.Instance.OnDistanceTravelled += DistanceTravelled;
            StatsManager.Instance.OnTimePlayed += TimePlayed;
        }
        
        private void OnDisable()
        {
            StatsManager.Instance.OnEnemyDamageDealt -= EnemyDamageDealt;
            StatsManager.Instance.OnEnemySpawned -= EnemySpawned;

            StatsManager.Instance.OnEnemyKilled -= EnemyKilled;
            StatsManager.Instance.OnPlayerDamageDealt -= PlayerDamageDealt;
            StatsManager.Instance.OnWeaponShot -= WeaponBulletShot;
            StatsManager.Instance.OnKeyTapped -= KeyTapped;

            StatsManager.Instance.OnDistanceTravelled -= DistanceTravelled;
            StatsManager.Instance.OnTimePlayed -= TimePlayed;
        }
        
        #region EnemiesStats
        
        private void EnemyDamageDealt(float obj)
        {
            StatsManager.Instance.EnemyStats.totalDamageDealt += Mathf.FloorToInt(obj);
        }

        private void EnemySpawned(int obj)
        {
            StatsManager.Instance.EnemyStats.totalEnemiesSpawned += obj;
        }
        
        #endregion

        #region PlayerStats
        
        private void EnemyKilled(int obj)
        {
            StatsManager.Instance.PlayerStats.totalKilled += obj;
        }

        private void PlayerDamageDealt(float obj)
        {
            StatsManager.Instance.PlayerStats.totalDamageDealt += Mathf.FloorToInt(obj);
        }

        private void WeaponBulletShot(int obj)
        {
            StatsManager.Instance.PlayerStats.weaponsShot += obj;
        }

        private void KeyTapped(int obj)
        {
            StatsManager.Instance.PlayerStats.totalKeysTapped += obj;
        }
        
        #endregion

        #region WorldStats
        
        private void DistanceTravelled(float obj)
        {
            StatsManager.Instance.WorldStats.totalDistanceTravelled += Mathf.FloorToInt(obj);
        }

        private void TimePlayed(float obj)
        {
            StatsManager.Instance.WorldStats.totalTimePlayed += Mathf.FloorToInt(obj);
        }
        
        #endregion
    }
}