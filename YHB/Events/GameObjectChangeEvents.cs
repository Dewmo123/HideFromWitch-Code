using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.Players;
using DewmoLib.Utiles;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Events
{
	public class GameObjectChangeEvents
	{
		public static RotateEvent RotateEvent = new RotateEvent();
		public static MoveSpeedChangeEvent MoveSpeedChangeEvent = new MoveSpeedChangeEvent();
		public static MoveDirectionChangeEvent MoveDirectionChangeEvent = new MoveDirectionChangeEvent();
		public static PlayerChangeRoleEvent PlayerChangeRoleEvent = new PlayerChangeRoleEvent();
		public static GroundCheckerSizeChangedEvent GroundCheckerSizeChangedEvent = new GroundCheckerSizeChangedEvent();
	}

	public class GroundCheckerSizeChangedEvent : GameEvent
	{
		public Vector3 size;

		public GroundCheckerSizeChangedEvent Initialize(Vector3 size)
		{
			this.size = size;
			return this;
		}
	}

	public class PlayerChangeRoleEvent : GameEvent
	{
		public Role role;

		public PlayerChangeRoleEvent Initialize(Role role)
		{
			this.role = role;
			return this;
		}
	}

	public class RotateEvent : GameEvent
	{
		public EntityMovement entityMovement;
		public Quaternion rotateValue;
	}

	// 상호참조이나 우회하면 필요이상으로 복잡해 질듯 하여 상호참조로 놔두겠습니다.
	// 후일에 더 좋은 데이터 전달 방식이 필요해지면 해당 방식으로 같이 바꾸겠습니다.
	public class MoveSpeedChangeEvent : GameEvent
	{
		public EntityMovement entityMovement;
		public float previousMoveSpeed;
		public float newMoveSpeed;

		public MoveSpeedChangeEvent Initialize(EntityMovement movement, float previousValue, float newVlaue)
		{
			this.entityMovement = movement;
			this.previousMoveSpeed = previousValue;
			this.newMoveSpeed = newVlaue;
			return this;
		}
	}

	public class MoveDirectionChangeEvent : GameEvent
	{
		public EntityMovement entityMovement;
		public Vector2 inputDirection;
		public Quaternion rotation;

		public MoveDirectionChangeEvent Initialize(EntityMovement entityMovement, Vector2 inputDirection, Quaternion rotation)
		{
			this.entityMovement = entityMovement;
			this.inputDirection = inputDirection;
			this.rotation = rotation;
			return this;
		}
	}
}
