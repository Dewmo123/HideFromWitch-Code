using Server.Objects;
using Server.Utiles;
using System.Numerics;

namespace Server.Events
{
    internal class ClientMoveEvent : GameEvent
    {
        public int index;
        public Vector3 position;
        public Vector3 direction;
        public float speed;

        public override void ResetItem()
        {
            index = -1;
            position = default;
            direction = default;
            speed = 0;
        }
    }
    internal class ClientChangeModelEvent : GameEvent
    {
        public int index;
        public int modelIndex;

        public override void ResetItem()
        {
            index = -1;
            modelIndex = -1;
        }
    }
    internal class ClientRotateEvent : GameEvent
    {
        public int index;
        public Quaternion rotation;

        public override void ResetItem()
        {
            index = -1;
            rotation = default;
        }
    }
    internal class AttackEvent : GameEvent
    {
        public int attacker;
        public int hitIndex;
        public override void ResetItem()
        {
            hitIndex = default;
            attacker = default;
        }
    }
    internal class ShootEvent : GameEvent
    {
        public int index;
        public VectorPacket direction;
        public VectorPacket startPos;
        public override void ResetItem()
        {
            direction = default;
            startPos = default;
        }
    }
    internal class GameStartEvent : GameEvent
    {
        public override void ResetItem()
        {
        }
    }
    internal class PlayerDeadEvent : GameEvent
    {
        public Player player;
        public override void ResetItem()
        {
            player = null;
        }

    }
    internal class UseSkillEvent : GameEvent
    {
        public Player player;
        public override void ResetItem()
        {
            player = null;
        }
    }
    internal class FallEvent : GameEvent
    {
        public Player player;
        public override void ResetItem()
        {
        }
    }
}
