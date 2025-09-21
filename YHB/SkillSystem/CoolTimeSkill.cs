using DewmoLib.Utiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.SkillSystem
{
	public abstract class CoolTimeSkill : Skill
	{
		[SerializeField] protected CoolTimeSkillInfoSO skillInfo;

		protected override void ActivateCore() { }

		protected override void DeActivateCore() { }

		private void Update() 
		{
			skillInfo.UpdateCoolTime();
		}

		public override void UseSkill()
		{
            skillInfo.SetCoolTime();
		}

		public override bool CanUseSkill()
		{
			bool canUse = skillInfo.CanUseSkill();
			return canUse;
		}
	}
}
