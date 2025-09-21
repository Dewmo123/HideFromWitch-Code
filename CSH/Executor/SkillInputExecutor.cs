using Assets._00.Work.YHB.Scripts.Core;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using Assets._00.Work.YHB.Scripts.Executors;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.SkillSystem;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using AKH.Network;

namespace Assets._00.Work.CSH._01_Scripts.Executor
{
    public enum SkillType
    {
        Runner,
        Scan,
        Force
    }
    public class SkillInputExecutor : ActiverMono
    {
        [Header("Setting Value")]
        [SerializeField] private InputSO inputSO;

        [Header("Attack Value")]
        [SerializeField] private ScriptableBehaviourSO tryUseSkillBehaviour;
        [SerializeField] private SerializedDictionary<SkillType, Skill> skills;
        public SkillType CurrentSkill { get; private set; }
        public override void EarlyInitialize()
        {
            base.EarlyInitialize();
        }

        protected override void ActivateCore()
        {
            inputSO.OnScanEvent += HandleScanEvent;
        }

        protected override void DeActivateCore()
        {
            inputSO.OnScanEvent -= HandleScanEvent;
        }

        private void HandleScanEvent()
        {
            NetworkManager.Instance.SendPacket(new C_UseSkill());
        }
        public void UseSkill(SkillType type)
        {
            skills[type].UseSkill();
        }
    }
}
