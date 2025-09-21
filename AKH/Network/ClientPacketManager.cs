using DewmoLib.Network.Packets;
using DewmoLib.Utiles;
using UnityEngine;

namespace AKH.Network
{
    public class ClientPacketManager : PacketManager
    {
        private PacketHandler _packetHandler;
        public ClientPacketManager(EventChannelSO packetChannel)
        {
            _packetHandler = new(packetChannel);
            Register();
        }

        public override void Register()
        {
            RegisterHandler<S_Chat>((ushort)PacketID.S_Chat, _packetHandler.S_ChatHandler);
            RegisterHandler<S_RoomList>((ushort)PacketID.S_RoomList, _packetHandler.S_RoomListHandler);
            RegisterHandler<S_RoomEnter>((ushort)PacketID.S_RoomEnter, _packetHandler.S_RoomEnterHandler);
            RegisterHandler<S_RoomExit>((ushort)PacketID.S_RoomExit, _packetHandler.S_RoomExitHandler);
            RegisterHandler<S_PacketResponse>((ushort)PacketID.S_PacketResponse, _packetHandler.S_PacketResponseHandler);
            RegisterHandler<S_Move>((ushort)PacketID.S_Move, _packetHandler.S_MoveHandler);
            RegisterHandler<S_SyncTimer>((ushort)PacketID.S_SyncTimer, _packetHandler.S_SyncTimerHandler);
            RegisterHandler<S_RoomEnterFirst>((ushort)PacketID.S_RoomEnterFirst, _packetHandler.S_RoomEnterFirstHandler);
            RegisterHandler<S_ResetGame>((ushort)PacketID.S_ResetGame, _packetHandler.S_ResetGameHandler);
            RegisterHandler<S_PlayAnimation>((ushort)PacketID.S_PlayAnimation, _packetHandler.S_PlayAnimationHandler);
            RegisterHandler<S_Shoot>((ushort)PacketID.S_Shoot, _packetHandler.S_ShootHandler);
            RegisterHandler<S_Dead>((ushort)PacketID.S_Dead, _packetHandler.S_DeadHandler);
            RegisterHandler<S_ChangeModel>((ushort)PacketID.S_ChangeModel, _packetHandler.S_ChangeModelHandler);
            RegisterHandler<S_Scanned>((ushort)PacketID.S_Scanned, _packetHandler.S_ScannedHandler);
            RegisterHandler<S_UseSkill>((ushort)PacketID.S_UseSkill, _packetHandler.S_UseSkillHandler);
            RegisterHandler<S_Teleport>((ushort)PacketID.S_Teleport, _packetHandler.S_TeleportHandler);
            RegisterHandler<S_MoveLock>((ushort)PacketID.S_MoveLock, _packetHandler.S_MoveLockHandler);
            RegisterHandler<S_HostChange>((ushort)PacketID.S_HostChange, _packetHandler.S_HostChangeHandler);
            RegisterHandler<S_RoomStateChange>((ushort)PacketID.S_RoomStateChange, _packetHandler.S_RoomStateChangeHandler);
            RegisterHandler<S_GameEnd>((ushort)PacketID.S_GameEnd, _packetHandler.S_GameFinishHandler);
            RegisterHandler<S_SetSkill>((ushort)PacketID.S_SetSkill, _packetHandler.S_SetSkillHandler);
        }
    }
}
