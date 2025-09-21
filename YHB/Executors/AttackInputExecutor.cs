using Assets._00.Work.YHB.Scripts.Core;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using Assets._00.Work.YHB.Scripts.SkillSystem;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Executors
{
	public class AttackInputExecutor : UseSkillExecutor
	{
		protected override void ActivateCore()
		{
			inputSO.OnAttackKStatusChangeEvent += HandleAttackKStatusChangeEvent;
		}

		protected override void DeActivateCore()
		{
			inputSO.OnAttackKStatusChangeEvent -= HandleAttackKStatusChangeEvent;
		}

		private void HandleAttackKStatusChangeEvent(bool pressed)
		{
			if (pressed)
				tryUseSkillBehaviour.Execute(_useSkillData);
		}
	}
}
