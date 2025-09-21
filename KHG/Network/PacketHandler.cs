using AKH.Scripts.Packet;
using Assets._00.Work.YHB.Scripts.Players;
using KHG.Events;
using KHG.Managers;
using ServerCore;
using UnityEngine;
public enum RoomState
{
    Lobby,
    Ready,
    InGame
}
public partial class PacketHandler
{
    RoomState _currentState = RoomState.Lobby;
    public void S_RoomListHandler(PacketSession session, IPacket packet)
    {
        S_RoomList s_RoomList = (S_RoomList)packet;

        RoomListEvent roomListEvent = PacketEvents.RoomListEvent;
        roomListEvent.infoPackets = s_RoomList.roomInfos;
        _packetChannel.InvokeEvent(roomListEvent);
    }
    public void S_HostChangeHandler(PacketSession session, IPacket packet)
    {
        Debug.Log("SetHost");
        HostManager.Instance.SetHost();
        var evt = PacketEvents.GameStateChangeEvent;
        evt.State = _currentState;
        _packetChannel.InvokeEvent(evt);//귀찮아서 걍 함 ㅎㅎ
    }

    public void S_RoomStateChangeHandler(PacketSession session, IPacket packet)
    {
        S_RoomStateChange stateChange = (S_RoomStateChange)packet;
        RoomState state = (RoomState)stateChange.state;
        _currentState = state;
        string stateText = string.Empty;
        switch (state)
        {
            case RoomState.Ready:
                stateText = "사물들이 숨는 시간 입니다!";
                ShowText(0.3f, 3f, 2f, TitleMode.MainTitle, stateText);
                break;
            case RoomState.InGame:
                stateText = "술래가 풀려났습니다!!";
                ShowText(0.3f, 3f, 2f, TitleMode.MainTitle, stateText);
                break;
        }
        var evt = PacketEvents.GameStateChangeEvent;
        evt.State = state;
        _packetChannel.InvokeEvent(evt);//귀찮아서 걍 함 ㅎㅎ
    }
    public void S_GameFinishHandler(PacketSession session, IPacket packet)
    {
        S_GameEnd result = (S_GameEnd)packet;
        Role winner = (Role)result.winner;

        UnityEngine.Debug.Log("게임 끝남");
        ShowText(0.3f, 1f, 2f, TitleMode.MainTitle, winner == EntityManager.Instance.GetObject<Player>(_myIndex).Role ? "승리하였습니다!!" : "패배하였습니다..");
        var evt = PacketEvents.GameFinishEvent;
        evt.Win = winner == EntityManager.Instance.GetObject<Player>(_myIndex).Role ? true : false;
        _packetChannel.InvokeEvent(evt);
    }
}