using AgamaLibrary.DataStructures;
using Alchemy.Inspector;
using Assets._00.Work.YHB.Scripts.Core;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Executors
{
	public class MoveInputExecutor : ActiverMono
	{
		[Header("Cam")]
		[LabelText("Camera Parent")]
		[SerializeField] protected Transform cameraRotationOwner;
		[SerializeField] protected bool useOnlyRotationY = true;

		[Header("Setting")]
		[SerializeField] protected InputSO inputSO;

		// 코드가 더러워 지는데, Executor 자체에서 Entity에 의존하는 걸 제거하기 위함.
		[FoldoutGroup("Move Setting")]
		[SerializeField] protected GameObject movementObject;
		[FoldoutGroup("Move Setting")]
		[SerializeField] protected ScriptableBehaviourSO moveInputBehaviour;
		[FoldoutGroup("Move Setting")]
        [SerializeField] protected float moveSpeed;

		[FoldoutGroup("Jump Setting")]
		[SerializeField] protected bool useJump = true;
		[FoldoutGroup("Jump Setting")]
		[SerializeField] protected ScriptableBehaviourSO jumpInputBehaviour;

		[SerializeField] protected IMover movement;

        protected NotifyValue<bool> isMoving;
		protected MovementData _moveData;

		public override void EarlyInitialize()
		{
			base.EarlyInitialize();

			movementObject.TryGetComponent<IMover>(out IMover mover);
			movement = mover;

			_moveData = new MovementData();
			_moveData.movement = movement;

			isMoving = new NotifyValue<bool>(false);
		}

		protected override void ActivateCore()
		{
			isMoving.OnValueChanged += HandleIsMovingChange;
			movement.MoveSpeed = moveSpeed;
			if (useJump)
				inputSO.OnJumpStatusChangeEvent += HandleJumpStatusChangeEvent;
		}

		protected override void DeActivateCore()
		{
			isMoving.OnValueChanged -= HandleIsMovingChange;

			if (useJump)
				inputSO.OnJumpStatusChangeEvent -= HandleJumpStatusChangeEvent;
		}

		protected void HandleJumpStatusChangeEvent(bool status)
		{
			if (!status)
				return;

			jumpInputBehaviour.Execute<MovementData>(_moveData);
		}

		protected virtual void Update()
		{
			if (!IsActived)
				return;

			isMoving.Value = inputSO.MovementDirection != Vector2.zero;
			if (isMoving.Value)
				ExecuteMoveBehaviour();
		}

		protected virtual void HandleIsMovingChange(bool previousValue, bool nextValue)
		{
			if (!nextValue)
			{
				ExecuteMoveBehaviour();
			}
		}

		protected void ExecuteMoveBehaviour()
		{
			Quaternion rotation;

			if (useOnlyRotationY)
				rotation = Quaternion.Euler(0, cameraRotationOwner.transform.rotation.eulerAngles.y, 0);
			else
				rotation = cameraRotationOwner.transform.rotation;

			_moveData.moveRotation = rotation;
			_moveData.moveDirection = inputSO.MovementDirection;
			moveInputBehaviour.Execute<MovementData>(_moveData);
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (!useJump)
				jumpInputBehaviour = null;

			if (movementObject == null)
				return;

			if (!movementObject.TryGetComponent<IMover>(out IMover mover))
			{
				movementObject = null;
				Debug.LogError("Mover can't be null");
				return;
			}
			movement = mover;
		}
#endif
	}
}
