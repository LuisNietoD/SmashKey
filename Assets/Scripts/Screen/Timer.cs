using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    float timer = 0f;
    private TMP_Text txt;

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }
    
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        txt.text = timer.ToString("00");

        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (timer >= 15f)
        {
            switch (GameMetrics.Global.LetterCount)
            {
                case 0:
                    GameController.Metrics = Resources.Load<GameMetrics>("Metrics/Low");
                    break;
                case >2:
                    GameController.Metrics = Resources.Load<GameMetrics>("Metrics/Default");
                    break;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
