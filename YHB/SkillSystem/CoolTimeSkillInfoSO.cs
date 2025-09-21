using DewmoLib.Utiles;
using System;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.SkillSystem
{
	public class CoolTimeSkillEvent : GameEvent
	{
		public float maxCoolTime; // 최대 쿨타임
		public float currentCoolTime; // 현재 남은 쿨타임
	}

	[CreateAssetMenu(fileName = "CoolTimeSkillInfo", menuName = "SO/SkillInfo/CoolTime", order = 0)]
	public class CoolTimeSkillInfoSO : SkillInfoSO
	{
		[SerializeField] private float maxCoolTime;

		private CoolTimeSkillEvent _data;

		private void OnEnable()
		{
			_data = CreateInstanceOfNewClass<CoolTimeSkillEvent>();
			_data.maxCoolTime = maxCoolTime;
		}

		public void UpdateCoolTime()
		{
			if (_data.currentCoolTime > 0)
			{
				_data.currentCoolTime = Mathf.Max(0, _data.currentCoolTime - Time.deltaTime);
				SkillEventChannel.InvokeEvent(_data);
			}
		}

		public bool CanUseSkill()
		{
			return Mathf.Approximately(_data.currentCoolTime, 0);
		}

		// 쿨타임을 0초로 만듭니다.
		public void ResetCoolTime()
		{
			_data.currentCoolTime = 0;
			SkillEventChannel.InvokeEvent(_data);
		}

		// 쿨타임을 적용시킵니다.
		public void SetCoolTime()
		{
			_data.currentCoolTime = _data.maxCoolTime;
			SkillEventChannel.InvokeEvent(_data);
		}

		protected override Type GetEventType()
			=> _data.GetType();
	}
}
