using System.Numerics;

namespace Server.Rooms
{
    internal static class GameSetting
    {
        public readonly static int modelCount = 93;
        public readonly static int minPlayerCount = 6;
        public readonly static Vector3 beforeSeekerPos = new Vector3(5077.37f, 7809, 5007.37f);//임의
        public readonly static Vector3 seekerPos = new Vector3(77.37f, 9, 7.37f);//임의
        public readonly static Vector3 hiderPos = new Vector3(-12, 16, -17.5f);//임의
    }
}
