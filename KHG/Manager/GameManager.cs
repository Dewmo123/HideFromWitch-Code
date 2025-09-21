using AKH.Scripts.Packet;
using Assets._00.Work.CDH.Code.Sound;
using Assets._00.Work.YHB.Scripts.Core;
using Core.EventSystem;
using DewmoLib.Dependencies;
using DewmoLib.Utiles;
using KHG.Events;
using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour,INeedInject
{
    [SerializeField] private InputSO input;
    [SerializeField] private EventChannelSO packetChannel;
    [SerializeField] private EventChannelSO uiChannel;
    [SerializeField] private SoundPlayCompo bgmPlayer;
    [SerializeField] private SoundPlayCompo losePlayer;
    [SerializeField] private SoundPlayCompo winPlayer;

    [SerializeField] private Texture2D cursorTexture;
    [Inject] private PacketResponsePublisher _publisher;
    public bool Injected { get; set; } = false;

    private AwaitableCompletionSource _waitInject = new();
    public AwaitableCompletionSource WaitInject => _waitInject;

    private async void Awake()
    {
        await ((INeedInject)this).Init();
        _publisher.AddListener(PacketID.C_GameStart, HandleGameStart);
        input.OnMouseLockEvent += HandleMouseLock;
        packetChannel.AddListener<GameFinishEvent>(HandleGameFinish);

        Cursor.SetCursor(cursorTexture, Vector2.one * 18, CursorMode.ForceSoftware);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void HandleGameStart(bool obj)
    {
        if (!obj)
        {
            var evt = UserInterfaceEvents.MessageEvent;
            evt.Data = new() { fadeInTime = 0, fadeOutTime = 0.3f, lifeTime = 3, mode = TitleMode.SubTitle };
            evt.Message = "최소플레이 인원은 6명 입니다.";
            packetChannel.InvokeEvent(evt);
            bgmPlayer.PlayBGM();
        }
    }

    private void OnDestroy()
    {
        packetChannel.RemoveListener<GameFinishEvent>(HandleGameFinish);
        _publisher.RemoveListener(PacketID.C_GameStart, HandleGameStart);
        input.OnMouseLockEvent -= HandleMouseLock;
    }

    private void HandleGameFinish(GameFinishEvent evt)
    {
        if (evt.Win)
            winPlayer.PlaySound();
        else
            losePlayer.PlaySound();
    }

    private void HandleMouseLock(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
