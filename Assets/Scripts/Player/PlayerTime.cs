using UnityEngine;

namespace Player
{
    public class PlayerTime : MonoBehaviour
    {
        [SerializeField] private float distanceUnit;
        private float playedTime;

        private void OnEnable()
        {
            GameController.OnGameEnd += PlayerEnd;
        }

        private void Update()
        {
            playedTime += Time.deltaTime;
        }

        private void PlayerEnd()
        {
            StatsManager.Instance.OnDistanceTravelled?.Invoke(playedTime * distanceUnit);
            StatsManager.Instance.OnTimePlayed?.Invoke(playedTime);
        }
    }
}