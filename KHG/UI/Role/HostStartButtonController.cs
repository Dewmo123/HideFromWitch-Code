using AKH.Scripts.Packet;
using DewmoLib.Utiles;
using KHG.Managers;
using KHG.UIs;
using UnityEngine;

public class HostStartButtonController : MonoBehaviour, ICoreUI
{
    [SerializeField] private EventChannelSO packetChannel;
    [SerializeField] private GameObject startButtonObject;

    private ICoreUIContext _coreUIContext;

    public void Initialize(ICoreUIContext coreUIContext)
    {
        _coreUIContext = coreUIContext;
        packetChannel.AddListener<GameStateChangeEvent>(HandleGameStateChanged);
        RefreshButton(false);
    }

    private void OnDestroy()
    {
        packetChannel.RemoveListener<GameStateChangeEvent>(HandleGameStateChanged);
    }

    public void StartButtonPressed()
    {
        _coreUIContext?.RequestGameStart();
    }

    private void HandleGameStateChanged(GameStateChangeEvent evt)
    {
        RefreshButton(evt.State == RoomState.Lobby);
    }

    private void RefreshButton(bool isLobbyState)
    {
        bool canStart = isLobbyState && HostManager.Instance.IsHost;
        startButtonObject.SetActive(canStart);
    }
}
