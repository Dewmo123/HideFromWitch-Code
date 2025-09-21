using Assets._00.Work.CDH.Code.Sound;
using DewmoLib.Utiles;
using UnityEngine;

namespace Assets._00.Work.CDH.Code.Events
{
    public class SoundEvents
    {
        public static readonly ReqVolumeEvent ReqVolumeEvent = new();
        public static readonly ResponseVolumeEvent ResponseVolumeEvent = new();
        public static readonly SetVolumeEvent SetVolumeEvent = new();
        public static readonly PlaySFXEvent PlaySFXEvent = new();
        public static readonly PlayBGMEvent PlayBGMEvent = new();
    }

    public class ReqVolumeEvent : GameEvent
    {

    }

    public class ResponseVolumeEvent : GameEvent
    {
        public float Sfx;
        public float Bgm;
    }

    public class SetVolumeEvent : GameEvent
    {
        public float volume;
        public AudioTypes audioType;

        public SetVolumeEvent Initializer(float volume, AudioTypes audioType)
        {
            this.volume = volume;
            this.audioType = audioType;

            return this;
        }
    }

    public class PlaySFXEvent : GameEvent
    {
        public Vector3 position;
        public SoundSO sound;

        public PlaySFXEvent Initializer(Vector3 position, SoundSO sound)
        {
            this.position = position;
            this.sound = sound;
            return this;
        }
    }

    public class PlayBGMEvent : GameEvent
    {
        public SoundSO sound;

        public PlayBGMEvent Initializer(SoundSO sound)
        {
            this.sound = sound;
            return this;
        }
    }

    public class StopBGMEvent : GameEvent
    {

    }
}
