using Assets._00.Work.CDH.Code.Sound;
using DewmoLib.ObjectPool.RunTime;
using DewmoLib.Utiles;
using System.Collections;
using UnityEngine;

namespace Assets._00.Work.CIW.Code
{
    public class BulletBoomEffect : MonoBehaviour, IPoolable
    {
        [SerializeField] ParticleSystem effect;
        [SerializeField] SoundPlayCompo sp;
        [field : SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;

        Pool _pool;

        public void PlayParticle(Vector3 position)
        {
            transform.position = position;
            effect.Play();
            sp.PlaySound();
            StartCoroutine(WaitAndRelease());
        }

        private IEnumerator WaitAndRelease()
        {
            yield return new WaitForSeconds(effect.main.duration);

            _pool.Push(this);
        }

        public void ResetItem()
        {
        }

        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }
    }
}

