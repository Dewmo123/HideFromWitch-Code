using DewmoLib.Utiles;
using KHG.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KHG.UIs
{
    public abstract class UIManager : MonoBehaviour
    {
        [SerializeField] private EventChannelSO uiChannel;
        [SerializeField] private CoreUI DefaultPlayer;

        public void SetTitle(string msg, TitleMode mode, TitleData titleData)
        {
            var evt = UserInterfaceEvents.MessageEvent;
            evt.Data.mode = mode;
            evt.Message = msg;
            evt.Data = titleData;

            uiChannel.InvokeEvent(evt);
        }
    }
}
