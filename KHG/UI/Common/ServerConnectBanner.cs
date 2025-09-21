using Core.EventSystem;
using DewmoLib.Dependencies;
using DewmoLib.Utiles;
using KHG.Events;
using System.Collections;
using UnityEngine;

namespace KHG.UIs
{
    public class ServerConnectBanner : PanelController
    {
        [SerializeField] private float pendingTime;
        [SerializeField] private Panel connectPanel;

        [SerializeField] private EventChannelSO uiChannel;
        public override void Close()
        {
            connectPanel.SetActive(false);

            var evt = UserInterfaceEvents.WarnUiEvent;
            evt.Title = "���� ����";
            evt.Message = "�۾� �ð� �ʰ�\n���ͳ� ���� �Ǵ� ������ �����ִ��� Ȯ�����ּ���";
            uiChannel.InvokeEvent(evt);
        }

        public override void Open()
        {
            connectPanel.gameObject.SetActive(true);
            connectPanel.SetActive(true);

            StartCoroutine(ConnectFailed());
        }

        private IEnumerator ConnectFailed()
        {
            yield return new WaitForSecondsRealtime(pendingTime);
            Close();
        }
    }
}
