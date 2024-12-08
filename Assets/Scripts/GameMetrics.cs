using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameMetrics", menuName = "GameMetrics")]
public class GameMetrics : ScriptableObject
{
    public static GameMetrics Global => GameController.Metrics;
    
    [field : SerializeField] public float platformTravelTime { get; private set; } = -10f;
    [field : SerializeField] public string LeaderboardID { get; private set; }
    
    [field : SerializeField] public int LetterCount { get; private set; }
    public void AddLetter() => LetterCount++;
    public void ResetLetters() => LetterCount = 0;

    [field: SerializeField, FoldoutGroup("Grenade Launcher")] 
    public float GrenadeLauncher_CooldownTime = 1;
    [field: SerializeField, FoldoutGroup("Grenade Launcher")] 
    public int GrenadeLauncher_GrenadeCount = 5;
    
    [field: SerializeField, FoldoutGroup("Laser")] 
    public float Laser_CooldownTime = 5;
    [field: SerializeField, FoldoutGroup("Laser")] 
    public int Laser_Lifetime = 1;
    
    [field: SerializeField, FoldoutGroup("Minigun")] 
    public float Minigun_FireRate = 0.1f;
    [field: SerializeField, FoldoutGroup("Minigun")] 
    public float Minigun_BulletSpeed = 20;
    [field: SerializeField, FoldoutGroup("Minigun")] 
    public Vector3 Minigun_BulletScale = new Vector3(1f, 1f, 1f);
    
    [field: SerializeField, FoldoutGroup("Pistol")] 
    public float Pistol_CooldownTime = 1.2f;
    [field: SerializeField, FoldoutGroup("Pistol")] 
    public float Pistol_BulletSpeed = 20;
    [field: SerializeField, FoldoutGroup("Pistol")] 
    public Vector3 Pistol_BulletScale = new Vector3(1, 1, 1);
    
    [field: SerializeField, FoldoutGroup("Shotgun")] 
    public float Shotgun_CooldownTime = 0.5f;
    [field: SerializeField, FoldoutGroup("Shotgun")] 
    public float Shotgun_SpreadAngle = 15f;
    [field: SerializeField, FoldoutGroup("Shotgun")] 
    public float Shotgun_BulletSpeed = 20;
    [field: SerializeField, FoldoutGroup("Shotgun")] 
    public int Shotgun_BulletCount = 3;
    [field: SerializeField, FoldoutGroup("Shotgun")] 
    public Vector3 Shotgun_BulletScale = new Vector3(1, 1, 1);
    
    [field: SerializeField, FoldoutGroup("Symmetrical Shooter")] 
    public float Symmetrical_CooldownTime = 0.5f;
    [field: SerializeField, FoldoutGroup("Symmetrical Shooter")]
    public float Symmetrical_BulletSpeed = 10;
    [field: SerializeField, FoldoutGroup("Symmetrical Shooter")]
    public int Symmetrical_BulletCount = 2;
    [field: SerializeField, FoldoutGroup("Symmetrical Shooter")]
    public Vector3 Symmetrical_BulletScale = new Vector3(1, 1, 1);
    [field: SerializeField, FoldoutGroup("Symmetrical Shooter")]
    public float Symmetrical_SpawnOffset = 1f;
    [field: SerializeField, FoldoutGroup("Symmetrical Shooter")]
    public float Symmetrical_OffsetBetweenBullets = 1f;
}