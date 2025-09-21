using DewmoLib.Utiles;
using KHG.Events;
using KHG.UIs;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Jumpscare : MonoBehaviour
{
    [SerializeField] private EventChannelSO uiChannel;
    public void SetActivate(bool value)
    {
        if(value) PopupWarn();
    }

    private void PopupWarn()
    {
        WarnUiEvent evt = UserInterfaceEvents.WarnUiEvent;
        evt.Title = "경고.";
        evt.Message = "해당 설정은 정신적 고통을 초래할 수 있으며,\n그 결과에 따른 모든 책임은 본인에게 있음을 알려드립니다.";


        uiChannel.InvokeEvent(evt);
    }
}
