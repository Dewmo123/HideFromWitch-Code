using Assets._00.Work.CSH._01_Scripts.Pools;
using DewmoLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._00.Work.YHB.Scripts.Entities
{
	public class EntityHealth : EntityComponent
	{
        
        [SerializeField] private int maxHealth;
		[SerializeField] private bool activeWithResetHealth;
		

		public UnityEvent OnHit;
		public UnityEvent OnDead;

		private int _currentHealth;
		public int CurrentHealth
		{
			get => _currentHealth;
			set => _currentHealth = Mathf.Clamp(value, 0, maxHealth);
		}

		public bool IsDead { get; private set; }

		private Entity _entity;

		public override void Initialize(EntityComponentRegistry registry)
		{
			base.Initialize(registry);

			_entity = registry.ResolveComponent<Entity>();
		}

		protected override void ActivateCore()
		{
			_entity.OnDamageEvent += TakeDamage;

			if (activeWithResetHealth)
				CurrentHealth = maxHealth;
		}

		protected override void DeActivateCore()
		{
			_entity.OnDamageEvent -= TakeDamage;
		}

		public void Dead()
		{
			if (!IsActived)
				return;
			Debug.Log("se");
			if (IsDead)
				return;

			IsDead = true;
			OnDead?.Invoke();
		}

		private bool TakeDamage(DamageData damageData)
		{
			if (IsDead)
				return false;

			CurrentHealth -= damageData.damage;
			OnHit?.Invoke();

			if (CurrentHealth <= 0)
				Dead();

			return true;
		}

		
	}
}