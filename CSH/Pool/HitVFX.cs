using DewmoLib.ObjectPool.RunTime;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

namespace Assets._00.Work.CSH._01_Scripts.Pools
{
    public class HitVFX : MonoBehaviour, IPoolable
    {
        [SerializeField] VisualEffect vfx;

        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;

        Pool _pool;

        public void PlayVFX()
        {
            vfx.Play();
            StartCoroutine(WaitAndRelease());
        }

        private IEnumerator WaitAndRelease()
        {
            yield return new WaitForSeconds(1.5f);

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