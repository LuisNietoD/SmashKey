using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        GetComponent<TextMeshProUGUI>().text = timer.ToString("00");

        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
