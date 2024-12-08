using UnityEngine;

public class Shotgun : MonoBehaviour, IWeapon
{
    public int damage { get; } = 10;
    public float cooldownTime = 0.5f;
    private float actualCooldown = 0;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 20f;
    public Vector3 bulletScale = new Vector3(1f, 1f, 1f);
    public int bulletCount = 1;
    public float spreadAngle = 15f;
    
    private void Awake()
    {
        cooldownTime = GameMetrics.Global.Shotgun_CooldownTime;
        spreadAngle = GameMetrics.Global.Shotgun_SpreadAngle;
        bulletSpeed = GameMetrics.Global.Shotgun_BulletSpeed;
        bulletCount = GameMetrics.Global.Shotgun_BulletCount;
        bulletScale = GameMetrics.Global.Shotgun_BulletScale;
    }

    private void Update()
    {
        actualCooldown -= Time.deltaTime;
    }

    public void AutoShoot()
    {
        if (actualCooldown > 0)
            return;
        
        actualCooldown = cooldownTime;
        if (bulletPrefab == null || shootPoint == null) return;

        float startAngle = -spreadAngle / 2;
        float angleIncrement = spreadAngle / Mathf.Max(1, bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.transform.localScale = bulletScale;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = Quaternion.Euler(0, startAngle + (i * angleIncrement), 0) * shootPoint.forward;
                rb.linearVelocity = direction * bulletSpeed;
            }
        }
        
        StatsManager.Instance.OnWeaponShot?.Invoke(1);
    }
}