using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour, IEnemy
{
    private SplineContainer splineContainer;
    private float movementDuration;
    private float timeElapsed = 0f;
    private bool canMove, isOnSpline;

    public int damage { get; } = 10;
    public int health { get; set; } = 50;
    
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 5f;

    [SerializeField] private Rigidbody rb;
    private Transform player;
    private float nextFireTime;

    public void Initialize(SplineContainer spline, float duration)
    {
        splineContainer = spline;
        movementDuration = duration;
        
        player = GameObject.FindGameObjectWithTag("Player").transform;

        PlaySequence();
    }

    private void PlaySequence()
    {
        Transform tr = transform;
        tr.position = new Vector3(tr.position.x, tr.position.y-3f, tr.position.z);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(tr.DOMoveY(3f, 2f))
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
        if (splineContainer == null || !inGame) return;

        Debug.Log(isOnSpline);
        
        if (isOnSpline)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / movementDuration);
            transform.position = splineContainer.EvaluatePosition(t);
            if (timeElapsed / movementDuration > 1)
            {
                timeElapsed = 0;
            }
            
            Attack();
        }
        else if (canMove)
        {
            Vector3 direction = ((Vector3)splineContainer.EvaluatePosition(0) - transform.position).normalized;
            rb.linearVelocity = direction * 5f + new Vector3(0f, 0f, GameController.Metrics.platformTravelTime);

            if (Vector3.Distance(splineContainer.EvaluatePosition(0), transform.position) <= 5f)
            {
                rb.linearVelocity = Vector3.zero;
                
                isOnSpline = true;
                canMove = false;
            }
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
                bullet.GetComponent<Bullet>().Initialize(15, damage);
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
        StatsManager.Instance.OnEnemyDamageDealt?.Invoke(damageAmount);
    }

    private void Die()
    {
        EnemySpawner.Instance.OnEnemyKilled(this);
        
        StatsManager.Instance.OnEnemyKilled?.Invoke(1);
        Destroy(gameObject);
    }
}