using Assets._00.Work.YHB.Scripts.Executors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.SkillSystem
{
	public abstract class Skill : ActiverMono
	{
		// 스킬 사용가능한지만 확인
		public abstract bool CanUseSkill();
		public abstract void UseSkill();
	}
}
