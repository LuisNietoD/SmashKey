using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class StatsListener : MonoBehaviour
    {
        private void Awake()
        {
            StatsManager.Instance.PlayerStats.bulletsPerWeapon = new Dictionary<IWeapon, int>();
        }

        private void OnEnable()
        {
            StatsManager.Instance.OnEnemyDamageDealt += EnemyDamageDealt;
            StatsManager.Instance.OnEnemySpawned += EnemySpawned;

            StatsManager.Instance.OnEnemyKilled += EnemyKilled;
            StatsManager.Instance.OnPlayerDamageDealt += PlayerDamageDealt;
            StatsManager.Instance.OnBulletShot += BulletShot;
            StatsManager.Instance.OnWeaponBulletShot += WeaponBulletShot;
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
            StatsManager.Instance.OnBulletShot -= BulletShot;
            StatsManager.Instance.OnWeaponBulletShot -= WeaponBulletShot;
            StatsManager.Instance.OnKeyTapped -= KeyTapped;

            StatsManager.Instance.OnDistanceTravelled -= DistanceTravelled;
            StatsManager.Instance.OnTimePlayed -= TimePlayed;
        }
        
        #region EnemiesStats
        
        private void EnemyDamageDealt(float obj)
        {
            StatsManager.Instance.EnemyStats.totalDamageDealt += obj;
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
            StatsManager.Instance.PlayerStats.totalDamageDealt += obj;
        }

        private void BulletShot(int obj)
        {
            StatsManager.Instance.PlayerStats.totalBulletsShot += obj;
        }

        private void WeaponBulletShot(IWeapon arg1, int arg2)
        {
            if (!StatsManager.Instance.PlayerStats.bulletsPerWeapon.ContainsKey(arg1))
            {
                StatsManager.Instance.PlayerStats.bulletsPerWeapon[arg1] += arg2;
            }
            else
            {
                StatsManager.Instance.PlayerStats.bulletsPerWeapon[arg1] = arg2;
            }
        }

        private void KeyTapped(int obj)
        {
            StatsManager.Instance.PlayerStats.totalKeysTapped += obj;
        }
        
        #endregion

        #region WorldStats
        
        private void DistanceTravelled(float obj)
        {
            StatsManager.Instance.WorldStats.totalDistanceTravelled += obj;
        }

        private void TimePlayed(float obj)
        {
            StatsManager.Instance.WorldStats.totalTimePlayed += obj;
        }
        
        #endregion
    }
}