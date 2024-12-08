using DG.Tweening;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] private List<GameObject> allPanels;

    private void OnEnable()
    {
        SetElements();
        StartCoroutine(DisplayElementsSequentially());
    }
    
    private void SetElements()
    {
        foreach (GameObject panel in allPanels)
        {
            panel.gameObject.SetActive(false);
        }
    }

    private IEnumerator DisplayElementsSequentially()
    {
        foreach (GameObject element in allPanels)
        {
            element.gameObject.SetActive(true);
            
            DisplayElementEffect(element.gameObject);
            
            yield return new WaitForSeconds(0.3f * (element.transform.childCount - 1));
        }
    }

    private void DisplayElementEffect(GameObject uiElement)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(uiElement.transform.DOScale(1.2f, 0.1f))
                .Append(uiElement.transform.DOScale(0.8f, 0.1f))
                .Append(uiElement.transform.DOScale(1f, 0.1f));
    }
}
