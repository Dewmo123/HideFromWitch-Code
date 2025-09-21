using DewmoLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets._00.Work.CDH.Code.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour, IPoolable
    {
        [SerializeField] private AudioMixerGroup bgmGroup;
        [SerializeField] private AudioMixerGroup sfxGroup;
        [SerializeField] private AudioSource source;

        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;

        private Pool myPool;

        public void ResetItem()
        {
        }

        public void SetUpPool(Pool pool)
        {
            myPool = pool;
        }

        public async void PlaySound(SoundSO sound)
        {
            switch (sound.audioType)
            {
                case AudioTypes.SFX:
                    source.outputAudioMixerGroup = sfxGroup;
                    break;
                case AudioTypes.BGM:
                    source.outputAudioMixerGroup = bgmGroup;
                    break;
            }

            source.volume = sound.volume;
            source.pitch = sound.pitch;

            if (sound.randomPicth)
            {
                source.pitch = UnityEngine.Random.Range(-sound.randomPitchModifier, sound.randomPitchModifier);
            }

            source.loop = sound.loop;
            source.clip = sound.clip;

            source.Play();
            if (!sound.loop)
            {
                try
                {
                    await Awaitable.WaitForSecondsAsync(sound.clip.length + 0.2f,
                        destroyCancellationToken);
                    myPool.Push(this);
                }
                catch { }
            }
        }

        public void StopSound()
        {
            source.Stop();
            myPool.Push(this);
        }
    }
}
