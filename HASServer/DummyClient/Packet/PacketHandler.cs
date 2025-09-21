using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
    internal static void S_ChangeModelHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_ChatHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_DeadHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_EnterRoomFirstHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_HostChangeHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_MoveHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_MoveLockHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_PacketResponseHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_PlayAnimationHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_ResetGameHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_RoomEnterFirstHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_RoomEnterHandler(PacketSession session, IPacket packet)
    {
        var roomEnter = (S_RoomEnter)packet;
        //Console.WriteLine($"newPlayer: {roomEnter.newPlayer.index}");
    }

    internal static void S_RoomExitHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_RoomListHandler(PacketSession session, IPacket packet)
    {
        var listPacket = (S_RoomList)packet;
        foreach(var item in listPacket.roomInfos)
        {
            Console.WriteLine($"{item.roomId}: {item.currentCount} / {item.maxCount}");
        }
    }

    internal static void S_RoomStateChangeHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_RotateHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_RoundStartHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_ScannedHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_ShootHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_SyncTimerHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_TeamInfosHandler(PacketSession session, IPacket packet)
    {
        Console.WriteLine("ASD");
    }

    internal static void S_TeleportHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_TestTextHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_UpdateInfosHandler(PacketSession session, IPacket packet)
    {

    }

    internal static void S_UpdateLocationsHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_UpdateRoomStateHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_UpdateSnapshotHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_UseSkillHandler(PacketSession session, IPacket packet)
    {
    }
}
