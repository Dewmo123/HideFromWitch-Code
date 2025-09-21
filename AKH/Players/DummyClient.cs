using UnityEngine;
using Assets._00.Work.YHB.Scripts.Entities;
using System.Diagnostics.Tracing;
using DewmoLib.Utiles;
using System;
using DewmoLib.ObjectPool.RunTime;
using Assets._00.Work.YHB.Scripts.Players;
using DewmoLib.Dependencies;

namespace AKH.Scripts.Players.Active
{
    public class DummyClient : Player, IPoolable
    {
        [SerializeField] private EventChannelSO packetChannel;

        private Pool myPool;

        public Action<Vector3> OnMoveEvent;
        public Action<Vector3,Vector3> OnShootEvent;
        public Action<Quaternion> OnRotationEvent;

        [field : SerializeField] public PoolItemSO PoolItem { get; private set; }
        [Inject] private PoolManagerMono _poolManager;
        public GameObject GameObject => gameObject;
        public void SetUpPool(Pool pool)
        {
            myPool = pool;
        }

        public void ResetItem()
        {
        }

        public void HandleDummyClientMove(Vector3 move)
        {
            OnMoveEvent?.Invoke(move);
        }

        public void HandleDummyClientRotation(Quaternion rotation)
        {
            OnRotationEvent?.Invoke(rotation);
        }
        public void HandleDummyClientShoot(Vector3 startPos, Vector3 direction)
        {
            OnShootEvent?.Invoke(startPos, direction);
        }
    }
}
