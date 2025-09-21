using Assets._00.Work.YHB.Scripts.Entities;
using System;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Players
{
	public class PlayerAnimator : EntityComponent
	{
		[SerializeField] private string defaultAnimation;
		private int _defaultAnimationHash;

		public event Action OnAnimationChangeEvent;

		private EntityAnimator _entityAnimator;

		private int _beforeAnimation;
		private int _currentAnimation;

		private bool _animationLock;

		public override void EarlyInitialize()
		{
			base.EarlyInitialize();

			_defaultAnimationHash = Animator.StringToHash(defaultAnimation);
		}

		public override void Initialize(EntityComponentRegistry registry)
		{
			base.Initialize(registry);

			_entityAnimator = registry.ResolveComponent<EntityAnimator>();
		}

		protected override void ActivateCore()
		{
			Debug.Log("Actrive");
			SetAnimationLock(false);
			ChangeAnimation(_defaultAnimationHash, false, true);
		}

		public override void Release(EntityComponentRegistry registry)
		{
			base.Release(registry);
		}

		protected override void DeActivateCore()
		{
			Debug.Log("DeActrive");
			SetAnimationLock(true);
		}

		public void ChangeAnimation(int newAnimHash, bool saveCurrentAnim = true, bool force = false)
		{
			if (_currentAnimation == newAnimHash)
				return;
			else if (_animationLock && !force)
			{
				_beforeAnimation = newAnimHash;
				return;
			}

			_entityAnimator.SetParam(_currentAnimation, false);

			if (saveCurrentAnim)
				_beforeAnimation = _currentAnimation;

			_entityAnimator.SetParam(_currentAnimation = newAnimHash, true);

			OnAnimationChangeEvent?.Invoke();
		}

		public void TurnBeforeAnim(bool saveCurrentAnim, bool force = false)
		{
			ChangeAnimation(_beforeAnimation, saveCurrentAnim, force);
		}

		public void SetAnimationLock(bool value)
			=> _animationLock = value;
    }
}
