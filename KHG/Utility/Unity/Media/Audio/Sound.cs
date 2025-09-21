using DewmoLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Audio;

namespace Utility.Unity.Media.Audio
{
    public class Sound : MonoBehaviour, IPoolable
    {
        private PoolItemSO _poolItemSO;
        private Pool _audioPool;

        private AudioSource _source;
        private AudioMixer _mixer;

        private float _targetVolume;
        private float _targetPitch;
        private float _speed;

        public PoolItemSO PoolItem => _poolItemSO;
        public GameObject GameObject => gameObject;

        public void InitSound(AudioSource source, AudioMixer mixer, PoolItemSO poolItemSO)
        {
            this._source = source;
            this._mixer = mixer;
            this._poolItemSO = poolItemSO;
        }

        public void Play()
        {
            Verify();
            _source.Play();
        }

        public void Pause()
        {
            Verify();
            _source.Pause();
        }

        public void Remove()
        {
            if (_audioPool != null)
            {
                _audioPool.Push(this);
                return;
            }
            Debug.LogWarning($"{gameObject.name} : 오디오 풀링을 하고있지 않습니다.");
            Destroy(gameObject);
        }

        public void SetLoop(bool value)
        {
            Verify();
            _source.loop = value;
        }

        public void SetVolume(float target, float speed = 0f)
        {
            Verify();
            _targetVolume = target;
            _speed = speed;
        }

        public void SetPitch(float target, float speed = 0f)
        {
            Verify();
            _targetVolume = target;
            _speed = speed;
        }

        private void Update()
        {
            SetAudioSettings();
        }

        private void SetAudioSettings()
        {
            if (_source.volume != _targetVolume)
            {
                _source.volume = Mathf.Lerp(_source.volume, _targetVolume, Time.deltaTime + _speed);
            }
            if (_source.pitch != _targetPitch)
            {
                _source.pitch = Mathf.Lerp(_source.pitch, _targetPitch, Time.deltaTime + _speed);
            }
        }

        private void Verify()
        {
            if (_mixer == null || _source == null)
            {
                Debug.LogWarning($"{gameObject.name} : 소리를 출력할 AudioMixer또는 AudioSource가 존재하지 않습니다.");
            }
        }

        public void SetUpPool(Pool pool)
        {
            _audioPool = pool;
        }

        public void ResetItem()
        {
            _speed = 0f;
            _targetVolume = 0.5f;
            _targetPitch = 1f;

            _mixer = null;
            _source = null;
        }
    }
}
