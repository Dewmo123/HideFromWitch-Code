using Assets._00.Work.YHB.Scripts.ExecuteBehaviour;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.Behaviours.Skills;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using DewmoLib.ObjectPool.RunTime;
using System;
using UnityEngine;

namespace AKH.Scripts.Players.Active
{
    public class DummyClientShootExecuter : MonoBehaviour
    {
        [SerializeField] private DummyClient dummyClient;
        [SerializeField] private ScriptableBehaviourSO shootBehaviour;
        [SerializeField] private PoolItemSO bulletItem;
        private ShootBulletData _shootData;
        private void Awake()
        {
            _shootData = new();
            _shootData.bulletItem = bulletItem;
            dummyClient.OnShootEvent += HandleShoot;
        }
        private void OnDestroy()
        {
            dummyClient.OnShootEvent -= HandleShoot;
        }

        private void HandleShoot(Vector3 vector1, Vector3 vector2)
        {
            _shootData.direction = vector2;
            _shootData.startPosition = vector1;
            shootBehaviour.Execute(_shootData);
        }
    }
}
