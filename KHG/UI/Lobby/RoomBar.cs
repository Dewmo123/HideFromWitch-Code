using AKH.Network;
using DewmoLib.ObjectPool.RunTime;
using DewmoLib.Utiles;
using KHG.Events;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomBar : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI roomNameTmp;
    [SerializeField] private TextMeshProUGUI roomCountTmp;
    [SerializeField] private TextMeshProUGUI roomOwnerTmp;
    [SerializeField] private EventChannelSO uiChannel;

    [SerializeField] private Button connectButton;

    [SerializeField] private PoolItemSO serverListItemSO;

    private Pool _currentPool;
    private int _roomId;
    public string Name
    {
        get => gameObject.name;
        set => gameObject.name = value;
    }

    public PoolItemSO PoolItem => serverListItemSO;

    public GameObject GameObject => gameObject;

    public void SetRoomInfo(RoomInfoPacket info)
    {
        CheckPlayerCount(info.currentCount,info.maxCount);

        Name = $"Room_{info.roomId}";
        roomNameTmp.SetText(info.roomName);
        roomCountTmp.SetText($"{info.currentCount}/{info.maxCount}");
        roomOwnerTmp.SetText(info.hostName);

        _roomId = info.roomId;
    }

    private void CheckPlayerCount(int cur,int max)
    {
        connectButton.interactable = cur < max;
    }

    public void ConnectRoom()
    {
        if (_roomId == 0) return;

        Connect();
    }

    public void Push()
    {
        if (_currentPool != null) _currentPool.Push(this);
        else Destroy(gameObject);
    }

    private void Connect()
    {
        C_RoomEnter c_RoomEnter = new C_RoomEnter() { roomId = _roomId };

        NetworkManager.Instance.SendPacket(c_RoomEnter);

        uiChannel.InvokeEvent(UserInterfaceEvents.ServerConnectEvent);
    }

    public void SetUpPool(Pool pool)
    {
        _currentPool = pool;
    }

    public void ResetItem()
    {
        roomNameTmp.text = "N/A";
        roomCountTmp.text = string.Empty;
        roomOwnerTmp.text = string.Empty;
        _roomId = 0;
    }

    public void ResetItem(Transform listParent)
    {
        roomNameTmp.text = "N/A";
        transform.parent = listParent;
        transform.localScale = Vector3.one;
        transform.rotation = transform.parent.rotation;
        transform.localPosition = Vector3.zero;
        roomCountTmp.text = string.Empty;
        roomOwnerTmp.text = string.Empty;
        _roomId = 0;
    }
}
