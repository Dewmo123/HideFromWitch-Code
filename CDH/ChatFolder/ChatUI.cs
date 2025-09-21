
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _00.Work.CDH.Code.ChatFolder
{
    public class ChatUI : MonoBehaviour
    {
        [Header("Chat Close Setting")]
        [SerializeField] private float chatCloseDuration;
        [Space]
        [Header("Chat UIs")]
        [SerializeField] private Image background;
        [SerializeField] private Image scrollView;
        [SerializeField] private Image scrollBar;
        [SerializeField] private Image inputField;

        private Color backgroundOriginColor;
        private Color scrollViewOriginColor;
        private Color scrollBarOriginColor;
        private Color inputFieldOriginColor;

        private void Awake()
        {
            backgroundOriginColor = background.color;
            scrollViewOriginColor = scrollView.color;
            scrollBarOriginColor = scrollBar.color;
            inputFieldOriginColor = inputField.color;
        }

        public void ChatOpen()
        {
            background.color = backgroundOriginColor;
            scrollView.color = scrollViewOriginColor;
            scrollBar.color = scrollBarOriginColor;
            inputField.color = inputFieldOriginColor;
        }

        public void StartClose(Action closeAfterHandler)
        {
            StartCoroutine(ChatClosing(closeAfterHandler));
        }

        private IEnumerator ChatClosing(Action closeAfterHandler)
        {
            float time = 0.0f;
            float percent = 0.0f;
            float tempAlpha = 0.0f;
            Color tempColor = Color.clear;

            float backgroundAlpha = background.color.a;
            float scrollViewAlpha = scrollView.color.a;
            float scrollBarAlpha = scrollBar.color.a;
            float inputFieldAlpha = inputField.color.a;

            while (chatCloseDuration > time)
            {
                time += Time.deltaTime;
                percent = time / chatCloseDuration;
                float easedPercent = percent * percent * percent;

                tempAlpha = Mathf.Lerp(backgroundAlpha, 0, easedPercent);
                tempColor = background.color;
                tempColor.a = tempAlpha;
                background.color = tempColor;
                tempAlpha = Mathf.Lerp(scrollViewAlpha, 0, easedPercent);
                tempColor = scrollView.color;
                tempColor.a = tempAlpha;
                scrollView.color = tempColor;
                tempAlpha = Mathf.Lerp(scrollBarAlpha, 0, easedPercent);
                tempColor = scrollBar.color;
                tempColor.a = tempAlpha;
                scrollBar.color = tempColor;
                tempAlpha = Mathf.Lerp(inputFieldAlpha, 0, easedPercent);
                tempColor = inputField.color;
                tempColor.a = tempAlpha;
                inputField.color = tempColor;

                yield return null;
            }

            closeAfterHandler?.Invoke();
        }

    }
}
