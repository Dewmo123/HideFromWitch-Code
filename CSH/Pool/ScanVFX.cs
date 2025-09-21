using DewmoLib.ObjectPool.RunTime;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

namespace Assets._00.Work.CSH._01_Scripts.Pools
{
    public class ScanVFX : MonoBehaviour, IPoolable
    {
        [SerializeField] VisualEffect vfx;

        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;

        Pool _pool;

        public void PlayVFX()
        {
            vfx.Play();
            //StartCoroutine(WaitAndRelease());
        }

        private IEnumerator WaitAndRelease()
        {
            while (vfx.aliveParticleCount > 0)
                yield return null;
            Debug.Log("vfx ����");
            _pool.Push(this); // ��ƼŬ ������ Ǯ�� ��ȯ
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