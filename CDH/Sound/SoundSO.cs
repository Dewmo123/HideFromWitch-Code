using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.CDH.Code.Sound
{
    public enum AudioTypes
    {
        SFX,
        BGM,
    }

    [CreateAssetMenu(fileName = "Sound Clip", menuName = "SO/SoundClip")]
    public class SoundSO : ScriptableObject
    {

        public AudioTypes audioType;
        public AudioClip clip;
        public bool loop;
        public bool randomPicth;

        [Range(0.0f, 1.0f)]
        public float randomPitchModifier = 0.1f;
        [Range(0.1f, 2.0f)]
        public float volume = 1.0f;
        [Range(0.1f, 3.0f)]
        public float pitch = 1.0f;
    }
}
