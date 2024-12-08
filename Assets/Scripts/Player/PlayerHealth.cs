using LTX.ChanneledProperties;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        private void OnEnable()
        {
            GameController.TimeScale.AddPriority(this, PriorityTags.None, 0f);
        }
        
        private void OnDisable()
        {
            GameController.TimeScale.RemovePriority(this);
        }

        public void OnPlayerDeath()
        {
            GameController.TimeScale.ChangeChannelPriority(this, PriorityTags.Highest);
            GameController.OnGameEnd?.Invoke();
        }
    }
}