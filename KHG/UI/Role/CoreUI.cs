using AKH.Network;
using AKH.Scripts.Packet;
using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.Utiles;
using UnityEngine;

namespace KHG.UIs
{
    public class CoreUI : MonoBehaviour, ICoreUIContext
    {
        [SerializeField] private TimerController timer;
        [SerializeField] private EventChannelSO packetChannel;
        [field: SerializeField] public InputSO inputSO { get; private set; }
        public InputSO Input => inputSO;

        public bool Injected { get; set; }

        private ICoreUI[] coreUis;
        private AwaitableCompletionSource _waitInject = new();
        public AwaitableCompletionSource WaitInject => _waitInject;

        private void Awake()
        {
            packetChannel.AddListener<SyncTimer>(SetTime);
            coreUis = GetComponentsInChildren<ICoreUI>();
            InitCoreUis();
        }

        private void OnDestroy()
        {
            packetChannel.RemoveListener<SyncTimer>(SetTime);
        }

        private void InitCoreUis()
        {
            foreach (var ui in coreUis)
            {
                ui.Initialize(this);
            }
        }

        public void RequestGameStart()
        {
            NetworkManager.Instance.SendPacket(new C_GameStart());
        }

        // Backward compatibility for existing button bindings in scenes/prefabs.
        public void HostStartButtonPressed()
        {
            RequestGameStart();
        }

        public void SetTime(SyncTimer evt)
        {
            float time = evt.remaintime;
            timer.SetTime(time);
        }
    }
}
