using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        private int health = 100;
        
        public void Hit(int damageAmount)
        {
            StatsManager.Instance.OnPlayerDamageDealt?.Invoke(damageAmount);
            health -= damageAmount;
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