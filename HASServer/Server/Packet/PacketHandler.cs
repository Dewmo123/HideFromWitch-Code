using Server;
using Server.Events;
using Server.Objects;
using Server.Pool;
using Server.Rooms;
using Server.Utiles;
using ServerCore;
using System;

class PacketHandler
{
    private static RoomManager _roomManager = RoomManager.Instance;
    internal static void C_CreateRoomHandler(PacketSession session, IPacket packet)
    {
        var createRoom = (C_CreateRoom)packet;
        var clientSession = session as ClientSession;
        Console.WriteLine(createRoom.roomName);
        int roomId = _roomManager.GenerateRoom(createRoom);
        EnterRoomProcess(roomId, clientSession);
    }

    internal static void C_RoomEnterHandler(PacketSession session, IPacket packet)
    {
        var enterPacket = (C_RoomEnter)packet;
        var clientSession = session as ClientSession;
        if (clientSession.Room != null || string.IsNullOrEmpty(clientSession.Name))
        {
            Console.WriteLine($"Enter Error");
            return;
        }
        EnterRoomProcess(enterPacket.roomId, clientSession);
    }

    private static void EnterRoomProcess(int roomId, ClientSession clientSession)
    {
        var room = _roomManager.GetRoomById(roomId) as GameRoom;
        if (room == default)
        {
            Console.WriteLine($"Wrong RoomId: {roomId}");
            return;
        }
        //if (room == default)//test
        //{
        //    int n= _roomManager.GenerateRoom(new C_CreateRoom() { maxCount = 6, roomName = "ASD" }, 1);
        //    room = _roomManager.GetRoomById(n) as GameRoom;
        //}
        Console.WriteLine("EnterRoom");
        room.Push(() =>
        {
            if (room.CanAddPlayer)
            {
                Player newP = room.Enter(clientSession);
                SendPacketResponse(clientSession, PacketID.C_RoomEnter, true);
            }
            else
            {
                SendPacketResponse(clientSession, PacketID.C_RoomEnter, false);
            }
        });
    }

    private static void SendPacketResponse(ClientSession session, PacketID caller, bool v)
    {
        S_PacketResponse response = new S_PacketResponse();
        response.responsePacket = (ushort)caller;
        response.success = v;
        session.Send(response.Serialize());
    }

    internal static void C_RoomExitHandler(PacketSession session, IPacket packet)
    {
        var clientSession = session as ClientSession;
        var room = clientSession.Room;
        if (room == null)
            return;
        room.Push(() => PoolManager.Instance.Push(room.Leave(clientSession)));
        Console.WriteLine($"Leave Room: {clientSession.SessionId}");
    }

    internal static void C_RoomListHandler(PacketSession session, IPacket packet)
    {
        var clientSession = session as ClientSession;
        var list = _roomManager.GetRoomInfos();
        Console.WriteLine(list.Count);
        S_RoomList roomList = new S_RoomList();
        roomList.roomInfos = list;
        clientSession.Send(roomList.Serialize());
    }

    internal static void C_SetNameHandler(PacketSession session, IPacket packet)
    {
        var clientSession = session as ClientSession;
        var setName = (C_SetName)packet;
        bool success = !string.IsNullOrEmpty(setName.name) || (setName.name.Length < 6 && setName.name.Length > 2);
        if (success)
        {
            clientSession.Name = setName.name;
            Console.WriteLine($"Set Name Success: {clientSession.Name}");
        }
        else
        {
            Console.WriteLine($"Set Name Error: {clientSession.Name}");
        }
        SendPacketResponse(clientSession, PacketID.C_SetName, success);
    }

    internal static void C_ChatHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        var cchat = (C_Chat)packet;
        if (string.IsNullOrEmpty(clientSession.Name) || clientSession.Room == null)
            return;
        S_Chat chat = new();
        chat.text = cchat.text;
        chat.pName = clientSession.Name;
        //chat.pName = "ASD";
        Console.WriteLine(chat.text);
        clientSession.Room.Broadcast(chat);
    }

    internal static void C_GameStartHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        var room = clientSession.Room;
        if (room == null)
            return;
        if (room.SessionCount < GameSetting.minPlayerCount)
            SendPacketResponse(clientSession, PacketID.C_GameStart, false);
        else if(room.HostIndex == clientSession.PlayerId)
        {
            var evt = PoolManager.Instance.Pop<GameStartEvent>();
            room.InvokeEvent(evt);
        }
    }

    internal static void C_MoveHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        if (clientSession.Room == null)
            return;
        var move = (C_Move)packet;
        var room = clientSession.Room;
        var evt = PoolManager.Instance.Pop<ClientMoveEvent>();
        evt.index = clientSession.PlayerId;
        evt.direction = move.direction.ToVector3();
        evt.position = move.position.ToVector3();
        evt.speed = move.speed;
        clientSession.Room.InvokeEvent(evt);
    }
    internal static void C_RotateHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        if (clientSession.Room == null)
            return;
        var rotate = (C_Rotate)packet;
        var room = clientSession.Room;
        var evt = PoolManager.Instance.Pop<ClientRotateEvent>();
        evt.index = clientSession.PlayerId;
        evt.rotation = rotate.rotation.ToQuaternion();
        clientSession.Room.InvokeEvent(evt);
    }

    internal static void C_ShootHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        if (clientSession.Room == null)
            return;
        C_Shoot shoot = (C_Shoot)packet;
        S_PlayAnimation animPacket = new()
        {
            index = clientSession.PlayerId,
            animType = (ushort)AnimType.Attack
        };
        clientSession.Room.Push(() => clientSession.Room.Broadcast(animPacket));
        var evt = PoolManager.Instance.Pop<ShootEvent>();
        evt.direction = shoot.direction;
        evt.startPos = shoot.startPos;
        evt.index = clientSession.PlayerId;
        clientSession.Room.InvokeEvent(evt);
    }

    internal static void C_AttackHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_Attack attack = (C_Attack)packet;
        if (clientSession.Room == null)
            return;
        var attackEvent = PoolManager.Instance.Pop<AttackEvent>();
        attackEvent.hitIndex = attack.hitIndex;
        attackEvent.attacker = clientSession.PlayerId;
        clientSession.Room.InvokeEvent(attackEvent);
    }

    internal static void C_ResourceLoadCompleteHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        if (clientSession.Room == null)
            return;
        Room room = clientSession.Room;
        room.Push(() =>
        {
            room.FirstEnter(clientSession);
            room.Broadcast(new S_RoomEnter()
            {
                newPlayer = (PlayerInitPacket)room.ObjectManager
                .GetObject<Player>(clientSession.PlayerId)
                .CreatePacket()
            });
            if(room.HostIndex == clientSession.PlayerId)
            {
                session.Send(new S_HostChange().Serialize());
            }
        });
    }

    internal static void C_UseSkillHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        if (clientSession.Room == null)
            return;
        var skillEvt = PoolManager.Instance.Pop<UseSkillEvent>();
        skillEvt.player = clientSession.Room.ObjectManager.GetObject<Player>(clientSession.PlayerId);
        clientSession.Room.InvokeEvent(skillEvt);
    }

    internal static void C_ScannedHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        if (clientSession.Room == null)
            return;
        C_Scanned scanned = (C_Scanned)packet;
        Console.WriteLine(scanned.scanned.Count);
        foreach(var item in scanned.scanned)
            clientSession.Room.GetSession(item.index).Send(new S_Scanned().Serialize());
    }

    internal static void C_MoveLockHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        if (clientSession.Room == null)
            return;
        C_MoveLock lockPacket = (C_MoveLock)packet;
        S_MoveLock br = new() { index = clientSession.PlayerId, value = lockPacket.value };
        clientSession.Room.Push(() => clientSession.Room.Broadcast(br));
    }

    internal static void C_FallHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        Console.WriteLine("Fall");
        if (clientSession.Room == null)
            return;
        var evt = PoolManager.Instance.Pop<FallEvent>();
        Room room = clientSession.Room;
        evt.player = room.ObjectManager.GetObject<Player>(clientSession.PlayerId);
        room.InvokeEvent(evt);
    }
}