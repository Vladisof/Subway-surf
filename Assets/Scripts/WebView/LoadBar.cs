using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace WebView
{
    public class LoadBar : MonoBehaviour
    {
        [SerializeField] private Image fillImg;
        [SerializeField] private float loadingTime;
        
        private float _currentTime;

        private event Action OnEnd;
        public event Action OnLoadingComplete;

        public void SetAnimationCallback(Action action) =>
            OnEnd = action;

        public void PlayBarAnimation() =>
            StartCoroutine(IE_Load());

        public void StopBarAnimation() =>
            StopAllCoroutines();

        private IEnumerator IE_Load()
        {
            while (_currentTime < loadingTime)
            {
               // Debug.Log("Loading...");
                _currentTime += Time.deltaTime;
                float progress = _currentTime / loadingTime;
                fillImg.fillAmount = progress;
                yield return null;
            }
            
            OnLoadingComplete?.Invoke();
        }
    }
}