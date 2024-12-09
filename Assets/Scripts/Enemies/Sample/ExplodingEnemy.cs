using System;
using DG.Tweening;
using Player;
using UnityEngine;

public class ExplodingEnemy : MonoBehaviour, IEnemy
{
    public int damage { get; } = 20;
    public int health { get; set; } = 50;
    public float moveSpeed = 5f;
    public float explosionRange = 2f;
    public float explosionRadius = 5f;
    public GameObject explosionPrefab;

    private bool canMove;
    private Transform player;
    private Rigidbody rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        
        PlaySequence();
    }
    
    private void PlaySequence()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(1f, 2f))
            .OnComplete(() => canMove = true);
    }
    
    private bool inGame = true;
    
    private void OnEnable()
    {
        GameController.OnGameEnd += GameEnd;
    }
    
    private void OnDisable()
    {
        GameController.OnGameEnd -= GameEnd;
    }

    private void GameEnd()
    {
        inGame = false;
    }

    void Update()
    {
        if (player == null || !canMove || !inGame) return;
        

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
                if (hit.TryGetComponent(out PlayerHealth playerHealth))
                {
                    playerHealth.Hit(damage);
                    Destroy(gameObject);
                }
            }
        }
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
        StatsManager.Instance.OnEnemyKilled?.Invoke(1);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EnemySpawner.Instance.OnEnemyKilled(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
