using UnityEngine;
using DG.Tweening;

public class BasicEnemy : MonoBehaviour, IEnemy
{
    public int damage { get; } = 10;
    public int health { get; set; } = 50;
    public float moveSpeed = 2f;
    public float jumpBackSpeed = 3f;
    public float advanceDistance = 2f;
    public float shootingRange = 10f;
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 5f;

    private Transform player;
    private float nextFireTime;
    private Vector3 originalPosition;
    private Tween currentTween;
    private Rigidbody _rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalPosition = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > shootingRange && (currentTween == null || !currentTween.IsActive()))
        {
            _rb.linearVelocity = new Vector3(0, 0, GameController.Metrics.platformTravelTime);
            //MoveTowardsPlayer();
        }
        else if (distanceToPlayer <= shootingRange && (currentTween == null || !currentTween.IsActive()))
        {
            _rb.linearVelocity = new Vector3(0, 0, 0);
            //JumpBackToOriginalPosition();
            Attack();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 targetPosition = Vector3.MoveTowards(transform.position, player.position, advanceDistance);
        currentTween = transform.DOMove(targetPosition, advanceDistance / GameController.Metrics.platformTravelTime)
            .SetEase(Ease.Linear);
    }

    private void JumpBackToOriginalPosition()
    {
        if (Vector3.Distance(transform.position, player.position) <= shootingRange)
        {
            Vector3 jumpBackPosition = transform.position + transform.forward * advanceDistance;
            currentTween = transform.DOMove(jumpBackPosition, advanceDistance / jumpBackSpeed)
                .SetEase(Ease.OutQuad);
        }
    }

    public void Attack()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;

            if (bulletPrefab != null && bulletSpawnPoint != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direction = (player.position - bulletSpawnPoint.position).normalized;
                    rb.linearVelocity = direction * bulletSpeed;
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
    }

    private void Die()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        Destroy(gameObject);
    }
}
