namespace Server.Utiles
{
    public enum ObjectType
    {
        None,
        Player
    }
    public enum Role
    {
        None = 1,
        Hunter = 2,
        Runner = 4,
        Observer = 8,
        Dead = 16
    }
    public enum RoomState
    {
        Lobby,
        Ready,
        InGame,
        Between
    }
    public enum AnimType
    {
        None,
        Attack
    }
    public enum SkillType
    {
        Runner,
        Scan,
        Force
    }
}
