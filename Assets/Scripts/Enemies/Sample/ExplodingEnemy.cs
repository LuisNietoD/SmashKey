using UnityEngine;

public class ExplodingEnemy : MonoBehaviour, IEnemy
{
    public int damage { get; } = 20;
    public int health { get; set; } = 50;
    public float moveSpeed = 5f;
    public float explosionRange = 3f;
    public float explosionRadius = 5f;
    public GameObject explosionPrefab;

    private Transform player;
    private Rigidbody rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= explosionRange)
        {
            Attack();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed + new Vector3(0f, 0f, GameController.Metrics.platformTravelTime);
    }

    public void Attack()
    {
        rb.linearVelocity = Vector3.zero;

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                // Assuming the player has a method to take damage
                //PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
                //if (playerHealth != null)
               // {
                //    playerHealth.TakeDamage(damage);
                //}
            }
        }

        Destroy(gameObject);
    }

    public void Hit(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
        StatsManager.Instance.OnEnemyDamageDealt?.Invoke(damageAmount);
    }

    private void Die()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
        StatsManager.Instance.OnEnemyKilled?.Invoke(1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
