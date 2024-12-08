using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Stats
{
    public class DisplayOnEnable : MonoBehaviour
    {
        [SerializeField] private float effectDuration;
        
        [SerializeField] private List<TMP_Text> allScores;
        [SerializeField] private List<TMP_Text> allElements;

        public UnityEvent GetValues;
        public Stat stat;
        
        private void OnEnable()
        {
            GetValues?.Invoke();
            SetElementsText();
            StartCoroutine(DisplayElementsSequentially());
        }
        
        private void SetElementsText()
        {
            foreach (TMP_Text element in allElements)
            {
                element.gameObject.SetActive(false);
            }
        }
        
        private IEnumerator DisplayElementsSequentially()
        {
            for (int index = 0; index < allElements.Count; index++)
            {
                TMP_Text element = allElements[index];
            
                element.gameObject.SetActive(true);
                DisplayElementEffect(element.gameObject);
                StartCoroutine(LerpScore(allScores[index], stat.GetValues()[index]));
            
                yield return new WaitForSeconds(0.3f);
            }
        }
        
        private void DisplayElementEffect(GameObject uiElement)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(uiElement.transform.DOScale(1.2f, 0.1f))
                .Append(uiElement.transform.DOScale(0.8f, 0.1f))
                .Append(uiElement.transform.DOScale(1f, 0.1f));
        }
    
        private IEnumerator LerpScore(TMP_Text text, float scoreText)
        {
            float elapsedTime = 0f;
            int startScore = 0;

            while (elapsedTime < effectDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / effectDuration;
                int displayedScore = Mathf.RoundToInt(Mathf.Lerp(startScore, scoreText, progress));
            
                text.text = displayedScore.ToString();

                yield return null;
            }
        
            text.text = scoreText.ToString();
        }
    }
}