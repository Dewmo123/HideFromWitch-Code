using DewmoLib.ObjectPool.RunTime;
using UnityEngine;

namespace Assets._00.Work.CSH._01_Scripts.Hunter
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        public PoolItemSO PoolItem { get; set; }

        public GameObject GameObject => gameObject;

        
        private Rigidbody rb;

        private void OnTriggerEnter(Collider other)
        {
            
        }
        
        public void Fire(Vector3 direction, float speed)
        {
            rb.AddForce(direction * speed, ForceMode.Impulse);
        }

        public void ResetItem()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void SetUpPool(Pool pool)
        {
            
        }
    }
}