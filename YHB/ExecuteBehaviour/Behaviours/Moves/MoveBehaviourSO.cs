using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.ExecuteBehaviour.Behaviours.Moves
{
	[CreateAssetMenu(fileName = "MoveBehaviour", menuName = "SO/ScriptableBehaviour/Behaviour/Move/Move", order = 0)]
	public class MoveBehaviourSO : ScriptableBehaviourSO
	{
		protected override bool LogicExecute<T>(T data)
		{
			if (data is not MovementData movementData)
				return false;

			movementData.movement.SetMovementDirection((Vector2)movementData.moveDirection, movementData.moveRotation);
			return true;
		}
	}
}
