using System;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        private int health;

        private void Start()
        {
            health = GameMetrics.Global.Health;
        }

        public void Hit(int damageAmount)
        {
            StatsManager.Instance.OnPlayerDamageDealt?.Invoke(damageAmount);
            health--;
            Debug.Log(health);
            if (health <= 0)
            {
                OnPlayerDeath();
            }
        }

        private void OnPlayerDeath()
        {
            GameController.OnGameEnd?.Invoke();
            Destroy(gameObject);
        }
    }
}