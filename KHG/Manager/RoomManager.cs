using AKH.Network;
using Core.EventSystem;
using DewmoLib.Dependencies;
using DewmoLib.Utiles;
using KHG.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KHG.UIs
{
    public class RoomManager : MonoBehaviour, INeedInject
    {
        [Inject] private PacketResponsePublisher _publisher;
        [SerializeField] private EventChannelSO packetChannel;
        [SerializeField] private EventChannelSO uiChannel;

        [SerializeField] private PanelController connectPanelController;
        [Space]
        [SerializeField] private string roomName = "AKH";

        public bool Injected { get; set; } = false;
        private AwaitableCompletionSource _waitInject = new();
        public AwaitableCompletionSource WaitInject => _waitInject;

        private async void Awake()
        {
            await ((INeedInject)this).Init();
            _publisher.AddListener(PacketID.C_RoomEnter, IsEnterSuccess);
            _publisher.AddListener(PacketID.C_CreateRoom, IsEnterSuccess);
            uiChannel.AddListener<ServerConnectEvent>(HandleConnectReq);
        }

        private void OnDestroy()
        {
            _publisher.RemoveListener(PacketID.C_RoomEnter, IsEnterSuccess);
            _publisher.RemoveListener(PacketID.C_CreateRoom, IsEnterSuccess);
            uiChannel.RemoveListener<ServerConnectEvent>(HandleConnectReq);
        }

        private async void IsEnterSuccess(bool value)
        {
            //connectPanelController.Close();
            if (value)
            {
                await SceneManager.LoadSceneAsync(roomName);
                NetworkManager.Instance.SendPacket(new C_ResourceLoadComplete());
            }
        }

        private void HandleConnectReq(ServerConnectEvent evt)
        {
            connectPanelController.Open();
        }
    }
}
