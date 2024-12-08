using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stats
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject stats;
        
        private void OnEnable()
        {
            GameController.OnGameEnd += SetStats;
        }

        private void OnDisable()
        {
            GameController.OnGameEnd -= SetStats;
        }

        private void SetStats()
        {
            stats.SetActive(true);
        }

        public void OpenScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}