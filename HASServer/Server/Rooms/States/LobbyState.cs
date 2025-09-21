using Server.Events;
using Server.Objects;
using Server.Utiles;
using System;
using System.Numerics;

namespace Server.Rooms.States
{
    class LobbyState : CanMoveState
    {
        public LobbyState(GameRoom room) : base(room)
        {
        }
        public override void Enter()
        {
            base.Enter();
            _room.Bus.AddListener<GameStartEvent>(HandleGameStartReq);
            var players = _room.ObjectManager.GetObjects<Player>();
            S_ResetGame reset = new();
            reset.playerinits = new();
            foreach(var player in players)
            {
                player.SkillUseCount = 0;
                player.Role = Role.None;
                player.position = GameSetting.hiderPos;
                reset.playerinits.Add((PlayerInitPacket)player.CreatePacket());
            }
            _room.Broadcast(reset);
        }
        public override void Exit()
        {
            _room.Bus.RemoveListener<GameStartEvent>(HandleGameStartReq);
            base.Exit();
        }

        private void HandleGameStartReq(GameStartEvent @event)
        {
            _room.ChangeState(RoomState.Between);
        }
    }
}
