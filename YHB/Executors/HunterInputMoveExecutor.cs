using Alchemy.Inspector;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.Players;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Executors
{
	public class HunterInputMoveExecutor : MoveInputExecutor, IEntityResolver
	{
		[FoldoutGroup("Animation Setting")]
		[SerializeField] private string idleAnimParam = "idle";
		[FoldoutGroup("Animation Setting")]
		[SerializeField] private string moveAnimParam = "move";
		private int _idleAnimHash;
		private int _moveAnimHash;
		private PlayerAnimator _playerAnimator;

		public override void EarlyInitialize()
		{
			base.EarlyInitialize();

			_idleAnimHash = Animator.StringToHash(idleAnimParam);
			_moveAnimHash = Animator.StringToHash(moveAnimParam);
		}

		public void Initialize(EntityComponentRegistry registry)
		{
			_playerAnimator = registry.ResolveComponent<PlayerAnimator>();
		}

		public void Release(EntityComponentRegistry registry) { }

		protected override void HandleIsMovingChange(bool previousValue, bool nextValue)
		{
			if (nextValue)
			{
				_playerAnimator.ChangeAnimation(_moveAnimHash);
			}
			else
			{
				_playerAnimator.ChangeAnimation(_idleAnimHash);
				ExecuteMoveBehaviour();
			}
		}
	}
}
