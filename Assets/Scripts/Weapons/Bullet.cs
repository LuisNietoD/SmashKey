using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            StatsManager.Instance.OnPlayerDamageDealt?.Invoke(10f);
        }
        else if (col.CompareTag("Enemy"))
        {
            StatsManager.Instance.OnEnemyDamageDealt?.Invoke(10f);
        }
        
        Destroy(gameObject);
    }
}