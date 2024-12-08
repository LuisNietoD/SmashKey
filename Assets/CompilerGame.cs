using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;

public class CompilerGame : MonoBehaviour
{
    public RectTransform pointer;      // Pointer en tant que RectTransform
    public GameObject bugPrefab;       // Prefab du bug (avec RectTransform)
    public RectTransform gameArea;     // Zone délimitant la surface de jeu
    public int bugCount = 10;          // Nombre de bugs à générer
    public float moveDuration = 0.5f;  // Durée de déplacement du pointer

    public List<GameObject> bugs = new List<GameObject>(); // Liste des bugs
    private int score = 0;
    public float pointerSpeed = 3f;
    private bool start  = false;
    private float finalPos;
    public TextMeshProUGUI multiplierText;
    private void Start()
    {
        finalPos = -pointer.anchoredPosition.x;
        SpawnBugs();
    }

    private void SpawnBugs()
    {
        for (int i = 0; i < bugCount; i++)
        {
            GameObject bug = Instantiate(bugPrefab, gameArea);

            Vector2 newPosition = new Vector2(bugs[i].GetComponent<RectTransform>().anchoredPosition.x + Random.Range(120, 190), 0);

            RectTransform bugTransform = bug.GetComponent<RectTransform>();
            if (bugTransform != null)
            {
                bugTransform.anchoredPosition = newPosition;
            }

            bugs.Add(bug);
        }
    }

    private void Update()
    {
        if(pointer.anchoredPosition.x >= finalPos)
            return;
        
        if (start)
        {
            pointer.anchoredPosition += new Vector2(pointerSpeed* Time.deltaTime, 0);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (start)
            {
                Debug.Log(pointer.anchoredPosition.x);
                GameObject b = TestCollision();
                if (b != null)
                {
                    Debug.Log(b.name);
                    bugs.Remove(b);
                    Destroy(b);
                    GameMetrics.Global.AddMultiply();
                }
                else
                {
                    GameMetrics.Global.RemoveMultiply();
                }

                if (GameMetrics.Global.Multiply >= 0)
                {
                    multiplierText.text ="X" + GameMetrics.Global.Multiply.ToString();
                }
                else
                {
                    multiplierText.text = "/" + GameMetrics.Global.Multiply.ToString();
                }
            }
            
            start = true;
            pointer.transform.SetAsLastSibling();
        }
    }

    private GameObject TestCollision()
    {
        float x = pointer.anchoredPosition.x;

        foreach (var bug in bugs)
        {
            RectTransform b = bug.GetComponent<RectTransform>();
            float left = b.anchoredPosition.x - b.rect.width / 2;
            float right = b.anchoredPosition.x + b.rect.width / 2;

            if (x >= left && x <= right)
            {
                return bug;
            }
        }
        return null;
    }
}
