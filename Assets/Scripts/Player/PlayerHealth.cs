using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        private int health = 100;
        
        public void Hit(int damageAmount)
        {
            health -= damageAmount;
            if (health <= 0)
            {
                OnPlayerDeath();
            }
            StatsManager.Instance.OnPlayerDamageDealt?.Invoke(damageAmount);
        }

        private void OnPlayerDeath()
        {
            GameController.OnGameEnd?.Invoke();
            Destroy(gameObject);
        }
    }
}