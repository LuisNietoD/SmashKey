using UnityEngine;
using System.Collections;

public class SymmetricalShooter : MonoBehaviour, IWeapon
{
    
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    
    public int damage => GameMetrics.Global.Symmetrical_Damage;
    private float cooldownTime => GameMetrics.Global.Symmetrical_CooldownTime;
    
    
    private float bulletSpeed => GameMetrics.Global.Symmetrical_BulletSpeed;
    private Vector3 bulletScale => GameMetrics.Global.Symmetrical_BulletScale;
    private int bulletCount => GameMetrics.Global.Symmetrical_BulletCount;
    private float spawnOffset => GameMetrics.Global.Symmetrical_SpawnOffset;
    private float offsetBetweenBullets => GameMetrics.Global.Symmetrical_OffsetBetweenBullets;
    public float spawnDelay = 0.03f;
    private float actualCooldown = 0;

    private void Update()
    {
        actualCooldown -= Time.deltaTime;
    }
    
    public void AutoShoot()
    {
        if (actualCooldown > 0 || !GameMetrics.Global.Symmetrical_Enabled)
            return;
        
        actualCooldown = cooldownTime;
        
        if (bulletPrefab == null || spawnPoint == null) return;

        StartCoroutine(ShootWithDelay());
        
        StatsManager.Instance.OnWeaponShot?.Invoke(1);
    }

    private IEnumerator ShootWithDelay()
    {
        int[] indices = GenerateRandomOrder(bulletCount);

        for (int i = 0; i < bulletCount; i++)
        {
            int index = indices[i];
            float offset = spawnOffset + index * offsetBetweenBullets;

            // Right bullet
            SpawnBullet(spawnPoint.position + spawnPoint.right * offset, spawnPoint.forward);

            // Left bullet
            SpawnBullet(spawnPoint.position - spawnPoint.right * offset, spawnPoint.forward);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnBullet(Vector3 position, Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.LookRotation(direction));
        bullet.transform.localScale = bulletScale;
        bullet.GetComponent<Bullet>().Initialize(5, damage);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }

    private int[] GenerateRandomOrder(int count)
    {
        int[] indices = new int[count];
        for (int i = 0; i < count; i++) indices[i] = i;

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, count);
            (indices[i], indices[randomIndex]) = (indices[randomIndex], indices[i]);
        }

        return indices;
    }
}