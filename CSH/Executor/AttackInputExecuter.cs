using Assets._00.Work.CSH._01_Scripts.Component;
using Assets._00.Work.YHB.Scripts.Core;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour;
using Assets._00.Work.YHB.Scripts.Executors;
using System;
using UnityEngine;

namespace Assets._00.Work.CSH._01_Scripts._Executor
{
    public class AttackInputExecuter : ActiverMono, IEntityResolver
    {
        [Header("Value")]
        [SerializeField] private InputSO inputSO;
        [SerializeField] private ScriptableBehaviourSO attackInputBehaviour;

        private AttackCompo _attackCompo;

        public void Initialize(EntityComponentRegistry registry)
        {
            _attackCompo = registry.ResolveComponent<AttackCompo>();
        }

        protected override void ActivateCore()
        {
            inputSO.OnAttackKStatusChangeEvent += HandleAttackStatusChangeEvent;
        }

        protected override void DeActivateCore()
		{
            inputSO.OnAttackKStatusChangeEvent -= HandleAttackStatusChangeEvent;
        }

        public void Release(EntityComponentRegistry registry)
        {
        }

        private void HandleAttackStatusChangeEvent(bool status)
        {
            if(!status)
                return;

            attackInputBehaviour.Execute<AttackCompo>(_attackCompo);

        }

    }
}
