using DewmoLib.ObjectPool.RunTime;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.CSH._01_Scripts.Pools
{
    public class ScanDecal : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public PoolItemSO PoolItem { get; set; }

        public GameObject GameObject => gameObject;

        Pool _pool;

        public void PushDecal()
        {

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