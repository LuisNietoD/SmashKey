using UnityEngine;

public class GrenadeLauncher : MonoBehaviour, IWeapon
{
    public int damage { get; } = 10;
    public float cooldownTime = 1;
    private float actualCooldown = 0;
    public GameObject grenadePrefab;
    public Transform launchPoint;
    public int grenadeCount = 5;
    public float explosionRadius = 5f;
    public float launchForce = 10f;
    public float launchAngle = 45f;
    public float yExplode = 0.5f;

    private void Awake()
    {
        cooldownTime = GameMetrics.Global.GrenadeLauncher_CooldownTime;
        grenadeCount = GameMetrics.Global.GrenadeLauncher_GrenadeCount;
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
        
        LaunchGrenades();
        
        StatsManager.Instance.OnWeaponShot?.Invoke(1);
    }
    
    private void LaunchGrenades()
    {
        for (int i = 0; i < grenadeCount; i++)
        {
            LaunchGrenade();
        }
    }

    private void LaunchGrenade()
    {
        if (grenadePrefab == null || launchPoint == null) return;

        GameObject grenade = Instantiate(grenadePrefab, launchPoint.position, launchPoint.rotation);

        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 randomDirection = Quaternion.Euler(-launchAngle, Random.Range(-30f, 30f), 0) * launchPoint.forward;
            rb.AddForce(randomDirection * (launchForce + Random.Range(-2, 2)) , ForceMode.Impulse);
        }

        Grenade grenadeScript = grenade.GetComponent<Grenade>();
        if (grenadeScript != null)
        {
            grenadeScript.Initialize(yExplode, explosionRadius);
        }
    }
}
