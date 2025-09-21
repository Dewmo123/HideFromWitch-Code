using DewmoLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Audio;

namespace Utility.Unity.Media.Audio
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private PoolManagerMono audioPool;
        [SerializeField] private PoolItemSO _audioPoolType;
        public static SoundManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        public Sound MakeSound(AudioSource source, AudioMixer mixer, float volume = 0f, bool threeDementionMode = false)
        {
            Sound sound = audioPool.Pop<Sound>(_audioPoolType);
            GameObject soundObj = sound.GameObject;

            soundObj.name = $"Audio_{source.name}";
            soundObj.transform.position = threeDementionMode ? source.transform.position : Vector3.zero;

            sound.InitSound(source, mixer, _audioPoolType);

            return sound;
        }

        public void RemoveSound(Sound target)
        {
            target.Remove();
        }
    }
}
