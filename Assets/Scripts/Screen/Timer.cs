using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    float timer = 15f;
    private TMP_Text txt;

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }
    
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        txt.text = timer.ToString("00");

        if (timer <= 0f)
        {
            int letterScore = GameMetrics.Global.LetterCount;
            if (GameMetrics.Global.Multiply >= 0)
            {
                letterScore *= GameMetrics.Global.Multiply;
            }
            else
            {
                letterScore /= -GameMetrics.Global.Multiply;
            }
            switch (letterScore)
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
