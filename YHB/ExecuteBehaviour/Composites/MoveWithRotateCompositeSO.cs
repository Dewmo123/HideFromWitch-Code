using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.ExecuteBehaviour.Composites
{
	[CreateAssetMenu(fileName = "Composite_MoveWithRotate", menuName = "SO/ScriptableBehaviour/Composite/MoveWithRotate", order = 0)]
	public class MoveWithRotateCompositeSO : DefaultCompositeSO
	{
		[SerializeField] private ScriptableBehaviourSO rotateBehaviour;

		private static RotateValueData _rotatationValueData = new RotateValueData();

		protected override bool LogicExecute<T>(T data)
		{
			if (data is not MovementData movementData)
				return false;

			if (movementData.moveDirection != Vector3.zero)
			{
				Vector3 moveDirection = new Vector3(movementData.moveDirection.x, 0, movementData.moveDirection.y);
				Quaternion rotation = Quaternion.Euler(0, movementData.moveRotation.eulerAngles.y, 0);

				_rotatationValueData.rotateValue = Quaternion.LookRotation(rotation * moveDirection);
				_rotatationValueData.entityMovement = movementData.movement as EntityMovement;

				if (_rotatationValueData.entityMovement == null)
					return false;

				rotateBehaviour.Execute(_rotatationValueData);
			}

			return base.LogicExecute<T>(data);
		}
	}
}
