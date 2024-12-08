using UnityEngine;

public class Minigun : MonoBehaviour, IWeapon
{
    public int damage => GameMetrics.Global.Minigun_Damage;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    private float bulletSpeed => GameMetrics.Global.Minigun_BulletSpeed;
    private Vector3 bulletScale => GameMetrics.Global.Minigun_BulletScale;
    private float fireRate => GameMetrics.Global.Minigun_FireRate;

    public float spreadAngle = 30f;
    private float currentSpreadAngle = 0f;
    private bool pingPongDirection = true;
    private float lastShotTime = 0f;
    
    public void AutoShoot()
    {
        if (bulletPrefab == null || shootPoint == null || !GameMetrics.Global.Minigun_Enabled) return;

        if (Time.time - lastShotTime >= fireRate)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.transform.localScale = bulletScale;
            bullet.GetComponent<Bullet>().Initialize(5, damage);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = Quaternion.Euler(0, currentSpreadAngle, 0) * shootPoint.forward;
                rb.linearVelocity = direction * bulletSpeed;
            }

            UpdateSpread();
            lastShotTime = Time.time;
            
            StatsManager.Instance.OnWeaponShot?.Invoke(1);
        }
    }

    private void UpdateSpread()
    {
        if (pingPongDirection)
        {
            currentSpreadAngle += spreadAngle * Time.deltaTime * (1 / fireRate);
            if (currentSpreadAngle >= spreadAngle)
            {
                currentSpreadAngle = spreadAngle;
                pingPongDirection = false;
            }
        }
        else
        {
            currentSpreadAngle -= spreadAngle * Time.deltaTime * (1 / fireRate);
            if (currentSpreadAngle <= -spreadAngle)
            {
                currentSpreadAngle = -spreadAngle;
                pingPongDirection = true;
            }
        }
    }
}