using Assets._00.Work.YHB.Scripts.Core;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using Assets._00.Work.YHB.Scripts.SkillSystem;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Executors
{
	public class MoveLockSkillExecutor : UseSkillExecutor
	{
		protected override void ActivateCore()
		{
			inputSO.OnMoveLockEvent += HandleMoveLockEvent;
		}

		protected override void DeActivateCore()
		{
			inputSO.OnMoveLockEvent -= HandleMoveLockEvent;
		}

		private void HandleMoveLockEvent()
		{
			tryUseSkillBehaviour.Execute(_useSkillData);
		}
	}
}
