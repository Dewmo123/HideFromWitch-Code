using Assets._00.Work.YHB.Scripts.Core;
using Assets._00.Work.YHB.Scripts.Entities;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Executors
{
	public class ChangeVerticalVelocityExecutor : ActiverMono
	{
		[SerializeField] private InputSO inputSO;
		[SerializeField] private GameObject verticalVelocityObject;

		private IVerticalVelocitySettable _verticalVelocitySettable;
		private float _vertical = 0;

		public override void EarlyInitialize()
		{
			base.EarlyInitialize();

			if (!verticalVelocityObject.TryGetComponent<IVerticalVelocitySettable>(out IVerticalVelocitySettable verticalVelocity))
			{
				Debug.LogError("verticalVelocitySettable can't be null");
				return;
			}

			_verticalVelocitySettable = verticalVelocity;
		}

		protected override void ActivateCore()
		{
			inputSO.OnSprintStatusChangeEvent += HandleSprintStatusChangeEvent;
			inputSO.OnJumpStatusChangeEvent += HandleJumpStatusChangeEvent;
		}

		protected override void DeActivateCore()
		{
			inputSO.OnSprintStatusChangeEvent -= HandleSprintStatusChangeEvent;
			inputSO.OnJumpStatusChangeEvent -= HandleJumpStatusChangeEvent;
		}

		private void HandleSprintStatusChangeEvent(bool obj)
		{
			_vertical += obj ? -1 : 1;
			_verticalVelocitySettable.SetVerticalVelocity(_vertical);
		}

		private void HandleJumpStatusChangeEvent(bool obj)
		{
			_vertical += obj ? 1 : -1;
			_verticalVelocitySettable.SetVerticalVelocity(_vertical);
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (!verticalVelocityObject.TryGetComponent<IVerticalVelocitySettable>(out IVerticalVelocitySettable verticalVelocity))
			{
				verticalVelocityObject = null;
				Debug.LogError("verticalVelocitySettable can't be null");
				return;
			}
		}
#endif
	}
}
