using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Gameplay.Controllers
{
    public class TimerController : MonoBehaviour
    {
        public event Action OnOutOfTime;

        [Header("UI Elements")] 
        [SerializeField] TextMeshProUGUI timerText;

        [Header("Colors")] 
        [SerializeField] Color simpleColor = Color.white;
        [SerializeField] Color lowTimeColor = Color.red;

        [Header("Thresholds")] 
        [SerializeField] float lowTimeThreshold = 5;

        private float secondsLeft, pauseSeconds;

        private Coroutine timerCoroutine;

        public void SetTimer(float time)
        {
            secondsLeft = time;
        }

        public void StartTimer()
        {
            StopTimer();
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }

        public void StopTimer()
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }
        }

        IEnumerator TimerCoroutine()
        {
            while (secondsLeft > -1)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(secondsLeft);
                timerText.text = timeSpan.ToString("mm':'ss");
                timerText.color = secondsLeft > lowTimeThreshold ? simpleColor : lowTimeColor;
                yield return new WaitForSeconds(1);
                secondsLeft -= 1;
            }

            OnOutOfTime?.Invoke();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}