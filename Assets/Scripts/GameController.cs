using System;
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
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Load()
    {
        Application.wantsToQuit += UnLoad;
        Application.targetFrameRate = 60;
    }

    private static bool UnLoad()
    {
        return true;
    }

    public static void GameEnd()
    {
        OnGameEnd?.Invoke();
    }
}