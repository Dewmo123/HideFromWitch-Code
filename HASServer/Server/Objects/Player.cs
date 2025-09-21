using Server.Rooms;
using Server.Utiles;
using ServerCore;
using System;
using System.Numerics;

namespace Server.Objects
{
    internal class Player : ObjectBase
    {
        public override void InitObj(ObjectManager manager)
        {
            base.InitObj(manager);
        }
        public string Name;

        public int ModelIndex { get; set; }
        public float Speed { get; set; }
        public Role Role { get; set; }
        public SkillType Skill { get; set; }
        public Vector3 direction;
        public int SkillUseCount { get; set; }
        public bool MoveFlag { get; set; }
        public override IDataPacket CreatePacket()
        {
            PlayerInitPacket packet = new()
            {
                index = index,
                modelIndex = ModelIndex,
                name = Name,
                position = position.ToPacket(),
                rotation = rotation.ToPacket(),
                role = (ushort)Role
            };
            return packet;
        }
        public override void ResetItem()
        {
            base.ResetItem();
            Role = Role.None;
            SkillUseCount = 0;
        }
    }
}
