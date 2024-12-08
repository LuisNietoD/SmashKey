using System;
using LootLocker.Requests;
using LTX.ChanneledProperties;
using UnityEngine;

public static class GameController
{
    public static Action OnGameEnd;
    
    private static GameMetrics gameMetrics;
    public static GameMetrics Metrics
    {
        get
        {
            if (!gameMetrics)
                gameMetrics = Resources.Load<GameMetrics>("GameMetrics");

            return gameMetrics;
        }

        set => gameMetrics = value;
    }
    
    public static PrioritisedProperty<float> TimeScale { get; private set; }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Load()
    {
        Application.wantsToQuit += UnLoad;
        Application.targetFrameRate = 60;
        
        TimeScale.AddOnValueChangeCallback(UpdateTimeScale, true);
    }

    private static bool UnLoad()
    {
        return true;
    }
    
    private static void UpdateTimeScale(float newScale)
    {
        Time.timeScale = newScale;
    }
}