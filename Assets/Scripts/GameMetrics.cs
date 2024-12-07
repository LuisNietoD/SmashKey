using UnityEngine;

[CreateAssetMenu(fileName = "GameMetrics", menuName = "GameMetrics")]
public class GameMetrics : ScriptableObject
{
    public static GameMetrics Global => GameController.Metrics;
    
    [field : SerializeField] public float platformTravelTime { get; private set; } = -10f;
}