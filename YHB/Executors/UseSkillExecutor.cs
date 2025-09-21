using Assets._00.Work.YHB.Scripts.Core;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour;
using Assets._00.Work.YHB.Scripts.SkillSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Executors
{
	public abstract class UseSkillExecutor : ActiverMono
	{
		[Header("Setting Value")]
		[SerializeField] protected InputSO inputSO;

		[Header("Skill Value")]
		[SerializeField] protected ScriptableBehaviourSO tryUseSkillBehaviour;
		[SerializeField] protected Skill skill;

		protected UseSkillData _useSkillData;

		public override void EarlyInitialize()
		{
			base.EarlyInitialize();

			_useSkillData = new UseSkillData();
			_useSkillData.skill = skill;
		}
	}
}
