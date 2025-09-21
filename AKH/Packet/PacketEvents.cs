using DewmoLib.Utiles;
using System.Collections.Generic;

namespace AKH.Scripts.Packet
{
    public static class PacketEvents
    {
        public static readonly PacketResponse PacketResponse = new();
        public static readonly SyncTimer SyncTimer = new();
        public static readonly RoomListEvent RoomListEvent = new();
        public static readonly GameStateChangeEvent GameStateChangeEvent = new();
        public static readonly GameFinishEvent GameFinishEvent = new();
    }
    public class PacketResponse : GameEvent
    {
        public PacketID packetId;
        public bool success;
    }
    public class SyncTimer : GameEvent
    {
        public float remaintime;
    }
    public class RoomListEvent : GameEvent
    {
        public List<RoomInfoPacket> infoPackets;
    }
    public class GameStateChangeEvent : GameEvent
    {
        public RoomState State;
    }
    public class GameFinishEvent : GameEvent
    {
        public bool Win;
    }
}
