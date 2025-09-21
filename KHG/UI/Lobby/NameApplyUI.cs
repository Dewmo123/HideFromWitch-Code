using DewmoLib.Utiles;
using KHG.Events;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;

namespace KHG.UIs
{
    public class NameApplyUI : MonoBehaviour
    {
        [SerializeField] private GameObject setNameUI;
        [SerializeField] private GameObject menuButtonUI;
        [SerializeField] private GameObject roomListUI;
        [SerializeField] private TextMeshProUGUI inputText;
        [SerializeField] private EventChannelSO playerInfoChannel;
        [SerializeField] private EventChannelSO uiChannel;

        private string nickname;
        public void ApplyName()
        {
            if (inputText.text.Length < 2)
            {
                SendWarnning();
                return;
            }
            nickname = inputText.text;
            SendName();
            menuButtonUI?.SetActive(false);
            setNameUI?.SetActive(false);
        }

        private void SendWarnning()
        {
            MessageEvent evt = UserInterfaceEvents.MessageEvent;
            evt.Data = new TitleData
            {
                fadeInTime = 0,
                lifeTime = 1,
                fadeOutTime = 0.3f,
                mode = TitleMode.SubTitle
            };
            evt.Message = "닉네임은 최소 2글자 이상이여야 합니다!";
            uiChannel.InvokeEvent(evt);
        }

        private void SendName()
        {
            PlayerNameEvent evt = PlayerInfoEvents.PlayerNameEvent;
            evt.Name = nickname;
            playerInfoChannel.InvokeEvent(evt);

            roomListUI?.SetActive(true);
            print("name apply:" + evt.Name);
        }
    }

}