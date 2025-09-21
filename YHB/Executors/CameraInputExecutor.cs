using Assets._00.Work.YHB.Scripts.Core;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Alchemy.Inspector;
using Assets._00.Work.YHB.Scripts.Others;

namespace Assets._00.Work.YHB.Scripts.Executors
{
	public class CameraInputExecutor : ActiverMono
	{
		[Header("Setting Value")]
		[SerializeField] protected InputSO inputSO;
		[field: SerializeField] protected ScriptableBehaviourSO CameraRotateInputBehaviour { get; set; }
		[SerializeField] protected Transform cameraParent;

		[SerializeField] protected CameraRotateValueSO cameraRotateValueSO;

		protected CameraValueChangeData _cameraData;

		public override void EarlyInitialize()
		{
			base.EarlyInitialize();

			_cameraData = new CameraValueChangeData();
			_cameraData.cameraParent = cameraParent;
			_cameraData.rotationXMin = cameraRotateValueSO.rotationXMin;
			_cameraData.rotationXMax = cameraRotateValueSO.rotationXMax;
		}

		protected override void ActivateCore()
		{
			inputSO.OnLookChangedEvent += HandleLookChangedEvent;
		}

		protected override void DeActivateCore()
		{
			inputSO.OnLookChangedEvent -= HandleLookChangedEvent;
		}

		protected void HandleLookChangedEvent(Vector2 vector)
		{
			_cameraData.cameraRotateValue.x = -vector.y;
			_cameraData.cameraRotateValue.y = vector.x;

			_cameraData.cameraRotateValue *= cameraRotateValueSO.rotationSensitivity;
			ExecuteCaremaRotateBehaviour();
		}

		public void ExecuteCaremaRotateBehaviour()
		{
			CameraRotateInputBehaviour.Execute<CameraValueChangeData>(_cameraData);
		}
	}
}
