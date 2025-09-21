using DewmoLib.ObjectPool.RunTime;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Entities
{
	public struct DamageData
	{
		// 타격 횟수로 대미지 계산합니다.
		public int damage;
		public Transform bulletTrm;
	}

	public interface IDamageable
	{
		/// <summary>
		/// 데미지를 받습니다.
		/// </summary>
		/// <param name="damageData">데미지의 정보입니다.</param>
		/// <returns>데미지를 입었는지 여부입니다.</returns>
		bool TakeDamage(DamageData damageData);
	}
}