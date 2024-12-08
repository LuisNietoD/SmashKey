using UnityEngine;

public class Shotgun : MonoBehaviour, IWeapon
{
    public int damage => GameMetrics.Global.Shotgun_Damage;
    private float cooldownTime => GameMetrics.Global.Shotgun_CooldownTime;
    private float actualCooldown = 0;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    private float bulletSpeed => GameMetrics.Global.Shotgun_BulletSpeed;
    private Vector3 bulletScale => GameMetrics.Global.Shotgun_BulletScale;
    private int bulletCount => GameMetrics.Global.Shotgun_BulletCount;
    private float spreadAngle => GameMetrics.Global.Shotgun_SpreadAngle;

    private void Update()
    {
        actualCooldown -= Time.deltaTime;
    }

    public void AutoShoot()
    {
        if (actualCooldown > 0 || !GameMetrics.Global.Shotgun_Enabled)
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