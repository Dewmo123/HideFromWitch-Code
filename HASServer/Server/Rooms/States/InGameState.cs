using Server.Events;
using Server.Objects;
using Server.Utiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Server.Rooms.States
{
    internal class InGameState : CanMoveState
    {
        private CountTimeSync _gameCount;
        private static int _gameTime = 200;
        public InGameState(GameRoom room) : base(room)
        {
            _gameCount = new(HandleTimerElapsed, HandleTimerEnd, 100);
        }

        public override void Enter()
        {
            base.Enter();
            _gameCount.StartCount(_gameTime);
            _room.Bus.AddListener<ShootEvent>(HandleShoot);
            _room.Bus.AddListener<AttackEvent>(HandleAttack);
            _room.Bus.AddListener<PlayerDeadEvent>(HandlePlayerDead);
            _room.Bus.AddListener<UseSkillEvent>(HandleUseSkill);
            _room.ObjectManager.GetObjects<Player>()
                .Where(player => player.Role == Role.Hunter)
                .ToList().ForEach(HunterSetting);
        }
        private void HunterSetting(Player player)
        {
            player.Skill = (SkillType)Random.Shared.Next(1, 3);
            player.position = GameSetting.seekerPos;
            ClientSession session = _room.GetSession(player.index);
            S_Teleport teleport = new() { position = GameSetting.seekerPos.ToPacket() };
            session.Send(teleport.Serialize());
            S_SetSkill setSkill = new() { skill = (ushort)player.Skill };
            session.Send(setSkill.Serialize());
        }
        public override void Update()
        {
            base.Update();
            _gameCount.UpdateDeltaTime();
        }
        public override void Exit()
        {
            base.Exit();
            _room.Bus.RemoveListener<ShootEvent>(HandleShoot);
            _room.Bus.RemoveListener<AttackEvent>(HandleAttack);
            _room.Bus.RemoveListener<PlayerDeadEvent>(HandlePlayerDead);
            _room.Bus.RemoveListener<UseSkillEvent>(HandleUseSkill);
        }
        private void HandleUseSkill(UseSkillEvent @event)
        {
            Player player = @event.player;
            //횟수 판별
            switch (@event.player.Skill)
            {
                case SkillType.Runner:
                    break;
                case SkillType.Scan:
                    if (player.SkillUseCount == 3)
                        return;
                    S_UseSkill useSkill = new() { skillType = (ushort)player.Skill };
                    _room.Sessions[player.index].Send(useSkill.Serialize());
                    break;
                case SkillType.Force:
                    if (player.SkillUseCount == 2)
                        return;
                    ModelChange();
                    break;
            }
            player.SkillUseCount++;
        }
        private void HandlePlayerDead(PlayerDeadEvent @event)
        {
            Role role = @event.player.Role;
            if (!_room.players.ContainsKey(role))
                return;
            _room.players[role]--;
            Console.WriteLine($"{role}: {_room.players[role]}");
            if (_room.players[role] <= 0)
            {
                //승패 판정
                S_GameEnd gameEnd = new()
                {
                    loser = (ushort)role,
                    winner = (ushort)(role == Role.Hunter ? Role.Runner : Role.Hunter)
                };
                _room.Broadcast(gameEnd);
                _room.ChangeState(RoomState.Lobby);
            }
        }
        private void HandleAttack(AttackEvent evt)
        {
            var hitPlayer = _room.ObjectManager.GetObject<Player>(evt.hitIndex);
            var attacker = _room.ObjectManager.GetObject<Player>(evt.attacker);
            if (hitPlayer.Role != Role.Runner || attacker.Role != Role.Hunter)
                return;
            _room.PlayerDead(hitPlayer);
        }
        private void HandleShoot(ShootEvent @event)
        {
            if (_room.ObjectManager.GetObject<Player>(@event.index).Role != Role.Hunter)
                return;
            _room.Broadcast(new S_Shoot()
            {
                direction = @event.direction,
                startPos = @event.startPos,
                index = @event.index
            });
        }
        private void HandleTimerElapsed(double obj)
        {
            //Console.WriteLine((float)(_gameTime - obj));
            S_SyncTimer time = new() { time = (float)(_gameTime - obj) };
            _room.Broadcast(time);
        }
        private void ModelChange()
        {
            S_ChangeModel modelPacket = new();
            List<PlayerModelPacket> models = new();
            _room.ObjectManager.GetObjects<Player>().ForEach(player =>
            {
                if (player.Role != Role.Runner)
                    return;
                player.ModelIndex = Random.Shared.Next(0, GameSetting.modelCount);
                models.Add(new PlayerModelPacket()
                {
                    index = player.index,
                    modelIndex = player.ModelIndex
                });
            });
            modelPacket.modelInfos = models;
            _room.Broadcast(modelPacket);
        }
        private void HandleTimerEnd()
        {
            S_GameEnd gameEnd = new()
            {
                loser = (ushort)Role.Hunter,
                winner = (ushort)Role.Runner
            };
            _room.Broadcast(gameEnd);
            _room.ChangeState(RoomState.Lobby);
        }
    }
}
