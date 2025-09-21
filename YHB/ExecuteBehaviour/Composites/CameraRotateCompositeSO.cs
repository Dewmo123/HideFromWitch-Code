using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.ExecuteBehaviour.Composites
{
	[CreateAssetMenu(fileName = "Composite_CameraRotate", menuName = "SO/ScriptableBehaviour/Composite/CameraRotate", order = 0)]

	public class CameraRotateCompositeSO : CompositeBehaviourSO
	{
		private static RotateValueData _rotatationValueData = new RotateValueData();

		public override bool CanExecuteNext<T>(T data)
		{
			return true;
		}

		protected override bool LogicExecute<T>(T data)
		{
			ExecuteBeforeBehaviour(data);

			if (data is not CameraValueChangeData cameraValue || !HaveNextBehaviour)
				return false;

			Vector3 rotation = cameraValue.cameraParent.rotation.eulerAngles;
			rotation.x = 0;
			rotation.z = 0;

			if (rotation == Vector3.zero)
				return false;

			_rotatationValueData.rotateValue = Quaternion.Euler(rotation);
			_rotatationValueData.entityMovement = cameraValue.mover as EntityMovement;

			if (_rotatationValueData.entityMovement == null)
				return false;

			return TryExecuteNext(_rotatationValueData);
		}
	}
}
