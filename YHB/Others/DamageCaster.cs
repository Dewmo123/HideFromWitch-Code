using Assets._00.Work.YHB.Scripts.Players;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Others
{
	public abstract class DamageCaster : MonoBehaviour
	{
		[SerializeField] protected LayerMask whatIsTarget;

		public abstract void InitCaster();
		public abstract bool CastDamage();
	}
}
