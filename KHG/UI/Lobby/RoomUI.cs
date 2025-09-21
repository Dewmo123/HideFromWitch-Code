using AKH.Network;
using AKH.Scripts.Packet;
using Core.EventSystem;
using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using DewmoLib.Utiles;
using KHG.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHG.UIs
{
    public class RoomUI : MonoBehaviour, INeedInject
    {
        [Inject] private PacketResponsePublisher _publisher;
        [Inject] private PoolManagerMono _poolManager;
        [SerializeField] private Transform roomHandleTrm;
        [SerializeField] private EventChannelSO packetChannel;
        [SerializeField] private EventChannelSO uiChannel;

        [SerializeField] private PoolItemSO poolType;

        private List<RoomBar> _activeList = new();

        public bool Injected { get; set; }
        private AwaitableCompletionSource _waitInject = new();
        public AwaitableCompletionSource WaitInject => _waitInject;

        private async void Awake()
        {
            await ((INeedInject)this).Init();
            packetChannel.AddListener<RoomListEvent>(HandleRoomList);
            _publisher.AddListener(PacketID.C_RoomEnter, IsEnterSuccess);
        }
        private void OnDestroy()
        {
            packetChannel.RemoveListener<RoomListEvent>(HandleRoomList);
            _publisher.RemoveListener(PacketID.C_RoomEnter, IsEnterSuccess);
        }
        private void IsEnterSuccess(bool val)
        {
            if (val)
            {
                gameObject.SetActive(false);
                return;
            }

            WarnUiEvent warnUiEvent = UserInterfaceEvents.WarnUiEvent;
            warnUiEvent.Title = "오류";
            warnUiEvent.Message = "방 입장에 실패하였습니다.";

            uiChannel.InvokeEvent(warnUiEvent);
        }

        private void Start()
        {
            RequestRoomList();
        }

        public void CreateRoomList(List<RoomInfoPacket> list)
        {
            RemoveList();

            for (int i = 0; i < list.Count; i++)
            {
                StartCoroutine(Build(list[i], i * 0.07f));
            }
        }

        private IEnumerator Build(RoomInfoPacket roomInfo, float num)
        {
            yield return new WaitForSeconds(num);
            BuildRoom(roomInfo);
        }

        public void RequestRoomList()
        {
            C_RoomList c_RoomList = new C_RoomList();
            NetworkManager.Instance.SendPacket(c_RoomList);
        }

        private void BuildRoom(RoomInfoPacket roomInfo)
        {
            RoomBar listUI = _poolManager.Pop<RoomBar>(poolType);

            _activeList.Add(listUI);
            listUI.ResetItem(roomHandleTrm);
            listUI.SetRoomInfo(roomInfo);
        }

        private void HandleRoomList(RoomListEvent evt)
        {
            CreateRoomList(evt.infoPackets);
        }

        private void RemoveList()
        {
            foreach (var c in _activeList)
            {
                c.Push();
            }
            _activeList.Clear();
        }
    }
}
