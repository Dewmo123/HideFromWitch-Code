using Alchemy.Inspector;
using Assets._00.Work.YHB.Scripts.Core;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.Executors;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Others
{
	public class CameraZoomOutComponent : ActiverMono
	{
		[SerializeField] private InputSO inputSO;
		[SerializeField] private CinemachinePositionComposer positionComposer;

		[FoldoutGroup("Camera Zoom")]
		[SerializeField] private float startZoomValue;
		[FoldoutGroup("Camera Zoom")]
		[SerializeField] private bool activeWithSetStartZoomValue;
		[FoldoutGroup("Camera Zoom")]
		[SerializeField] private float zoomSpeed;
		[FoldoutGroup("Camera Zoom")]
		[SerializeField] private float minZoomValue = 5;
		[FoldoutGroup("Camera Zoom")]
		[SerializeField] private float maxZoomValue = 20;

		private float _lastZoomValue;

		private void Awake()
		{
			_lastZoomValue = startZoomValue;
		}

		protected override void ActivateCore()
		{
			inputSO.OnZoomOutDeltaEvent += HandleZoomOutDeltaEvent;

			if (activeWithSetStartZoomValue)
				SetCameraDistance(startZoomValue);
			else
				SetCameraDistance(_lastZoomValue);
		}

		protected override void DeActivateCore()
		{
			inputSO.OnZoomOutDeltaEvent -= HandleZoomOutDeltaEvent;

			_lastZoomValue = positionComposer.CameraDistance;
		}

		private void HandleZoomOutDeltaEvent(float delta)
		{
			float cameraDistance = positionComposer.CameraDistance + zoomSpeed * delta;
			SetCameraDistance(cameraDistance);
		}

		private void SetCameraDistance(float value)
		{
			positionComposer.CameraDistance = Mathf.Clamp(value, minZoomValue, maxZoomValue);
		}
	}
}
