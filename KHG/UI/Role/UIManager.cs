using DewmoLib.Utiles;
using KHG.Events;
using UnityEngine;

namespace KHG.UIs
{
    public abstract class UIManager : MonoBehaviour
    {
        [SerializeField] private EventChannelSO uiChannel;

        public void SetTitle(string msg, TitleMode mode, TitleData titleData)
        {
            titleData.mode = mode;
            var evt = new MessageEvent
            {
                Message = msg,
                Data = titleData
            };

            uiChannel.InvokeEvent(evt);
        }
    }
}
