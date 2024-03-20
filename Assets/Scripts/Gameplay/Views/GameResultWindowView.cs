using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views
{
    public class GameResultWindowView : MonoBehaviour
    {
        public event Action OnRestartButtonClicked;
        public event Action OnExitButtonClicked;

        [Header("Texts")]
        [SerializeField] TMP_Text tableClearedText;
        [SerializeField] TMP_Text poolOverflowText;
        [SerializeField] TMP_Text outOfTimeText;

        [Header("Buttons")]
        [SerializeField] Button restartButton;
        [SerializeField] Button exitButton;

        [Header("Animation settings")]
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] float unfadeDuration = 0.3f;

        public void ShowResult(FinishReason reason)
        {
            switch (reason)
            {
                case FinishReason.TableCleared:
                    tableClearedText.gameObject.SetActive(true);
                    break;
                case FinishReason.PoolOverflow:
                    poolOverflowText.gameObject.SetActive(true);
                    break;
                case FinishReason.OutOfTime:
                    outOfTimeText.gameObject.SetActive(true);
                    break;
            }
        }

        private void Start()
        {
            restartButton.onClick.AddListener(() => OnRestartButtonClicked?.Invoke());
            exitButton.onClick.AddListener(() => OnExitButtonClicked?.Invoke());
        }

        private void OnEnable()
        {
            UnfadeAsync(destroyCancellationToken).Forget();
        }

        private async UniTask UnfadeAsync(CancellationToken token)
        {
            canvasGroup.alpha = 0;

            float elapsedTime = 0;
            while (elapsedTime < unfadeDuration)
            {
                await UniTask.Yield(token);
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = elapsedTime / unfadeDuration;
            }

            canvasGroup.alpha = 1;
        }
    }
}