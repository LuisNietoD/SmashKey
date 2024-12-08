using UnityEngine;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour
{
    private SplineContainer splineContainer;
    private float movementDuration;
    private float timeElapsed = 0f;

    public void Initialize(SplineContainer spline, float duration)
    {
        splineContainer = spline;
        movementDuration = duration;
    }

    void Update()
    {
        if (splineContainer != null)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / movementDuration);
            transform.position = splineContainer.EvaluatePosition(t);
            if (timeElapsed / movementDuration > 1)
            {
                timeElapsed = 0;
            }
        }
    }
}