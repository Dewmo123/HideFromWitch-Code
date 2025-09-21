using DewmoLib.ObjectPool.RunTime;
using UnityEngine;

namespace Assets._00.Work.CIW.Code
{
    public class BulletTrail : MonoBehaviour
    {
        [SerializeField] TrailRenderer trailEffect;

        public void PlayTrail()
        {
            trailEffect.Clear();
            trailEffect.emitting = true;
        }

        public void StopTrail()
        {
            trailEffect.emitting = false;
        }

        public void ResetItem()
        {
            trailEffect.emitting = false;
        }
    }
}

