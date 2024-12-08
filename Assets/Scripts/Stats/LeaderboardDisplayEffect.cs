using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Stats
{
    public class LeaderboardDisplayEffect : MonoBehaviour
    {
        [SerializeField] private List<GameObject> allPanels;
        
        private void OnEnable()
        {
            foreach (GameObject panel in allPanels)
            {
                panel.gameObject.SetActive(false);
            }
            
            StartCoroutine(DisplayElementsSequentially());
        }
        
        private IEnumerator DisplayElementsSequentially()
        {
            yield return new WaitForSeconds(0.5f);
            
            foreach (GameObject element in allPanels)
            {
                element.gameObject.SetActive(true);
            
                DisplayElementEffect(element.gameObject);
            
                yield return new WaitForSeconds(0.15f * (element.transform.childCount));
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
}