using System.Collections.Generic;

[System.Serializable]
public struct PlayerStats
{
    public int totalKilled;
    public float totalDamageDealt;
    public int totalBulletsShot;
    public Dictionary<IWeapon, int> bulletsPerWeapon;
    public int totalKeysTapped;
}