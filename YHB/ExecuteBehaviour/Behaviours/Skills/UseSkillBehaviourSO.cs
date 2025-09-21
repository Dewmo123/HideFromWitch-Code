using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.ExecuteBehaviour.Behaviours.Skills
{
	[CreateAssetMenu(fileName = "UseSkillBehaviour", menuName = "SO/ScriptableBehaviour/Behaviour/Skill/UseSkill", order = 0)]
	public class UseSkillBehaviourSO : ScriptableBehaviourSO
	{
		protected override bool LogicExecute<T>(T data)
		{
			if (data is not UseSkillData useSkillData)
				return false;

			useSkillData.skill.UseSkill();
			return true;
		}
	}
}
