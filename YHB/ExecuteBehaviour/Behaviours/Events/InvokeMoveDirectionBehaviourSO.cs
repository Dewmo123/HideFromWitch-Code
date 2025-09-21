using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.Events;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using DewmoLib.Utiles;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.ExecuteBehaviour.Behaviours.Events
{
	[CreateAssetMenu(fileName = "InveokeMoveDirectionValueBehaviour", menuName = "SO/ScriptableBehaviour/Behaviour/Event/InveokeMoveDirectionValue", order = 0)]
	public class InvokeMoveDirectionBehaviourSO : ScriptableBehaviourSO
	{
		[SerializeField] private EventChannelSO gameEvent;

		protected override bool LogicExecute<T>(T data)
		{
			if (data is not MovementData moveData)
				return false;

			EntityMovement entityMovement = moveData.movement as EntityMovement;
			if (entityMovement == null)
				return false;

			GameObjectChangeEvents.MoveDirectionChangeEvent.entityMovement = entityMovement;
			GameObjectChangeEvents.MoveDirectionChangeEvent.inputDirection = moveData.moveDirection;
			GameObjectChangeEvents.MoveDirectionChangeEvent.rotation = moveData.moveRotation;

			gameEvent.InvokeEvent(GameObjectChangeEvents.MoveDirectionChangeEvent);
			return true;
		}
	}
}
