using Server.Events;
using Server.Objects;
using Server.Utiles;
using System;
using System.Linq;

namespace Server.Rooms.States
{
    class ReadyState : CanMoveState
    {
        private CountTimeSync _readyToStart;
        private int _seekerCount = 0;
        private int _time = 30;
        public ReadyState(GameRoom room) : base(room)
        {
            _readyToStart = new CountTimeSync(HandleTimerElapsed, HandleTimer, 100);
        }
        public override void Enter()
        {
            base.Enter();
            //int seekerCount = _room.SessionCount / 5;
            _room.Bus.AddListener<PlayerDeadEvent>(HandlePlayerDead);
            _readyToStart.StartCount(_time);
            SetPlayerRoles();
            _room.players[Role.Hunter] = _seekerCount;
            _room.players[Role.Runner] = _room.SessionCount - _seekerCount;
        }
        private void SetPlayerRoles()
        {
            _seekerCount = 1 + (int)MathF.Floor(_room.SessionCount / 6f) ;//test
            Console.WriteLine(_seekerCount);
            var players = _room.ObjectManager.GetObjects<Player>();
            players.ForEach(player =>
            {
                player.Role = Role.Runner;
                player.position = GameSetting.hiderPos;
                player.ModelIndex = Random.Shared.Next(0, GameSetting.modelCount);
            });
            for (int i = 0; i < _seekerCount; i++)
            {
                int randomVal = Random.Shared.Next(_room.SessionCount);
                if (players[i].Role == Role.Hunter)
                {
                    i--;
                    continue;
                }
                players[randomVal].Role = Role.Hunter;
                players[randomVal].position = GameSetting.beforeSeekerPos;
                players[randomVal].ModelIndex = -1;
            }
            S_ResetGame resetGame = new();
            resetGame.playerinits = new();
            players.ForEach(player => resetGame.playerinits.Add((PlayerInitPacket)player.CreatePacket()));
            _room.Broadcast(resetGame);
        }
        public override void Update()
        {
            base.Update();
            _readyToStart.UpdateDeltaTime();
        }
        public override void Exit()
        {
            base.Exit();
            _room.Bus.RemoveListener<PlayerDeadEvent>(HandlePlayerDead);
        }



        private void HandlePlayerDead(PlayerDeadEvent @event)
        {
            Role role = @event.player.Role;
            if (!_room.players.ContainsKey(role))
                return;
            _room.players[role]--;
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
        protected override void HandleFall(FallEvent @event)
        {
            Player player = @event.player;
            switch (player.Role)
            {
                case Role.None:
                    var nontel = new S_Teleport() { position = GameSetting.hiderPos.ToPacket() };
                    _room.GetSession(player.index).Send(nontel.Serialize());
                    break;
                case Role.Hunter:
                    var teleport = new S_Teleport() { position = GameSetting.beforeSeekerPos.ToPacket() };
                    _room.GetSession(player.index).Send(teleport.Serialize());
                    break;
                case Role.Runner:
                    _room.PlayerDead(player);
                    break;
            }
        }
        private void HandleTimerElapsed(double obj)
        {
            S_SyncTimer time = new() { time = (float)(_time - obj) };
            _room.Broadcast(time);
        }

        private void HandleTimer()
        {
            _room.ChangeState(RoomState.InGame);
        }
    }
}
