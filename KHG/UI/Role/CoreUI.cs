using AKH.Network;
using AKH.Scripts.Packet;
using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.Utiles;
using System;
using UnityEngine;

namespace KHG.UIs
{
    public class CoreUI : MonoBehaviour
    {
        [SerializeField] private TimerController timer;
        [SerializeField] private MenuBar menuBar;
        [SerializeField] private GameObject gameStartButton;
        [SerializeField] private EventChannelSO packetChannel;
        [field: SerializeField] public InputSO inputSO { get; private set; }

        public bool Injected { get; set; }

        private ICoreUI[] coreUis;
        private AwaitableCompletionSource _waitInject = new();
        public AwaitableCompletionSource WaitInject => _waitInject;

        private void Awake()
        {
            packetChannel.AddListener<SyncTimer>(SetTime);
            packetChannel.AddListener<GameStateChangeEvent>(HandleGameChange);
            inputSO.OnEscapeEvent += HandleEscpePressed;

            //Cursor.lockState = CursorLockMode.Locked;

            coreUis = GetComponentsInChildren<ICoreUI>();
            InitCoreUis();
        }

        private void HandleGameChange(GameStateChangeEvent evt)
        {
            print($"호스트 버튼설정중:{HostManager.Instance.IsHost}");
            if (evt.State == RoomState.Lobby && HostManager.Instance.IsHost)
                gameStartButton.SetActive(true);
            else
                gameStartButton.SetActive(false);
        }

        private void OnDestroy()
        {
            packetChannel.RemoveListener<SyncTimer>(SetTime);
            packetChannel.RemoveListener<GameStateChangeEvent>(HandleGameChange);
            inputSO.OnEscapeEvent -= HandleEscpePressed;
        }

        private void InitCoreUis()
        {
            foreach (var ui in coreUis)
            {
                ui.Initialize(this);
            }
        }

        private void HandleEscpePressed()
        {
            menuBar.EscapeCalled();
        }

        public void HostStartButtonPressed()
        {
            NetworkManager.Instance.SendPacket(new C_GameStart());
        }

        public void SetTime(SyncTimer evt)
        {
            float time = evt.remaintime;
            timer.SetTime(time);
        }
    }
}