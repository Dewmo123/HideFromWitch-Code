using DewmoLib.Utiles;

namespace DewmoLib.Dependencies
{
    public static class DependencyEvents
    {
        public static DependencyEvent DependencyEvent = new();
    }
    public class DependencyEvent : GameEvent
    {
        public INeedInject needInject;
    }
}
