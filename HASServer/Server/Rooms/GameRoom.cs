using Server.Events;
using Server.Objects;
using Server.Pool;
using Server.Rooms.States;
using Server.Utiles;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace Server.Rooms
{
    internal class GameRoom : Room
    {
        public bool CanAddPlayer => _stateMachine.CurrentStateEnum == RoomState.Lobby && SessionCount < MaxSessionCount;
        private RoomStateMachine _stateMachine;
        public Dictionary<Role, int> players { get; private set; }
        public bool IsGaming { get; set; } = false;
        public GameRoom(RoomManager manager, int roomId, string name) : base(manager, roomId, name)
        {
            players = new();
            players.Add(Role.Hunter, 0);
            players.Add(Role.Runner, 0);
            _stateMachine = new(this);
            ChangeState(RoomState.Lobby);
        }
        public override void UpdateRoom()
        {
            base.UpdateRoom();
            _stateMachine.UpdateRoom();
        }
        public override Player Enter(ClientSession session)
        {
            Player newP = base.Enter(session);
            if (_stateMachine.CurrentStateEnum != RoomState.Lobby)
                newP.Role = Role.Observer;
            newP.Skill = SkillType.Runner;
            newP.position = new Vector3(-12, 16, -17.5f);
            return newP;
        }
        public override Player Leave(ClientSession session)
        {
            Player removed = base.Leave(session);
            PlayerDead(removed);
            return removed;
        }

        public override void PlayerDead(Player deadPlayer)
        {
            Broadcast(new S_Dead() { index = deadPlayer.index });
            var deadEvent = PoolManager.Instance.Pop<PlayerDeadEvent>();
            deadEvent.player = deadPlayer;
            Bus.InvokeEvent(deadEvent);
            PoolManager.Instance.Push(deadEvent);
            deadPlayer.Role = Role.Dead;
        }

        public void ChangeState(RoomState newState)
        {
            S_RoomStateChange change = new() { state = (ushort)newState };
            Broadcast(change);
            _stateMachine.ChangeState(newState);
        }
    }
}
