using Alchemy.Inspector;
using Assets._00.Work.CDH.Code.Events;
using Assets._00.Work.CDH.Code.Sound;
using Assets._00.Work.YHB.Scripts.Entities.GroundCheckers;
using Assets._00.Work.YHB.Scripts.Events;
using Assets._00.Work.YHB.Scripts.Others;
using DewmoLib.Utiles;
using System;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Entities
{
	[Serializable]
	public class EntityMovement : EntityComponent, IMover, IJumpable
	{
		[Header("Setting")]
		[SerializeField] private EventChannelSO gameEventChannel;

		[FoldoutGroup("Move Setting")]
		[SerializeField] private float minMoveSpeed = 0;
		[FoldoutGroup("Move Setting")]
		[SerializeField] private float maxMoveSpeed = 100;
		[FoldoutGroup("Move Setting")]
		[SerializeField] private float moveSpeed = 5; // 스탯 기반일 필요가 없을 듯

		[FoldoutGroup("Ground Setting")]
		[SerializeField] private BoxGroundChecker boxGroundChecker;
		[FoldoutGroup("Ground Setting")]
		[SerializeField] private float gravityScale = -9.8f;
		[FoldoutGroup("Ground Setting")]
		[SerializeField] private bool useGravity = true;

		[FoldoutGroup("Jump Setting")]
		[SerializeField] private float jumpPower = 5;
		[FoldoutGroup("Jump Setting")]
		[SerializeField] private int maxJumpCount = 2;

		[FoldoutGroup("Sound Setting")]
		[SerializeField] private SoundPlayCompo jumpSoundPlayer;
		[FoldoutGroup("Sound Setting")]
		[SerializeField] private SoundPlayCompo fallSoundPlayer;

        private Rigidbody _rigidComp;

		private bool _isKinematic = false;

		public NotifyValue<float> FallTime { get; private set; }

		public float MoveSpeed
		{
			get => moveSpeed;
			set
			{
				float previous = moveSpeed;

				moveSpeed = Mathf.Clamp(value, minMoveSpeed, maxMoveSpeed);

				if (!Mathf.Approximately(previous, moveSpeed)) // 값차이 발생시
					gameEventChannel.InvokeEvent(GameObjectChangeEvents.MoveSpeedChangeEvent.Initialize(this, previous, moveSpeed));
			}
		}

		private bool _canMovement = true;
		public bool CanMovement
		{
			get => _canMovement;
			set
			{
				_canMovement = value;
				if (!_canMovement)
					StopImmediately();
			}
		}
		public bool CanRotation { get; set; } = true;

		private NotifyValue<bool> _isGround;
		public NotifyValue<bool> IsGround
		{
			get
			{
				_isGround.Value = boxGroundChecker.CheckGround();
				return _isGround;
			}
		}
		private Vector3 _velocity;
		public Vector3 Velocity => _velocity;

		private Vector3 _movementDirection;
		private Quaternion _lookTargetRotation;

		private int _currentJumpCount;
		private float _verticalVelocity;

		public override void EarlyInitialize()
		{
			base.EarlyInitialize();

			FallTime = new NotifyValue<float>(default, false, new FloatEqualityCompare());
			_isGround = new NotifyValue<bool>();
		}

		public override void Initialize(EntityComponentRegistry registry)
		{
			base.Initialize(registry);

			_rigidComp = registry.ResolveComponent<Rigidbody>();
			Debug.Assert(_rigidComp != null, $"{typeof(Rigidbody)} can not be found.");
			_rigidComp.useGravity = false;
			gameEventChannel.InvokeEvent(GameObjectChangeEvents.MoveSpeedChangeEvent.Initialize(this, 0, moveSpeed));
		}

		protected override void ActivateCore()
		{
			base.ActivateCore();

			IsGround.OnValueChanged += HandleGroundValueChange;
			gameEventChannel.AddListener<GroundCheckerSizeChangedEvent>(HandleGroundCheckerSizeChanged);
			SetKinematic(false);
		}

		protected override void DeActivateCore()
		{
			IsGround.OnValueChanged -= HandleGroundValueChange;
			gameEventChannel.RemoveListener<GroundCheckerSizeChangedEvent>(HandleGroundCheckerSizeChanged);
			boxGroundChecker.SetCheckerSize(Vector3.zero);

			base.DeActivateCore();
		}

		private void FixedUpdate()
		{
			if (!IsActived)
				return;

			CalculateMovement();
			if (useGravity)
				ApplyGravity();

			LookAtRotation();
			Move();
		}

		private void HandleGroundValueChange(bool previousValue, bool nextValue)
		{
			if (nextValue)
			{
				_currentJumpCount = 0;
				fallSoundPlayer.PlaySound();
			}
		}

		private void HandleGroundCheckerSizeChanged(GroundCheckerSizeChangedEvent @event)
		{
			boxGroundChecker.SetCheckerSize(@event.size);
		}

		/// <summary>
		/// 절대 좌표계 기준으로 움직일 방향을 설정합니다.
		/// </summary>
		public void SetMovementDirection(Vector2 direction)
		{
			_movementDirection = new Vector3(direction.x, 0, direction.y).normalized;
			_movementDirection.y = _verticalVelocity;
		}

		/// <summary>
		/// 절대 좌표계 기준으로 움직일 방향을 설정합니다.
		/// </summary>
		public void SetMovementDirection(Vector2 direction, Quaternion rotation)
		{
			_movementDirection = rotation * new Vector3(direction.x, 0, direction.y).normalized;
			_movementDirection.y = _verticalVelocity;
		}
		public void SetMovement(Vector3 velocity)
		{
			_movementDirection = velocity;
		}

		public void SetRotationDirection(Quaternion rotation)
		{
			_lookTargetRotation = rotation;
		}

		public void SetRotation(Quaternion rotation)
		{
			_rigidComp.transform.rotation = rotation;
		}

		public bool Jump()
		{
			// 21억번 눌러서 오버플로우내면 그건 솔직히 대단하니까 인정해주자.
			if (_currentJumpCount++ < maxJumpCount)
			{
				_verticalVelocity = jumpPower;
				jumpSoundPlayer.PlaySound();
                return true;
			}

			return false;
		}

		private void CalculateMovement()
		{
			if (CanMovement)
			{
				_velocity = _movementDirection; // Quaternion은 교환 법칙이 성립하지 않는다.
				_velocity *= moveSpeed;
			}

			if (useGravity)
				_velocity.y = _verticalVelocity;
		}

		private void LookAtRotation()
		{
			if (CanRotation)
			{
				float rotationSpeed = 8f;
				SetRotation(Quaternion.Lerp(_rigidComp.transform.rotation, _lookTargetRotation, rotationSpeed * Time.fixedDeltaTime));
			}
		}

		private void Move()
		{
			_rigidComp.linearVelocity = _velocity;
		}

		private void ApplyGravity()
		{
			if (!IsGround.Value)
				FallTime.Value += Time.fixedDeltaTime;
			else
				FallTime.Value = 0;

			if (IsGround.Value && _verticalVelocity < 0)
				_verticalVelocity = -0.03f;
			else
				_verticalVelocity += gravityScale * Time.fixedDeltaTime;
		}

		public void StopImmediately()
		{
			_velocity = Vector3.zero;
			_movementDirection = Vector3.zero;
			_verticalVelocity = 0;
			_rigidComp.linearVelocity = Vector3.zero;
		}

		public void SetUseGravity(bool value)
			=> useGravity = value;

		public void SetKinematic(bool value)
		{
			_isKinematic = value;
			_rigidComp.isKinematic = _isKinematic;
		}
		public void SetPosition(Vector3 position)
		{
			_rigidComp.position=position;
		}
	}
}
