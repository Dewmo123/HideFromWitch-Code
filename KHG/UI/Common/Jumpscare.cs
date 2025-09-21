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
        evt.Title = "���.";
        evt.Message = "�ش� ������ ������ ������ �ʷ��� �� ������,\n�� ����� ���� ��� å���� ���ο��� ������ �˷��帳�ϴ�.";


        uiChannel.InvokeEvent(evt);
    }
}
