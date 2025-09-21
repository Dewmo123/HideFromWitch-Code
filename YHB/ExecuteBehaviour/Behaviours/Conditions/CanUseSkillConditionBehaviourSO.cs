using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.ExecuteBehaviour.Behaviours.Conditions
{
	[CreateAssetMenu(fileName = "Condition_CanUseSkillBehaviourSO", menuName = "SO/ScriptableBehaviour/Behaviour/Conditions/CanUseSkill", order = 0)]
	public class CanUseSkillConditionBehaviourSO : ScriptableBehaviourSO
	{
		protected override bool LogicExecute<T>(T data)
		{
			if (data is not UseSkillData useSkillData)
				return false;

			return useSkillData.skill.CanUseSkill();
		}
	}
}
