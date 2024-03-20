using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class LobbyView : MonoBehaviour
    {
        public Slider TimerSlider;
        public TextMeshProUGUI TimerText;
        public Slider CountSlider;
        public TextMeshProUGUI CountText;
        public Button PlayButton;

        public void SetTimer(float time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            TimerText.text = timeSpan.ToString("mm':'ss");
        }

        public void SetCount(int count)
        {
            CountText.text = $"{count}";
        }
    }
}