using Server.Utiles;
using System;

namespace Server.Rooms.States
{
    internal class BetweenState : CanMoveState
    {
        private CountTimeSync _betweenTimer;
        public BetweenState(GameRoom room) : base(room)
        {
            _betweenTimer = new(HandleTimerElapsed, HandleTimerEnd, 100);
        }
        public override void Enter()
        {
            base.Enter();
            _betweenTimer.StartCount(3);
        }
        public override void Update()
        {
            base.Update();
            _betweenTimer.UpdateDeltaTime();
        }
        private void HandleTimerEnd()
        {
            _room.ChangeState(RoomState.Ready);
        }

        private void HandleTimerElapsed(double obj)
        {
            S_SyncTimer timer = new() { time = (float)(3.0 - obj) };
            _room.Broadcast(timer);
        }
    }
}
