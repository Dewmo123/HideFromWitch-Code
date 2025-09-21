using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace Assets._00.Work.CIW.Code
{
    public class BulletVFX : MonoBehaviour, IPoolable
    {
        [SerializeField] VisualEffect vfx;

        [field : SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;

        Pool _pool;

        public void PlayVFX()
        {
            vfx.Play();
            StartCoroutine(WaitAndRelease());
        }

        private IEnumerator WaitAndRelease()
        {
            while (vfx.aliveParticleCount > 0)
                yield return null;

            _pool.Push(this); // 파티클 끝나면 풀에 반환
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

