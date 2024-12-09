using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GameMetrics", menuName = "GameMetrics")]
public class GameMetrics : ScriptableObject
{
    public static GameMetrics Global => GameController.Metrics;
    
    [field : SerializeField] public float platformTravelTime { get; private set; } = -10f;
    [field : SerializeField] public string LeaderboardID { get; private set; }
    
    [field : SerializeField] public int LetterCount { get; private set; }
    [field : SerializeField] public int Multiply { get; private set; }
    [field : SerializeField] public int Health { get; private set; }
    public void AddLetter() => LetterCount++;
    public void AddMultiply() => Multiply++;
    public void RemoveMultiply() => Multiply--;
    public void ResetLetters() => LetterCount = 0;

    [field: SerializeField, FoldoutGroup("Grenade Launcher")]
    public bool GrenadeLauncher_Enabled { get; private set; } = true;
    [field: SerializeField, FoldoutGroup("Grenade Launcher")]
    public int GrenadeLauncher_Damage { get; private set; } = 10;
    [field: SerializeField, FoldoutGroup("Grenade Launcher")] 
    public float GrenadeLauncher_CooldownTime { get; private set; } = 1;
    [field: SerializeField, FoldoutGroup("Grenade Launcher")] 
    public int GrenadeLauncher_GrenadeCount { get; private set; } = 5;
    [field: SerializeField, FoldoutGroup("Grenade Launcher")] 
    public int GrenadeLauncher_ExplosionRadius { get; private set; } = 5;
    
    [field: SerializeField, FoldoutGroup("Laser")]
    public bool Laser_Enabled { get; private set; } = true;
    [field: SerializeField, FoldoutGroup("Laser")]
    public int Laser_Damage { get; private set; } = 10;
    [field: SerializeField, FoldoutGroup("Laser")] 
    public float Laser_CooldownTime { get; private set; } = 5;
    [field: SerializeField, FoldoutGroup("Laser")] 
    public int Laser_Lifetime { get; private set; } = 1;
    
    [field: SerializeField, FoldoutGroup("Minigun")] 
    public bool Minigun_Enabled { get; private set; } = true;
    [field: SerializeField, FoldoutGroup("Minigun")]
    public int Minigun_Damage { get; private set; } = 10;
    [field: SerializeField, FoldoutGroup("Minigun")] 
    public float Minigun_FireRate { get; private set; } = 0.1f;
    [field: SerializeField, FoldoutGroup("Minigun")] 
    public float Minigun_BulletSpeed { get; private set; } = 20;
    [field: SerializeField, FoldoutGroup("Minigun")] 
    public Vector3 Minigun_BulletScale { get; private set; } = new Vector3(1f, 1f, 1f);

    [field: SerializeField, FoldoutGroup("Pistol")]
    public bool Pistol_Enabled { get; private set; } = true;
    [field: SerializeField, FoldoutGroup("Pistol")]
    public int Pistol_Damage { get; private set; } = 10;
    [field: SerializeField, FoldoutGroup("Pistol")] 
    public float Pistol_CooldownTime { get; private set; } = 1.2f;
    [field: SerializeField, FoldoutGroup("Pistol")] 
    public float Pistol_BulletSpeed { get; private set; } = 20;
    [field: SerializeField, FoldoutGroup("Pistol")] 
    public Vector3 Pistol_BulletScale { get; private set; } = new Vector3(1, 1, 1);
    
    [field: SerializeField, FoldoutGroup("Shotgun")] 
    public bool Shotgun_Enabled { get; private set; } = true;
    [field: SerializeField, FoldoutGroup("Shotgun")]
    public int Shotgun_Damage { get; private set; } = 10;
    [field: SerializeField, FoldoutGroup("Shotgun")] 
    public float Shotgun_CooldownTime { get; private set; } = 0.5f;
    [field: SerializeField, FoldoutGroup("Shotgun")] 
    public float Shotgun_SpreadAngle { get; private set; } = 15f;
    [field: SerializeField, FoldoutGroup("Shotgun")] 
    public float Shotgun_BulletSpeed { get; private set; } = 20;
    [field: SerializeField, FoldoutGroup("Shotgun")] 
    public int Shotgun_BulletCount { get; private set; } = 3;
    [field: SerializeField, FoldoutGroup("Shotgun")] 
    public Vector3 Shotgun_BulletScale = new Vector3(1, 1, 1);

    [field: SerializeField, FoldoutGroup("Symmetrical Shooter")]
    public bool Symmetrical_Enabled { get; private set; } = true;
    [field: SerializeField, FoldoutGroup("Symmetrical Shooter")]
    public int Symmetrical_Damage { get; private set; } = 10;
    [field: SerializeField, FoldoutGroup("Symmetrical Shooter")] 
    public float Symmetrical_CooldownTime { get; private set; } = 0.5f;
    [field: SerializeField, FoldoutGroup("Symmetrical Shooter")]
    public float Symmetrical_BulletSpeed { get; private set; } = 10;
    [field: SerializeField, FoldoutGroup("Symmetrical Shooter")]
    public int Symmetrical_BulletCount { get; private set; } = 2;
    [field: SerializeField, FoldoutGroup("Symmetrical Shooter")]
    public Vector3 Symmetrical_BulletScale { get; private set; } = new Vector3(1, 1, 1);
    [field: SerializeField, FoldoutGroup("Symmetrical Shooter")]
    public float Symmetrical_SpawnOffset { get; private set; } = 1f;
    [field: SerializeField, FoldoutGroup("Symmetrical Shooter")]
    public float Symmetrical_OffsetBetweenBullets { get; private set; } = 1f;
}