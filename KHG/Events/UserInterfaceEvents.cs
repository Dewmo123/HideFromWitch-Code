using DewmoLib.Utiles;
using UnityEngine;

namespace KHG.Events
{
    public enum TitleMode
    {
        MainTitle,
        SubTitle
    }

    public struct TitleData
    {
        public float fadeInTime;
        public float lifeTime;
        public float fadeOutTime;

        public TitleMode mode;
    }

    public class UserInterfaceEvents
    {
        public static readonly WarnUiEvent WarnUiEvent = new();
        public static readonly ServerConnectEvent ServerConnectEvent = new();
        public static readonly MessageEvent MessageEvent = new();
    }

    public class WarnUiEvent : GameEvent
    {
        public string Title = "¿À·ù";
        public string Message = "Unexpected Error";
    }
    public class ServerConnectEvent : GameEvent
    {
        public bool result;
    }

    public class MessageEvent : GameEvent
    {
        public TitleData Data;
        public string Message;
    }
}