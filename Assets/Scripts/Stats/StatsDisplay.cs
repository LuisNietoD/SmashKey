using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private TMP_Text enemiesDamageDealt;
    [SerializeField] private TMP_Text enemiesSpawned;
    
    [Header("Player")]
    [SerializeField] private TMP_Text enemiesKilled;
    [SerializeField] private TMP_Text playerDamageDealt;
    [SerializeField] private TMP_Text weaponsShot;
    [SerializeField] private TMP_Text keysTapped;
    
    [Header("World")]
    [SerializeField] private TMP_Text distanceTravelled;
    [SerializeField] private TMP_Text timePlayed;
    
    private void OnEnable()
    {
        enemiesDamageDealt.text = StatsManager.Instance.EnemyStats.totalDamageDealt.ToString();
        enemiesSpawned.text = StatsManager.Instance.EnemyStats.totalEnemiesSpawned.ToString();

        enemiesKilled.text = StatsManager.Instance.PlayerStats.totalKilled.ToString();
        playerDamageDealt.text = StatsManager.Instance.PlayerStats.totalDamageDealt.ToString();
        weaponsShot.text = StatsManager.Instance.PlayerStats.weaponsShot.ToString();
        keysTapped.text = StatsManager.Instance.PlayerStats.totalKeysTapped.ToString();

        distanceTravelled.text = StatsManager.Instance.WorldStats.totalDistanceTravelled.ToString();
        timePlayed.text = StatsManager.Instance.WorldStats.totalTimePlayed.ToString();
    }
}
