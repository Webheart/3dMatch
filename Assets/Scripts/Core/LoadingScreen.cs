using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        public Image Image;
        public float FadeInDuration = 0.5f;
        public float FadeOutDuration = 0.5f;
        public AnimationCurve FadeInCurve;
        public AnimationCurve FadeOutCurve;

        private void Start()
        {
            FadeOutAsync(destroyCancellationToken).Forget();
        }

        public async UniTask FadeInAsync(CancellationToken token)
        {
            gameObject.SetActive(true);

            SetColorAlpha(0);

            float elapsedTime = 0;
            while (elapsedTime < FadeInDuration)
            {
                await UniTask.Yield(token);
                elapsedTime += Time.deltaTime;
                SetColorAlpha(FadeInCurve.Evaluate(elapsedTime / FadeInDuration));
            }

            SetColorAlpha(1);
        }

        public async UniTask FadeOutAsync(CancellationToken token)
        {
            SetColorAlpha(1);

            float elapsedTime = 0;
            while (elapsedTime < FadeOutDuration)
            {
                await UniTask.Yield(token);
                elapsedTime += Time.deltaTime;
                SetColorAlpha(1 - FadeOutCurve.Evaluate(elapsedTime / FadeOutDuration));
            }

            SetColorAlpha(0);
            gameObject.SetActive(false);
        }

        private void SetColorAlpha(float alpha)
        {
            var color = Image.color;
            color.a = alpha;
            Image.color = color;
        }
    }
}