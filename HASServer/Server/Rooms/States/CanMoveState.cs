using Server.Events;
using Server.Objects;
using Server.Utiles;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Server.Rooms.States
{
    internal abstract class CanMoveState : GameRoomState
    {
        private List<PlayerMovePacket> movePackets;
        public CanMoveState(GameRoom room) : base(room)
        {
            movePackets = new();
        }
        public override void Enter()
        {
            base.Enter();
            _room.Bus.AddListener<ClientMoveEvent>(HandleMove);
            _room.Bus.AddListener<ClientRotateEvent>(HandleRotate);
            _room.Bus.AddListener<FallEvent>(HandleFall);
        }
        public override void Update()
        {
            base.Update();
            foreach(var item in _room.ObjectManager.GetObjects<Player>())
            {
                if (!item.MoveFlag)
                    continue;
                movePackets.Add(new PlayerMovePacket()
                {
                    direction = item.direction.ToPacket(),
                    index = item.index,
                    position = item.position.ToPacket(),
                    rotation = item.rotation.ToPacket(),
                    speed = item.Speed
                });
                item.MoveFlag = false;
            }
            _room.Broadcast(new S_Move() { moves = movePackets});
            movePackets.Clear();
        }
        public override void Exit()
        {
            base.Exit();
            _room.Bus.RemoveListener<FallEvent>(HandleFall);
            _room.Bus.RemoveListener<ClientMoveEvent>(HandleMove);
            _room.Bus.RemoveListener<ClientRotateEvent>(HandleRotate);
        }
        protected virtual void HandleFall(FallEvent @event)
        {
            Player player = @event.player;
            switch (player.Role)
            {
                case Role.None:
                    var nontel = new S_Teleport() { position = GameSetting.hiderPos.ToPacket() };
                    _room.GetSession(player.index).Send(nontel.Serialize());
                    break;
                case Role.Hunter:
                    var teleport = new S_Teleport() { position = GameSetting.seekerPos.ToPacket() };
                    _room.GetSession(player.index).Send(teleport.Serialize());
                    break;
                case Role.Runner:
                    _room.PlayerDead(player);
                    break;
            }
        }

        private void HandleRotate(ClientRotateEvent @event)
        {
            var player = _room.ObjectManager.GetObject<Player>(@event.index);
            player.rotation = @event.rotation;
        }

        private void HandleMove(ClientMoveEvent @event)
        {
            var player = _room.ObjectManager.GetObject<Player>(@event.index);
            player.position = @event.position;
            player.direction = @event.direction;
            player.Speed = @event.speed;
            player.MoveFlag = true;
        }
    }
}
