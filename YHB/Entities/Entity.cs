using AgamaLibrary.Unity.Methods;
using DewmoLib.ObjectPool.RunTime;
using System;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Entities
{
	[RequireComponent(typeof(EntityInjector))]
	public abstract class Entity : MonoBehaviour, IDamageable
	{
		public EntityComponentRegistry Registry { get; private set; }
		public event Predicate<DamageData> OnDamageEvent;

		protected virtual void Awake()
		{
			Registry = transform.ForceGetComponent<EntityComponentRegistry>();
		}

		public bool TakeDamage(DamageData damageData)
		{
			if (OnDamageEvent == null)
				return false;

			return OnDamageEvent(damageData);
		}
	}
}
