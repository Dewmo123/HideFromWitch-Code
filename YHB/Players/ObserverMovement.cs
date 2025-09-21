using Alchemy.Inspector;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.Executors;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Players
{
	public class ObserverMovement : ActiverMono, IMover, IVerticalVelocitySettable
	{
		[FoldoutGroup("Move Speed Setting")]
		[LabelText("Default")]
		[SerializeField] private float defaultMoveSpeed = 5;
		[FoldoutGroup("Move Speed Setting")]
		[LabelText("Max")]
		[SerializeField] private float maxMoveSpeed = 5;
		[FoldoutGroup("Move Speed Setting")]
		[LabelText("Increase")]
		[SerializeField] private float moveIncreaseSpeed = 0.5f;

		private Vector3 _inputDirection;
		private Vector3 _movementDirection;
		private float _moveSpeed;
		public float MoveSpeed
		{
			get => _moveSpeed;
			set
			{
				_moveSpeed = Mathf.Min(value, maxMoveSpeed);
			}
		}

		private float _verticalVelocity;

		protected override void ActivateCore()
		{
			transform.localPosition = Vector3.zero;
		}

		protected override void DeActivateCore()
		{
			transform.localPosition = Vector3.zero;
			IsActived = false;
		}

		private void FixedUpdate()
		{
			Move();
		}

		private void Move()
		{
			transform.position += MoveSpeed * _movementDirection * Time.deltaTime;
			MoveSpeed += moveIncreaseSpeed * Time.fixedDeltaTime;
		}

		public void SetVerticalVelocity(float value)
		{
			_verticalVelocity = value;
			ApplyVerticalVelocity();
		}

		public void SetMovementDirection(Vector2 direction)
		{
			SetMovement(new Vector3(direction.x, _verticalVelocity, direction.y));
		}

		public void SetMovementDirection(Vector2 direction, Quaternion rotation)
		{
			Vector3 dir = rotation * new Vector3(direction.x, 0, direction.y);
			SetMovement(dir);
		}

		public void SetMovement(Vector3 direction)
		{
			if (direction == Vector3.zero)
			{
				_inputDirection = Vector3.zero;
				MoveSpeed = defaultMoveSpeed;
			}
			else
				_inputDirection = direction.normalized;

			_movementDirection = _inputDirection;

			ApplyVerticalVelocity();
		}

		private void ApplyVerticalVelocity()
		{
			_movementDirection.y = _verticalVelocity;
		}
	}
}
