using UnityEngine;

public class Minigun : MonoBehaviour, IWeapon
{
    public int damage { get; } = 5;
    public float cooldownTime = 0;
    public float actualCooldown = 0;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 20f;
    public Vector3 bulletScale = new Vector3(1f, 1f, 1f);
    public float spreadAngle = 30f;
    public float fireRate = 0.1f;

    private float currentSpreadAngle = 0f;
    private bool pingPongDirection = true;
    private float lastShotTime = 0f;

    public void AutoShoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        if (Time.time - lastShotTime >= fireRate)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.transform.localScale = bulletScale;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = Quaternion.Euler(0, currentSpreadAngle, 0) * shootPoint.forward;
                rb.linearVelocity = direction * bulletSpeed;
            }

            UpdateSpread();
            lastShotTime = Time.time;
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