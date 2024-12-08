using Player;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f;
    public int damage;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            StatsManager.Instance.OnPlayerDamageDealt?.Invoke(10f);
            other.GetComponent<PlayerHealth>().Hit(damage);
        }
        else if (other.CompareTag("Enemy"))
        {
            StatsManager.Instance.OnEnemyDamageDealt?.Invoke(10f);
            other.GetComponent<IEnemy>().Hit(damage);
        }
        
        Destroy(gameObject);
    }
}